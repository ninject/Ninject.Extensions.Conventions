// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IAssemblyScanner.cs" company="">
//   
// </copyright>
// <summary>
//   The i assembly scanner.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Ninject.Extensions.Conventions
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    using Ninject.Activation;

    /// <summary>
    /// The i assembly scanner.
    /// </summary>
    public interface IAssemblyScanner
    {
        /// <summary>
        /// Loads the specified assembly.
        /// </summary>
        /// <param name="assembly">
        /// The assembly.
        /// </param>
        void From(Assembly assembly);

        /// <summary>
        /// Uses the supplied types for processing subject to excludes and filters.
        /// </summary>
        /// <param name="types">
        /// </param>
        void From(IEnumerable<Type> types);

#if !NETCF && !SILVERLIGHT

        /// <summary>
        /// The from calling assembly.
        /// </summary>
        void FromCallingAssembly();
#endif

        // !NETCF

        /// <summary>
        /// Loads the specified assemblies.
        /// </summary>
        /// <param name="assemblies">
        /// The assemblies.
        /// </param>
        void From(IEnumerable<Assembly> assemblies);

#if !NO_ASSEMBLY_SCANNING

        /// <summary>
        /// Loads the specified assembly.
        /// </summary>
        /// <param name="assembly">
        /// The assembly.
        /// </param>
        void From(string assembly);

        /// <summary>
        /// Loads the specified assemblies.
        /// </summary>
        /// <param name="assemblies">
        /// The assemblies.
        /// </param>
        void From(IEnumerable<string> assemblies);

        /// <summary>
        /// The from.
        /// </summary>
        /// <param name="assemblies">
        /// </param>
        /// <param name="filter">
        /// </param>
        void From(IEnumerable<string> assemblies, Predicate<Assembly> filter);
#endif

        // !NO_ASSEMBLY_SCANNING

        /// <summary>
        /// Loads the assembly containing.
        /// </summary>
        /// <typeparam name="T">
        /// </typeparam>
        void FromAssemblyContaining<T>();

        /// <summary>
        /// Loads the assembly containing.
        /// </summary>
        /// <param name="type">
        /// The type.
        /// </param>
        void FromAssemblyContaining(Type type);

        /// <summary>
        /// Loads the assembly containing.
        /// </summary>
        /// <param name="types">
        /// The types.
        /// </param>
        void FromAssemblyContaining(IEnumerable<Type> types);

#if !NO_ASSEMBLY_SCANNING

        /// <summary>
        /// The from assemblies in path.
        /// </summary>
        /// <param name="path">
        /// </param>
        void FromAssembliesInPath(string path);

        /// <summary>
        /// The from assemblies in path.
        /// </summary>
        /// <param name="path">
        /// </param>
        /// <param name="assemblyFilter">
        /// </param>
        void FromAssembliesInPath(string path, Predicate<Assembly> assemblyFilter);

        /// <summary>
        /// The from assemblies matching.
        /// </summary>
        /// <param name="pattern">
        /// </param>
        void FromAssembliesMatching(string pattern);

        /// <summary>
        /// The from assemblies matching.
        /// </summary>
        /// <param name="patterns">
        /// </param>
        void FromAssembliesMatching(IEnumerable<string> patterns);
#endif

        /// <summary>
        /// Includes this instance.
        /// </summary>
        /// <typeparam name="T">
        /// </typeparam>
        void Including<T>();

        /// <summary>
        /// Includes the specified type.
        /// </summary>
        /// <param name="type">
        /// The type.
        /// </param>
        void Including(Type type);

        /// <summary>
        /// Includes the specified filters.
        /// </summary>
        /// <param name="filters">
        /// The filters.
        /// </param>
        void Including(IEnumerable<Type> filters);

        /// <summary>
        /// The where type is in namespace.
        /// </summary>
        /// <param name="nameSpace">
        /// </param>
        void WhereTypeIsInNamespace(string nameSpace);

        /// <summary>
        /// The where type is in namespace of.
        /// </summary>
        /// <typeparam name="T">
        /// </typeparam>
        void WhereTypeIsInNamespaceOf<T>();

        /// <summary>
        /// Excludes this instance.
        /// </summary>
        /// <typeparam name="T">
        /// </typeparam>
        void Excluding<T>();

        /// <summary>
        /// Excludes the specified type.
        /// </summary>
        /// <param name="type">
        /// The type.
        /// </param>
        void Excluding(Type type);

        /// <summary>
        /// Excludes the specified filters.
        /// </summary>
        /// <param name="filters">
        /// The filters.
        /// </param>
        void Excluding(IEnumerable<Type> filters);

        /// <summary>
        /// Excludes the specified filter.
        /// </summary>
        /// <param name="filter">
        /// The filter.
        /// </param>
        void Where(Predicate<Type> filter);

        /// <summary>
        /// The where type is not in namespace.
        /// </summary>
        /// <param name="nameSpace">
        /// </param>
        void WhereTypeIsNotInNamespace(string nameSpace);

        /// <summary>
        /// The where type is not in namespace of.
        /// </summary>
        /// <typeparam name="T">
        /// </typeparam>
        void WhereTypeIsNotInNamespaceOf<T>();

        /// <summary>
        /// The bind with.
        /// </summary>
        /// <typeparam name="T">
        /// </typeparam>
        void BindWith<T>() where T : IBindingGenerator, new();

        /// <summary>
        /// The bind with.
        /// </summary>
        /// <param name="generator">
        /// The generator.
        /// </param>
        void BindWith(IBindingGenerator generator);

        /// <summary>
        /// The bind with default conventions.
        /// </summary>
        void BindWithDefaultConventions();

#if !NO_ASSEMBLY_SCANNING

        /// <summary>
        /// The auto load modules.
        /// </summary>
        void AutoLoadModules();
#endif

        /// <summary>
        /// The where type inherits from.
        /// </summary>
        /// <typeparam name="T">
        /// </typeparam>
        void WhereTypeInheritsFrom<T>();

        /// <summary>
        /// The where type inherits from.
        /// </summary>
        /// <param name="type">
        /// </param>
        void WhereTypeInheritsFrom(Type type);

        /// <summary>
        /// The where type inherits from any.
        /// </summary>
        /// <param name="types">
        /// </param>
        void WhereTypeInheritsFromAny(IEnumerable<Type> types);

        /// <summary>
        /// Indicates that instances activated via the binding should be re-used as long as the object
        ///   returned by the provided callback remains alive (that is, has not been garbage collected).
        /// </summary>
        /// <param name="scope">
        /// The callback that returns the scope.
        /// </param>
        void InScope(Func<IContext, object> scope);

        /// <summary>
        /// Indicates that only a single instance of the binding should be created, and then
        ///   should be re-used for all subsequent requests.
        /// </summary>
        void InSingletonScope();

        /// <summary>
        /// Indicates that instances activated via the binding should not be re-used, nor have
        ///   their lifecycle managed by Ninject.
        /// </summary>
        void InTransientScope();

        /// <summary>
        /// Indicates that instances activated via the binding should be re-used within the same thread.
        /// </summary>
        void InThreadScope();

#if WEB
    
    // <summary>
    /// Indicates that instances activated via the binding should be re-used within the same
    /// HTTP request.
    /// </summary>
        void InRequestScope();
#endif
    }
}