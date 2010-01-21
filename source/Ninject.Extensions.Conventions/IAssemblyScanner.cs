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
using System.Reflection;
using Ninject.Activation;

#endregion

namespace Ninject.Extensions.Conventions
{
    /// <summary>
    /// 
    /// </summary>
    public interface IAssemblyScanner
    {
        /// <summary>
        /// Loads the specified assembly.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        void Assembly( Assembly assembly );

        /// <summary>
        /// 
        /// </summary>
        void CallingAssembly();

        /// <summary>
        /// Loads the specified assemblies.
        /// </summary>
        /// <param name="assemblies">The assemblies.</param>
        void Assemblies( IEnumerable<Assembly> assemblies );

        /// <summary>
        /// Loads the specified assembly.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        void Assembly( string assembly );

        /// <summary>
        /// Loads the specified assemblies.
        /// </summary>
        /// <param name="assemblies">The assemblies.</param>
        void Assemblies( IEnumerable<string> assemblies );

        /// <summary>
        /// 
        /// </summary>
        /// <param name="assemblies"></param>
        /// <param name="filter"></param>
        void Assemblies( IEnumerable<string> assemblies, Predicate<Assembly> filter );

        /// <summary>
        /// Loads the assembly containing.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        void AssemblyContaining<T>();

        /// <summary>
        /// Loads the assembly containing.
        /// </summary>
        /// <param name="type">The type.</param>
        void AssemblyContaining( Type type );

        /// <summary>
        /// Loads the assembly containing.
        /// </summary>
        /// <param name="types">The types.</param>
        void AssemblyContaining( IEnumerable<Type> types );

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        void AssembliesFromPath( string path );

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="assemblyFilter"></param>
        void AssembliesFromPath( string path, Predicate<Assembly> assemblyFilter );

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pattern"></param>
        void AssembliesMatching( string pattern );

        /// <summary>
        /// 
        /// </summary>
        /// <param name="patterns"></param>
        void AssembliesMatching( IEnumerable<string> patterns );

        /// <summary>
        /// Includes this instance.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        void IncludeType<T>();

        /// <summary>
        /// Includes the specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        void IncludeType( Type type );

        /// <summary>
        /// Includes the specified filters.
        /// </summary>
        /// <param name="filters">The filters.</param>
        void IncludeTypes( IEnumerable<Type> filters );

        /// <summary>
        /// Includes the specified filter.
        /// </summary>
        /// <param name="filter">The filter.</param>
        void IncludeTypes( Predicate<Type> filter );

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nameSpace"></param>
        void IncludeNamespace( string nameSpace );

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        void IncludeNamespaceContainingType<T>();

        /// <summary>
        /// Excludes this instance.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        void ExcludeType<T>();

        /// <summary>
        /// Excludes the specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        void ExcludeType( Type type );

        /// <summary>
        /// Excludes the specified filters.
        /// </summary>
        /// <param name="filters">The filters.</param>
        void ExcludeTypes( IEnumerable<Type> filters );

        /// <summary>
        /// Excludes the specified filter.
        /// </summary>
        /// <param name="filter">The filter.</param>
        void ExcludeTypes( Predicate<Type> filter );

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nameSpace"></param>
        void ExcludeNamespace( string nameSpace );

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        void ExcludeNamespaceContainingType<T>();

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        void Using<T>() where T : IBindingGenerator, new();

        /// <summary>
        /// 
        /// </summary>
        void Using( IBindingGenerator generator );

        /// <summary>
        /// 
        /// </summary>
        void UsingDefaultConventions();

        /// <summary>
        /// 
        /// </summary>
        void AutoLoadModules();

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        void IncludeAllTypesOf<T>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        void IncludeAllTypesOf( Type type );

        /// <summary>
        /// Indicates that instances activated via the binding should be re-used as long as the object
        /// returned by the provided callback remains alive (that is, has not been garbage collected).
        /// </summary>
        /// <param name="scope">The callback that returns the scope.</param>
        void InScope( Func<IContext, object> scope );

        /// <summary>
        /// Indicates that only a single instance of the binding should be created, and then
        /// should be re-used for all subsequent requests.
        /// </summary>
        void InSingletonScope();

        /// <summary>
        /// Indicates that instances activated via the binding should not be re-used, nor have
        /// their lifecycle managed by Ninject.
        /// </summary>
        void InTransientScope();

        /// <summary>
        /// Indicates that instances activated via the binding should be re-used within the same thread.
        /// </summary>
        void InThreadScope();

#if !NO_WEB
        /// <summary>
        /// Indicates that instances activated via the binding should be re-used within the same
        /// HTTP request.
        /// </summary>
        void InRequestScope();
#endif
    }
}