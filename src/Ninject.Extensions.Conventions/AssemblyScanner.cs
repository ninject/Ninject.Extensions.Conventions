#region License

// 
// Author: Ian Davis <ian@innovatian.com>
// Based on StructureMap 2.5 AssemblyScanner by Jeremy Miller.
// 
// Dual-licensed under the Apache License, Version 2.0, and the Microsoft Public License (Ms-PL).
// See the file LICENSE.txt for details.
// 

#endregion

#region Using Directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using Ninject.Activation;
using Ninject.Infrastructure;

#endregion

namespace Ninject.Extensions.Conventions
{
    using System.Security;

    /// <summary>
    /// 
    /// </summary>
    public class AssemblyScanner : IAssemblyScanner
    {
#if !NO_ASSEMBLY_SCANNING
        private bool _autoLoadModules;
#endif

        /// <summary>
        /// Initializes a new instance of the <see cref="AssemblyScanner"/> class.
        /// </summary>
        public AssemblyScanner()
        {
            Includes = new List<Type>();
            Excludes = new List<Type>();
            FilteredTypes = new List<Type>();
            Filters = new List<Predicate<Type>>();
            BindingGenerators = new List<IBindingGenerator>();
            TargetAssemblies = new List<Assembly>();
        }

        /// <summary>
        /// The scope callback delegate to use in binding generation
        /// </summary>
        protected Func<IContext, object> ScopeCallback { get; set; }

        /// <summary>
        /// The collection of assemblies to process.
        /// </summary>
        protected List<Assembly> TargetAssemblies { get; private set; }

        /// <summary>
        /// The generators used to create bindings
        /// </summary>
        public List<IBindingGenerator> BindingGenerators { get; private set; }

        /// <summary>
        /// Used to remove specifically exclude types from processing
        /// </summary>
        protected List<Type> Excludes { get; private set; }

        /// <summary>
        /// The filters used to remove potential binding candidates.
        /// </summary>
        protected List<Predicate<Type>> Filters { get; private set; }

        /// <summary>
        /// The filters used to identify potential binding candidates.
        /// </summary>
        protected List<Type> Includes { get; private set; }

        /// <summary>
        /// The filters used to identify potential binding candidates.
        /// </summary>
        protected List<Type> FilteredTypes { get; private set; }

        /// <summary>
        /// Performs binding generation on all targeted assemblies.
        /// </summary>
        /// <param name="kernel"></param>
        internal void Process( IKernel kernel )
        {
            TargetAssemblies.ForEach( assembly => Process( assembly, kernel ) );
        }

        private void Process( Assembly assembly, IKernel kernel )
        {
#if !NO_ASSEMBLY_SCANNING
            if ( _autoLoadModules && assembly.HasNinjectModules() )
            {
                kernel.Load( assembly );
            }
#endif

            if ( ScopeCallback == null )
            {
                ScopeCallback = StandardScopeCallbacks.Transient;
            }

            Process( assembly.GetExportedTypesPlatformSafe(), kernel );
            Process( FilteredTypes, kernel );

            foreach ( Type exportedType in Includes )
            {
                Type type = exportedType;
                BindingGenerators.ForEach( bindingGenerator => bindingGenerator.Process( type, ScopeCallback, kernel ) );
            }
        }

        private void Process( IEnumerable<Type> types, IKernel kernel )
        {
            foreach ( Type exportedType in types )
            {
                Type type = exportedType;
                bool exclude = Excludes.Contains( type );
                if ( exclude )
                {
                    continue;
                }

                exclude = Filters.Any( filter => !filter( type ) );
                if ( exclude )
                {
                    continue;
                }

                BindingGenerators.ForEach( bindingGenerator => bindingGenerator.Process( type, ScopeCallback, kernel ) );
            }
        }

        #region Implementation of IAssemblyScanner

        /// <summary>
        /// Loads the specified assembly.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        public void From( Assembly assembly )
        {
            if ( !TargetAssemblies.Contains( assembly ) )
            {
                TargetAssemblies.Add( assembly );
            }
        }

#if !NETCF && !SILVERLIGHT
        /// <summary>
        /// 
        /// </summary>
        public void FromCallingAssembly()
        {
            var trace = new StackTrace( false );

            Assembly thisAssembly = Assembly.GetExecutingAssembly();
            Assembly callingAssembly = null;
            for ( int i = 0; i < trace.FrameCount; i++ )
            {
                StackFrame currentFrame = trace.GetFrame( i );
                Assembly currentFrameAssembly = currentFrame.GetMethod().DeclaringType.Assembly;
                if ( currentFrameAssembly == thisAssembly )
                {
                    continue;
                }

                callingAssembly = currentFrameAssembly;
                break;
            }

            if ( callingAssembly != null )
            {
                From( callingAssembly );
            }
        }
#endif //!NETCF

