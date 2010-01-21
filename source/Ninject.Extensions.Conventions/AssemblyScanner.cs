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
    /// <summary>
    /// 
    /// </summary>
    public class AssemblyScanner : IAssemblyScanner
    {
        private bool _autoLoadModules;

        public AssemblyScanner()
        {
            Includes = new List<Predicate<Type>>();
            Excludes = new List<Predicate<Type>>();
            BindingGenerators = new List<IBindingGenerator>();
            TargetAssemblies = new List<Assembly>();
        }

        protected Func<IContext, object> ScopeCallback { get; set; }

        protected List<Assembly> TargetAssemblies { get; private set; }

        public List<IBindingGenerator> BindingGenerators { get; private set; }

        protected List<Predicate<Type>> Excludes { get; private set; }

        protected List<Predicate<Type>> Includes { get; private set; }

        internal void Process( IKernel kernel )
        {
            TargetAssemblies.ForEach( assembly => Process( assembly, kernel ) );
        }

        private void Process( Assembly assembly, IKernel kernel )
        {
            if ( _autoLoadModules && assembly.HasNinjectModules() )
            {
                kernel.Load( assembly );
            }

            if ( ScopeCallback == null )
            {
                ScopeCallback = StandardScopeCallbacks.Transient;
            }
            foreach ( Type exportedType in assembly.GetExportedTypes() )
            {
                Type type = exportedType;
                bool include = Includes.Count == 0 || Includes.Exists( predicate => predicate( type ) );
                if ( !include )
                {
                    continue;
                }

                bool exclude = Excludes.Exists( predicate => predicate( type ) );
                if ( exclude )
                {
                    continue;
                }

                BindingGenerators.ForEach( bindingGenerator => bindingGenerator.Process( type, ScopeCallback, kernel ) );
            }
        }

        private static string[] GetFilesMatchingPattern( string pattern )
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

                try
                {
                    var name = new AssemblyName {CodeBase = file};
                    assembly = temporaryDomain.Load( name );
                }
                catch ( BadImageFormatException )
                {
                    // Ignore native assemblies
                    continue;
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

        #region Implementation of IAssemblyScanner

        /// <summary>
        /// Loads the specified assembly.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        public void Assembly( Assembly assembly )
        {
            if ( !TargetAssemblies.Contains( assembly ) )
            {
                TargetAssemblies.Add( assembly );
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void CallingAssembly()
        {
            var trace = new StackTrace( false );

            Assembly thisAssembly = System.Reflection.Assembly.GetExecutingAssembly();
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
                Assembly( callingAssembly );
            }
        }

        /// <summary>
        /// Loads the specified assemblies.
        /// </summary>
        /// <param name="assemblies">The assemblies.</param>
        public void Assemblies( IEnumerable<Assembly> assemblies )
        {
            foreach ( Assembly assembly in assemblies )
            {
                if ( !TargetAssemblies.Contains( assembly ) )
                {
                    TargetAssemblies.Add( assembly );
                }
            }
        }

        /// <summary>
        /// Loads the specified assembly.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        public void Assembly( string assembly )
        {
            if ( !TargetAssemblies.Exists(
                      asm => string.Equals( asm.GetName().Name, assembly, StringComparison.OrdinalIgnoreCase ) ) )
            {
                Assembly( AppDomain.CurrentDomain.Load( assembly ) );
            }
        }

        /// <summary>
        /// Loads the specified assemblies.
        /// </summary>
        /// <param name="assemblies">The assemblies.</param>
        public void Assemblies( IEnumerable<string> assemblies )
        {
            Assemblies( assemblies, filter => true );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="assemblies"></param>
        /// <param name="filter"></param>
        public void Assemblies( IEnumerable<string> assemblies, Predicate<Assembly> filter )
        {
            IEnumerable<AssemblyName> assemblyNames = FindAssemblies( assemblies, filter );
            IEnumerable<Assembly> assemblyInstances =
                assemblyNames.Select( name => System.Reflection.Assembly.Load( name ) );
            foreach ( Assembly assembly in assemblyInstances )
            {
                Assembly( assembly );
            }
        }

        /// <summary>
        /// Loads the assembly containing.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void AssemblyContaining<T>()
        {
            Assembly( typeof (T).Assembly );
        }

        /// <summary>
        /// Loads the assembly containing.
        /// </summary>
        /// <param name="type">The type.</param>
        public void AssemblyContaining( Type type )
        {
            Assembly( type.Assembly );
        }

        /// <summary>
        /// Loads the assembly containing.
        /// </summary>
        /// <param name="types">The types.</param>
        public void AssemblyContaining( IEnumerable<Type> types )
        {
            foreach ( Type type in types )
            {
                Assembly( type.Assembly );
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        public void AssembliesFromPath( string path )
        {
            AssembliesFromPath( path, filter => true );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="assemblyFilter"></param>
        public void AssembliesFromPath( string path, Predicate<Assembly> assemblyFilter )
        {
            const StringComparison comparison = StringComparison.OrdinalIgnoreCase;
            IEnumerable<string> assemblies = Directory.GetFiles( path ).Where( file =>
                                                                               {
                                                                                   string extension =
                                                                                       Path.GetExtension( file );
                                                                                   bool match =
                                                                                       string.Equals( extension, ".dll",
                                                                                                      comparison ) ||
                                                                                       string.Equals( extension, ".exe",
                                                                                                      comparison );
                                                                                   return match;
                                                                               } );
            Assemblies( assemblies, assemblyFilter );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pattern"></param>
        public void AssembliesMatching( string pattern )
        {
            AssembliesMatching( new[] {pattern} );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="patterns"></param>
        public void AssembliesMatching( IEnumerable<string> patterns )
        {
            const StringComparison comparison = StringComparison.OrdinalIgnoreCase;
            IEnumerable<string> files = patterns
                .SelectMany( pattern => GetFilesMatchingPattern( pattern ) );

            foreach ( string file in files )
            {
                string extension = Path.GetExtension( file );
                bool match =
                    string.Equals( extension, ".dll", comparison ) ||
                    string.Equals( extension, ".exe", comparison );
                if ( !match )
                {
                    continue;
                }
                Assembly( file );
            }
        }

        /// <summary>
        /// Includes this instance.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void IncludeType<T>()
        {
            IncludeType( typeof (T) );
        }

        /// <summary>
        /// Includes the specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        public void IncludeType( Type type )
        {
            IncludeTypes( scannedType => scannedType == type );
        }

        /// <summary>
        /// Includes the specified filters.
        /// </summary>
        /// <param name="filters">The filters.</param>
        public void IncludeTypes( IEnumerable<Type> filters )
        {
            foreach ( Type type in filters )
            {
                IncludeType( type );
            }
        }

        /// <summary>
        /// Includes the specified filter.
        /// </summary>
        /// <param name="filter">The filter.</param>
        public void IncludeTypes( Predicate<Type> filter )
        {
            Includes.Add( filter );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nameSpace"></param>
        public void IncludeNamespace( string nameSpace )
        {
            IncludeTypes( type => type.Namespace.StartsWith( nameSpace, StringComparison.OrdinalIgnoreCase ) );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void IncludeNamespaceContainingType<T>()
        {
            IncludeNamespace( typeof (T).Namespace );
        }

        /// <summary>
        /// Excludes this instance.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void ExcludeType<T>()
        {
            ExcludeType( typeof (T) );
        }

        /// <summary>
        /// Excludes the specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        public void ExcludeType( Type type )
        {
            ExcludeTypes( scannedType => scannedType == type );
        }

        /// <summary>
        /// Excludes the specified filters.
        /// </summary>
        /// <param name="filters">The filters.</param>
        public void ExcludeTypes( IEnumerable<Type> filters )
        {
            foreach ( Type type in filters )
            {
                ExcludeType( type );
            }
        }

        /// <summary>
        /// Excludes the specified filter.
        /// </summary>
        /// <param name="filter">The filter.</param>
        public void ExcludeTypes( Predicate<Type> filter )
        {
            Excludes.Add( filter );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nameSpace"></param>
        public void ExcludeNamespace( string nameSpace )
        {
            ExcludeTypes( type => type.Namespace.StartsWith( nameSpace, StringComparison.OrdinalIgnoreCase ) );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void ExcludeNamespaceContainingType<T>()
        {
            ExcludeNamespace( typeof (T).Namespace );
        }

        /// <summary>
        /// Usings this instance.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void Using<T>() where T : IBindingGenerator, new()
        {
            if ( !BindingGenerators.Exists( bindingGenerator => bindingGenerator.GetType() == typeof (T) ) )
            {
                BindingGenerators.Add( new T() );
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Using( IBindingGenerator generator )
        {
            if ( !BindingGenerators.Exists( bindingGenerator => bindingGenerator.GetType() == generator.GetType() ) )
            {
                BindingGenerators.Add( generator );
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void UsingDefaultConventions()
        {
            Using<DefaultBindingGenerator>();
        }

        /// <summary>
        /// 
        /// </summary>
        public void AutoLoadModules()
        {
            _autoLoadModules = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void IncludeAllTypesOf<T>()
        {
            IncludeAllTypesOf( typeof (T) );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        public void IncludeAllTypesOf( Type type )
        {
            BindingGenerators.Add( new RegexBindingGenerator( type.Name ) );
            IncludeTypes( type.IsAssignableFrom );
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