        /// <summary>
        /// Loads the specified assemblies.
        /// </summary>
        /// <param name="assemblies">The assemblies.</param>
        public void From( IEnumerable<Assembly> assemblies )
        {
            foreach ( Assembly assembly in assemblies.Where( assembly => !TargetAssemblies.Contains( assembly ) ) )
            {
                TargetAssemblies.Add( assembly );
            }
        }

#if !NO_ASSEMBLY_SCANNING
        private static IEnumerable<string> GetFilesMatchingPattern( string pattern )
        {
            string path = NormalizePath( Path.GetDirectoryName( pattern ) );
            string glob = Path.GetFileName( pattern );

            return Directory.GetFiles( path, glob );
        }

        private static string NormalizePath( string path )
        {
            if ( !Path.IsPathRooted( path ) )
            {
                path = Path.Combine( GetBaseDirectory(), path );
            }

            return Path.GetFullPath( path );
        }

        private static string GetBaseDirectory()
        {
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string searchPath = AppDomain.CurrentDomain.RelativeSearchPath;

            return String.IsNullOrEmpty( searchPath ) ? baseDirectory : Path.Combine( baseDirectory, searchPath );
        }

        private static IEnumerable<AssemblyName> FindAssemblies( IEnumerable<string> filenames,
                                                                 Predicate<Assembly> filter )
        {
            AppDomain temporaryDomain = CreateTemporaryAppDomain();

            foreach ( string file in filenames )
            {
                Assembly assembly;

                if (File.Exists(file))
                {
                    try
                    {
                        var name = new AssemblyName { CodeBase = file };
                        assembly = temporaryDomain.Load(name);
                    }
                    catch (BadImageFormatException)
                    {
                        // Ignore native assemblies
                        continue;
                    }
                }
                else
                {
                    assembly = temporaryDomain.Load(file);
                }

                if ( filter( assembly ) )
                {
                    yield return assembly.GetName();
                }
            }

            AppDomain.Unload( temporaryDomain );
        }

        private static AppDomain CreateTemporaryAppDomain()
        {
            return AppDomain.CreateDomain(
                "AssemblyScanner",
                AppDomain.CurrentDomain.Evidence,
                AppDomain.CurrentDomain.SetupInformation );
        }

        /// <summary>
        /// Loads the specified assembly.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        public void From( string assembly )
        {
            if ( !TargetAssemblies.Any(
                asm => string.Equals( asm.GetName().Name, assembly, StringComparison.OrdinalIgnoreCase ) ) )
            {
                From( new[] {assembly} );
            }
        }

        /// <summary>
        /// Loads the specified assemblies.
        /// </summary>
        /// <param name="assemblies">The assemblies.</param>
        public void From( IEnumerable<string> assemblies )
        {
            From( assemblies, filter => true );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="assemblies"></param>
        /// <param name="filter"></param>
        public void From( IEnumerable<string> assemblies, Predicate<Assembly> filter )
        {
            IEnumerable<AssemblyName> assemblyNames = FindAssemblies( assemblies, filter );
            IEnumerable<Assembly> assemblyInstances =
                assemblyNames.Select( name => Assembly.Load( name ) );
            foreach ( Assembly assembly in assemblyInstances )
            {
                From( assembly );
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        public void FromAssembliesInPath( string path )
        {
            FromAssembliesInPath( path, filter => true );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="assemblyFilter"></param>
        public void FromAssembliesInPath( string path, Predicate<Assembly> assemblyFilter )
        {
            IEnumerable<string> assemblies =
                Directory.GetFiles( path )
                    .Where( IsAssemblyFile );
            From( assemblies, assemblyFilter );
        }

        private static bool IsAssemblyFile( string fileName )
        {
            string extension = Path.GetExtension( fileName );
            return HasAssemblyExtension( extension );
        }

        private static bool HasAssemblyExtension( string extension )
        {
            const StringComparison comparison = StringComparison.OrdinalIgnoreCase;
            return string.Equals( extension, ".dll", comparison ) ||
                   string.Equals( extension, ".exe", comparison );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pattern"></param>
        public void FromAssembliesMatching( string pattern )
        {
            FromAssembliesMatching( new[] {pattern} );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="patterns"></param>
        public void FromAssembliesMatching( IEnumerable<string> patterns )
        {
            IEnumerable<string> files = patterns
                .SelectMany( pattern => GetFilesMatchingPattern( pattern ) );

            From( files.Where( IsAssemblyFile ) );
        }

        /// <summary>
        /// 
        /// </summary>
        public void AutoLoadModules()
        {
            _autoLoadModules = true;
        }
#endif //!NO_ASSEMBLY_SCANNING

        /// <summary>
        /// Loads the assembly containing.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void FromAssemblyContaining<T>()
        {
            From( typeof (T).Assembly );
        }

        /// <summary>
        /// Loads the assembly containing.
        /// </summary>
        /// <param name="type">The type.</param>
        public void FromAssemblyContaining( Type type )
        {
            From( type.Assembly );
        }

        /// <summary>
        /// Loads the assembly containing.
        /// </summary>
        /// <param name="types">The types.</param>
        public void FromAssemblyContaining( IEnumerable<Type> types )
        {
            foreach ( Type type in types )
            {
                From( type.Assembly );
            }
        }

        /// <summary>
        /// Uses the supplied types for processing subject to excludes and filters.
        /// </summary>
        /// <param name="types"></param>
        public void From( IEnumerable<Type> types )
        {
            foreach ( Type type in types.Where( type => !FilteredTypes.Contains( type ) ) )
            {
                FilteredTypes.Add( type );
            }
        }

        /// <summary>
        /// Includes this instance.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void Including<T>()
        {
            Including( typeof (T) );
        }

        /// <summary>
        /// Includes the specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        public void Including( Type type )
        {
            if ( !Includes.Contains( type ) )
            {
                Includes.Add( type );
            }
        }

        /// <summary>
        /// Includes the specified types.
        /// </summary>
        /// <param name="types">The types.</param>
        public void Including( IEnumerable<Type> types )
        {
            foreach ( Type type in types )
            {
                Including( type );
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nameSpace"></param>
        public void WhereTypeIsInNamespace( string nameSpace )
        {
            Where( type => NamespacesMatch( type.Namespace, nameSpace ) );
        }

        /// <summary>
        /// Deternmines whether two namespaces match.
        /// </summary>
        /// <param name="lhs">The target namespace.</param>
        /// <param name="rhs">The namespace template to match against.</param>
        /// <returns></returns>
        private static bool NamespacesMatch( string lhs, string rhs )
        {
            return string.IsNullOrEmpty( lhs )
                       ? string.IsNullOrEmpty( rhs )
                       : lhs.StartsWith( rhs, StringComparison.OrdinalIgnoreCase );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void WhereTypeIsInNamespaceOf<T>()
        {
            WhereTypeIsInNamespace( typeof (T).Namespace );
        }

        /// <summary>
        /// Excludes this instance.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void Excluding<T>()
        {
            Excluding( typeof (T) );
        }

        /// <summary>
        /// Excludes the specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        public void Excluding( Type type )
        {
            if ( !Excludes.Contains( type ) )
            {
                Excludes.Add( type );
            }
        }

        /// <summary>
        /// Excludes the specified filters.
        /// </summary>
        /// <param name="filters">The filters.</param>
        public void Excluding( IEnumerable<Type> filters )
        {
            foreach ( Type type in filters )
            {
                Excluding( type );
            }
        }

        /// <summary>
        /// Excludes the specified filter.
        /// </summary>
        /// <param name="filter">The filter.</param>
        public void Where( Predicate<Type> filter )
        {
            Filters.Add( filter );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nameSpace"></param>
        public void WhereTypeIsNotInNamespace( string nameSpace )
        {
            Where( type => !NamespacesMatch( type.Namespace, nameSpace ) );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void WhereTypeIsNotInNamespaceOf<T>()
        {
            WhereTypeIsNotInNamespace( typeof (T).Namespace );
        }

        /// <summary>
        /// Usings this instance.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void BindWith<T>() where T : IBindingGenerator, new()
        {
            if ( !BindingGenerators.Any( bindingGenerator => bindingGenerator.GetType() == typeof (T) ) )
            {
                BindingGenerators.Add( new T() );
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void BindWith( IBindingGenerator generator )
        {
            if ( !BindingGenerators.Any( bindingGenerator => bindingGenerator.GetType() == generator.GetType() ) )
            {
                BindingGenerators.Add( generator );
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void BindWithDefaultConventions()
        {
            BindWith<DefaultBindingGenerator>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void WhereTypeInheritsFrom<T>()
        {
            WhereTypeInheritsFrom( typeof (T) );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        public void WhereTypeInheritsFrom( Type type )
        {
            Where( type.IsAssignableFrom );
        }

        /// <summary>
        /// </summary>
        /// <param name="types"></param>
        public void WhereTypeInheritsFromAny( IEnumerable<Type> types )
        {
            Where( type => types.Any( t => t.IsAssignableFrom( type ) ) );
        }

        /// <summary>
        /// Indicates that instances activated via the binding should be re-used as long as the object
        /// returned by the provided callback remains alive (that is, has not been garbage collected).
        /// </summary>
        /// <param name="scope">The callback that returns the scope.</param>
        public void InScope( Func<IContext, object> scope )
        {
            ScopeCallback = scope;
        }

        /// <summary>
        /// Indicates that only a single instance of the binding should be created, and then
        /// should be re-used for all subsequent requests.
        /// </summary>
        public void InSingletonScope()
        {
            ScopeCallback = StandardScopeCallbacks.Singleton;
        }

        /// <summary>
        /// Indicates that instances activated via the binding should not be re-used, nor have
        /// their lifecycle managed by Ninject.
        /// </summary>
        public void InTransientScope()
        {
            ScopeCallback = StandardScopeCallbacks.Transient;
        }

        /// <summary>
        /// Indicates that instances activated via the binding should be re-used within the same thread.
        /// </summary>
        public void InThreadScope()
        {
            ScopeCallback = StandardScopeCallbacks.Thread;
        }

#if !NO_WEB
        /// <summary>
        /// Indicates that instances activated via the binding should be re-used within the same
        /// HTTP request.
        /// </summary>
        public void InRequestScope()
        {
            ScopeCallback = StandardScopeCallbacks.Request;
        }
#endif

        #endregion
    }
}