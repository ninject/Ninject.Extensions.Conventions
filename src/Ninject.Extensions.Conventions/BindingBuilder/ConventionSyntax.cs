//-------------------------------------------------------------------------------
// <copyright file="ConventionSyntax.cs" company="Ninject Project Contributors">
//   Copyright (c) 2009-2011 Ninject Project Contributors
//   Authors: Remo Gloor (remo.gloor@gmail.com)
//           
//   Dual-licensed under the Apache License, Version 2.0, and the Microsoft Public License (Ms-PL).
//   you may not use this file except in compliance with one of the Licenses.
//   You may obtain a copy of the License at
//
//       http://www.apache.org/licenses/LICENSE-2.0
//   or
//       http://www.microsoft.com/opensource/licenses.mspx
//
//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
// </copyright>
//-------------------------------------------------------------------------------

namespace Ninject.Extensions.Conventions.BindingBuilder
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text.RegularExpressions;

    using Ninject.Extensions.Conventions.BindingGenerators;
    using Ninject.Extensions.Conventions.Syntax;
#if !SILVERLIGHT_20 && !WINDOWS_PHONE && !NETCF_35 && !MONO
    using Ninject.Extensions.Factory;
#endif

    /// <summary>
    /// The syntax to configure the conventions
    /// </summary>
    public class ConventionSyntax : IFromWhereExcludeIncludeBindSyntax, IIncludingNonePublicTypesSelectSyntax, IConfigureSyntax
    {
        /// <summary>
        /// Builder for conventions
        /// </summary>
        private readonly IConventionBindingBuilder bindingBuilder;

#if !NO_ASSEMBLY_SCANNING
        /// <summary>
        /// Finder for assemblies.
        /// </summary>
        private readonly IAssemblyFinder assemblyFinder;
#endif

        /// <summary>
        /// Filter for types.
        /// </summary>
        private readonly ITypeFilter typeFilter;

        /// <summary>
        /// Factory to create binding generators.
        /// </summary>
        private readonly IBindingGeneratorFactory bindingGeneratorFactory;

#if !NO_ASSEMBLY_SCANNING
        /// <summary>
        /// Initializes a new instance of the <see cref="ConventionSyntax"/> class.
        /// </summary>
        /// <param name="bindingBuilder">The binding builder.</param>
        /// <param name="assemblyFinder">The assembly finder.</param>
        /// <param name="typeFilter">The type filter.</param>
        /// <param name="bindingGeneratorFactory">The binding generator factory.</param>
        public ConventionSyntax(
            IConventionBindingBuilder bindingBuilder, 
            IAssemblyFinder assemblyFinder,
            ITypeFilter typeFilter,
            IBindingGeneratorFactory bindingGeneratorFactory)
        {
            this.bindingBuilder = bindingBuilder;
            this.assemblyFinder = assemblyFinder;
            this.typeFilter = typeFilter;
            this.bindingGeneratorFactory = bindingGeneratorFactory;
        }
#else
        /// <summary>
        /// Initializes a new instance of the <see cref="ConventionSyntax"/> class.
        /// </summary>
        /// <param name="bindingBuilder">The binding builder.</param>
        /// <param name="typeFilter">The type filter.</param>
        /// <param name="bindingGeneratorFactory">The binding generator factory.</param>
        public ConventionSyntax(
            IConventionBindingBuilder bindingBuilder, 
            ITypeFilter typeFilter,
            IBindingGeneratorFactory bindingGeneratorFactory)
        {
            this.bindingBuilder = bindingBuilder;
            this.typeFilter = typeFilter;
            this.bindingGeneratorFactory = bindingGeneratorFactory;
        }
#endif

        #region Assembly Selection
        /// <summary>
        /// Scans the specified assemblies.
        /// </summary>
        /// <param name="assemblies">The assemblies.</param>
        /// <returns>The fluent syntax.</returns>
        public IIncludingNonePublicTypesSelectSyntax From(IEnumerable<Assembly> assemblies)
        {
            this.bindingBuilder.SelectAllTypesFrom(assemblies);
            return this;
        }

        /// <summary>
        /// Scans the specified assemblies.
        /// </summary>
        /// <param name="assemblies">The assemblies.</param>
        /// <returns>The fluent syntax.</returns>
        public IIncludingNonePublicTypesSelectSyntax From(params Assembly[] assemblies)
        {
            return this.From(assemblies.AsEnumerable());
        }

        /// <summary>
        /// Scans the calling assembly.
        /// </summary>
        /// <returns>The fluent syntax.</returns>
        public IIncludingNonePublicTypesSelectSyntax FromThisAssembly()
        {
            return this.From(Assembly.GetCallingAssembly());
        }

        /// <summary>
        /// Scans the assembly containing the specified type.
        /// </summary>
        /// <typeparam name="T">The type that specifies the assembly.</typeparam>
        /// <returns>The fluent syntax.</returns>
        public IIncludingNonePublicTypesSelectSyntax FromAssemblyContaining<T>()
        {
            return this.FromAssemblyContaining(typeof(T));
        }

        /// <summary>
        /// Scans the assembly containing the specified type..
        /// </summary>
        /// <param name="types">The types that specify the assemblies.</param>
        /// <returns>The fluent syntax.</returns>
        public IIncludingNonePublicTypesSelectSyntax FromAssemblyContaining(params Type[] types)
        {
            return this.FromAssemblyContaining(types.AsEnumerable());
        }

        /// <summary>
        /// Scans the assembly containing the specified type..
        /// </summary>
        /// <param name="types">The types that specify the assemblies.</param>
        /// <returns>The fluent syntax.</returns>
        public IIncludingNonePublicTypesSelectSyntax FromAssemblyContaining(IEnumerable<Type> types)
        {
            return this.From(types.Select(t => t.Assembly).Distinct());
        }

#if !NO_ASSEMBLY_SCANNING
        /// <summary>
        /// Scans the specified assemblies.
        /// </summary>
        /// <param name="assemblies">The name of the assemblies to be scanned.</param>
        /// <returns>The fluent syntax.</returns>
        public IIncludingNonePublicTypesSelectSyntax From(params string[] assemblies)
        {
            return this.From(assemblies.AsEnumerable());
        }

        /// <summary>
        /// Scans the specified assemblies.
        /// </summary>
        /// <param name="assemblies">The name of the assemblies to be scanned.</param>
        /// <returns>The fluent syntax.</returns>
        public IIncludingNonePublicTypesSelectSyntax From(IEnumerable<string> assemblies)
        {
            return From(assemblies, filter => true);
        }

        /// <summary>
        /// Scans the specified assemblies.
        /// </summary>
        /// <param name="assemblies">The name of the assemblies to be scanned.</param>
        /// <param name="filter">The filter for filtering the assemblies.</param>
        /// <returns>The fluent syntax.</returns>
        public IIncludingNonePublicTypesSelectSyntax From(IEnumerable<string> assemblies, Predicate<Assembly> filter)
        {
            return this.From(this.assemblyFinder.FindAssemblies(assemblies, filter));
        }

        /// <summary>
        /// Scans the assemblies in the given path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>The fluent syntax.</returns>
        public IIncludingNonePublicTypesSelectSyntax FromAssembliesInPath(string path)
        {
            return this.FromAssembliesInPath(path, filter => true);
        }

        /// <summary>
        /// Scans the assemblies in the given path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="filter">The filter used to filter the assemblies.</param>
        /// <returns>The fluent syntax.</returns>
        public IIncludingNonePublicTypesSelectSyntax FromAssembliesInPath(string path, Predicate<Assembly> filter)
        {
            return this.From(this.assemblyFinder.FindAssembliesInPath(path), filter);
        }

        /// <summary>
        /// Scans the assemblies matching the given pattern.
        /// </summary>
        /// <param name="patterns">The patterns to match the assemblies.</param>
        /// <returns>The fluent syntax.</returns>
        public IIncludingNonePublicTypesSelectSyntax FromAssembliesMatching(params string[] patterns)
        {
            return this.FromAssembliesMatching(patterns.AsEnumerable());
        }

        /// <summary>
        /// Scans the assemblies matching the given pattern.
        /// </summary>
        /// <param name="patterns">The patterns to match the assemblies.</param>
        /// <returns>The fluent syntax.</returns>
        public IIncludingNonePublicTypesSelectSyntax FromAssembliesMatching(IEnumerable<string> patterns)
        {
            return this.From(this.assemblyFinder.FindAssembliesMatching(patterns));
        }
#endif
        #endregion

        #region Select Types
        /// <summary>
        /// Selects all types matching the given filter.
        /// </summary>
        /// <param name="filter">The filter</param>
        /// <returns>The fluent syntax</returns>
        public IFromWhereExcludeIncludeBindSyntax Select(Func<Type, bool> filter)
        {
            this.bindingBuilder.Where(filter);
            return this;
        }

        /// <summary>
        /// Selects all types
        /// </summary>
        /// <returns>The fluent syntax</returns>
        public IFromWhereExcludeIncludeBindSyntax SelectAllTypes()
        {
            return this.Select(t => true);
        }

        /// <summary>
        /// Selects the types in the specified namespaces.
        /// </summary>
        /// <param name="namespaces">The namespaces from which the types are selected.</param>
        /// <returns>The fluent syntax</returns>
        public IFromWhereExcludeIncludeBindSyntax SelectTypesInNamespaces(IEnumerable<string> namespaces)
        {
            return this.Select(t => namespaces.Any(ns => this.typeFilter.IsTypeInNamespace(t, ns)));
        }

        /// <summary>
        /// Selects the types in the specified namespaces.
        /// </summary>
        /// <param name="namespaces">The namespaces from which the types are selected.</param>
        /// <returns>The fluent syntax</returns>
        public IFromWhereExcludeIncludeBindSyntax SelectTypesInNamespaces(params string[] namespaces)
        {
            return this.SelectTypesInNamespaces(namespaces.AsEnumerable());
        }

        /// <summary>
        /// Selects the types in the same namespaces as the given types.
        /// </summary>
        /// <param name="types">The types defining the namespaces.</param>
        /// <returns>The fluent syntax</returns>
        public IFromWhereExcludeIncludeBindSyntax SelectTypesInNamespaceOf(params Type[] types)
        {
            return this.SelectTypesInNamespaces(types.Select(t => t.Namespace));
        }

        /// <summary>
        /// Selects the types in the same namespace as the given type.
        /// </summary>
        /// <typeparam name="T">The type defining the namespace.</typeparam>
        /// <returns>The fluent syntax</returns>
        public IFromWhereExcludeIncludeBindSyntax SelectTypesInNamespaceOf<T>()
        {
            return this.SelectTypesInNamespaceOf(typeof(T));
        }

        /// <summary>
        /// Selects all types not in the given namespaces.
        /// </summary>
        /// <param name="namespaces">The namespaces from which the types are not selected.</param>
        /// <returns>The fluent syntax</returns>
        public IFromWhereExcludeIncludeBindSyntax SelectTypesNotInNamespaces(IEnumerable<string> namespaces)
        {
            return this.Select(t => namespaces.All(ns => !this.typeFilter.IsTypeInNamespace(t, ns)));
        }

        /// <summary>
        /// Selects all types not in the given namespaces.
        /// </summary>
        /// <param name="namespaces">The namespaces from which the types are not selected.</param>
        /// <returns>The fluent syntax</returns>
        public IFromWhereExcludeIncludeBindSyntax SelectTypesNotInNamespaces(params string[] namespaces)
        {
            return this.SelectTypesNotInNamespaces(namespaces.AsEnumerable());
        }

        /// <summary>
        /// Selects all types not in same namespaces as the given types.
        /// </summary>
        /// <param name="types">The types defining the namespace.</param>
        /// <returns>The fluent syntax</returns>
        public IFromWhereExcludeIncludeBindSyntax SelectTypesNotInNamespaceOf(params Type[] types)
        {
            return this.SelectTypesNotInNamespaces(types.Select(t => t.Namespace));
        }

        /// <summary>
        /// Selects all types not in same namespaces as the given type.
        /// </summary>
        /// <typeparam name="T">The type defining the namespace.</typeparam>
        /// <returns>The fluent syntax</returns>
        public IFromWhereExcludeIncludeBindSyntax SelectTypesNotInNamespaceOf<T>()
        {
            return this.SelectTypesNotInNamespaceOf(typeof(T));
        }

        /// <summary>
        /// Selects the types inherited from any of the given types.
        /// </summary>
        /// <param name="types">The ancestor types.</param>
        /// <returns>The fluent syntax</returns>
        public IFromWhereExcludeIncludeBindSyntax SelectTypesInheritedFromAny(params Type[] types)
        {
            return this.SelectTypesInheritedFromAny(types.AsEnumerable());
        }

        /// <summary>
        /// Selects the types inherited from any of the given types.
        /// </summary>
        /// <param name="types">The ancestor types.</param>
        /// <returns>The fluent syntax</returns>
        public IFromWhereExcludeIncludeBindSyntax SelectTypesInheritedFromAny(IEnumerable<Type> types)
        {
            return this.Select(t => this.typeFilter.IsTypeInheritedFromAny(t, types));
        }

        /// <summary>
        /// Selects the types inherited from the given types.
        /// </summary>
        /// <param name="type">The ancestor type.</param>
        /// <returns>The fluent syntax</returns>
        public IFromWhereExcludeIncludeBindSyntax SelectTypesInheritedFrom(Type type)
        {
            return this.SelectTypesInheritedFromAny(type);
        }

        /// <summary>
        /// Selects the types inherited from the given types.
        /// </summary>
        /// <typeparam name="T">The ancestor type.</typeparam>
        /// <returns>The fluent syntax</returns>
        public IFromWhereExcludeIncludeBindSyntax SelectTypesInheritedFrom<T>()
        {
            return this.SelectTypesInheritedFrom(typeof(T));
        }

        /// <summary>
        /// Selects the types with thw specified attribute.
        /// </summary>
        /// <typeparam name="T">The type of the attribute</typeparam>
        /// <returns>The fluent syntax</returns>
        public IFromWhereExcludeIncludeBindSyntax SelectTypesWithAttribute<T>() where T : Attribute
        {
            return this.SelectTypesWithAttribute(typeof(T));
        }

        /// <summary>
        /// Selects the types that matches the specified attribute predicate.
        /// </summary>
        /// <typeparam name="T">The type of the attribute</typeparam>
        /// <param name="predicate">A function to test if an attribute predicate.</param>
        /// <returns>The fluent syntax</returns>
        public IFromWhereExcludeIncludeBindSyntax SelectTypesWithAttribute<T>(Func<T, bool> predicate) where T : Attribute
        {
            return this.Select(t => this.typeFilter.HasAttribute(t, predicate));
        }

        /// <summary>
        /// Selects the types with thw specified attribute.
        /// </summary>
        /// <param name="attributeType">The type of the attribute.</param>
        /// <returns>The fluent syntax</returns>
        public IFromWhereExcludeIncludeBindSyntax SelectTypesWithAttribute(Type attributeType)
        {
            return this.Select(t => this.typeFilter.HasAttribute(t, attributeType));
        }        
        
        /// <summary>
        /// Selects the types without the specified attribute.
        /// </summary>
        /// <typeparam name="T">The type of the attribute</typeparam>
        /// <returns>The fluent syntax</returns>
        public IFromWhereExcludeIncludeBindSyntax SelectTypesWithoutAttribute<T>() where T : Attribute
        {
            return this.SelectTypesWithoutAttribute(typeof(T));
        }

        /// <summary>
        /// Selects the types that do not match the specified attribute predicate.
        /// </summary>
        /// <typeparam name="T">The type of the attribute</typeparam>
        /// <param name="predicate">A function to test if an attribute matches.</param>
        /// <returns>The fluent syntax</returns>
        public IFromWhereExcludeIncludeBindSyntax SelectTypesWithoutAttribute<T>(Func<T, bool> predicate) where T : Attribute
        {
            return this.Select(t => !this.typeFilter.HasAttribute(t, predicate));
        }

        /// <summary>
        /// Selects the types with thw specified attribute.
        /// </summary>
        /// <param name="attributeType">The type of the attribute.</param>
        /// <returns>The fluent syntax</returns>
        public IFromWhereExcludeIncludeBindSyntax SelectTypesWithoutAttribute(Type attributeType)
        {
            return this.Select(t => !this.typeFilter.HasAttribute(t, attributeType));
        }

        #endregion

        #region Type Filtering
        /// <summary>
        /// Filters the selected types.
        /// </summary>
        /// <param name="filter">The filter used to filter the selected types.</param>
        /// <returns>The fluent syntax</returns>
        public IFromExcludeIncludeBindSyntax Where(Func<Type, bool> filter)
        {
            this.bindingBuilder.Where(filter);
            return this;
        }

        /// <summary>
        /// Includes the given type
        /// </summary>
        /// <typeparam name="T">The type to be included</typeparam>
        /// <returns>The fluent syntax</returns>
        public IExcludeIncludeBindSyntax Including<T>()
        {
            return this.Including(typeof(T));
        }

        /// <summary>
        /// Includes the given types
        /// </summary>
        /// <param name="types">The types to be included.</param>
        /// <returns>The fluent syntax</returns>
        public IExcludeIncludeBindSyntax Including(params Type[] types)
        {
            return this.Including(types.AsEnumerable());
        }

        /// <summary>
        /// Includes the given types
        /// </summary>
        /// <param name="types">The types to be included.</param>
        /// <returns>The fluent syntax</returns>
        public IExcludeIncludeBindSyntax Including(IEnumerable<Type> types)
        {
            this.bindingBuilder.Including(types);
            return this;
        }

        /// <summary>
        /// Excludes the given type
        /// </summary>
        /// <typeparam name="T">The type to be excluded</typeparam>
        /// <returns>The fluent syntax</returns>
        public IExcludeIncludeBindSyntax Excluding<T>()
        {
            return this.Excluding(typeof(T));
        }

        /// <summary>
        /// Excludes the given types
        /// </summary>
        /// <param name="types">The types to be excluded.</param>
        /// <returns>The fluent syntax</returns>
        public IExcludeIncludeBindSyntax Excluding(params Type[] types)
        {
            return this.Excluding(types.AsEnumerable());
        }

        /// <summary>
        /// Excludes the given types
        /// </summary>
        /// <param name="types">The types to be excluded.</param>
        /// <returns>The fluent syntax</returns>
        public IExcludeIncludeBindSyntax Excluding(IEnumerable<Type> types)
        {
            this.bindingBuilder.Excluding(types);
            return this;
        }
        #endregion

        #region Binding
        /// <summary>
        /// Bind using a custom binding generator.
        /// </summary>
        /// <typeparam name="T">The type of the binding generator</typeparam>
        /// <returns>The fluent syntax</returns>
        public IConfigureSyntax BindWith<T>() where T : IBindingGenerator, new()
        {
            return this.BindWith(new T());
        }

        /// <summary>
        /// Bind using a custom binding generator.
        /// </summary>
        /// <param name="generator">The generator used to create the bindings.</param>
        /// <returns>The fluent syntax</returns>
        public IConfigureSyntax BindWith(IBindingGenerator generator)
        {
            this.bindingBuilder.BindWith(generator);
            return this;
        }

        /// <summary>
        /// Binds all interfaces of the given types to the type.
        /// </summary>
        /// <returns>The fluent syntax</returns>
        public IConfigureSyntax BindToAllInterfaces()
        {
            return this.BindWith(this.bindingGeneratorFactory.CreateAllInterfacesBindingGenerator());
        }

        /// <summary>
        /// Binds the base type of the given types to the type.
        /// </summary>
        /// <returns>The fluent syntax</returns>
        public IConfigureSyntax BindToBase()
        {
            return this.BindWith(this.bindingGeneratorFactory.CreateBaseBindingGenerator());
        }

        /// <summary>
        /// Binds the default interface of the given types to the type.
        /// e.g. Foo : IFoo
        /// </summary>
        /// <returns>The fluent syntax</returns>
        public IConfigureSyntax BindToDefaultInterface()
        {
            return this.BindWith(this.bindingGeneratorFactory.CreateDefaultInterfaceBindingGenerator());
        }

        /// <summary>
        /// Binds the default interface of the given types to the type.
        /// e.g. MyFoo matches IFoo, and SuperCrazyFoo matches ICrazyFoo and IFoo
        /// </summary>
        /// <returns>The fluent syntax</returns>
        public IConfigureSyntax BindToDefaultInterfaces()
        {
            return this.BindWith(this.bindingGeneratorFactory.CreateDefaultInterfacesBindingGenerator());
        }

        /// <summary>
        /// Expects that the given type has a single interface.
        /// In this case the interface is bound to the type.
        /// </summary>
        /// <returns>The fluent syntax</returns>
        public IConfigureSyntax BindToSingleInterface()
        {
            return this.BindWith(this.bindingGeneratorFactory.CreateSingleInterfaceBindingGenerator());
        }

        /// <summary>
        /// Binds the type to itself.
        /// </summary>
        /// <returns>The fluent syntax</returns>
        public IConfigureSyntax BindToSelf()
        {
            return this.BindWith(this.bindingGeneratorFactory.CreateSelfBindingGenerator());
        }

        /// <summary>
        /// Binds the selected interfaces to the type.
        /// </summary>
        /// <param name="selector">The selector of the interfaces.</param>
        /// <returns>The fluent syntax</returns>
        public IConfigureSyntax BindToSelection(ServiceSelector selector)
        {
            return this.BindWith(this.bindingGeneratorFactory.CreateSelectorBindingGenerator(selector));
        }

        /// <summary>
        /// Bind the type to its interfaces matching the given regular expression.
        /// </summary>
        /// <param name="pattern">The regular expression.</param>
        /// <returns>The fluent syntax</returns>
        public IConfigureSyntax BindToRegex(string pattern)
        {
            return this.BindWith(this.bindingGeneratorFactory.CreateRegexBindingGenerator(pattern));
        }

        /// <summary>
        /// Bind the type to its interfaces matching the given regular expression.
        /// </summary>
        /// <param name="pattern">The regular expression.</param>
        /// <param name="options">The regex options.</param>
        /// <returns>The fluent syntax</returns>
        public IConfigureSyntax BindToRegex(string pattern, RegexOptions options)
        {
            return this.BindWith(this.bindingGeneratorFactory.CreateRegexBindingGenerator(pattern, options));
        }

#if !SILVERLIGHT_20 && !WINDOWS_PHONE && !NETCF_35 && !MONO
        /// <summary>
        /// Binds interfaces to factory implementations using the factory extension.
        /// </summary>
        /// <returns>The fluent syntax</returns>
        public IConfigureSyntax BindToFactory()
        {
            return this.BindWith(this.bindingGeneratorFactory.FactoryBindingGenerator(null));
        }

        /// <summary>
        /// Binds interfaces to factory implementations using the factory extension.
        /// </summary>
        /// <param name="instanceProvider">The instance provider.</param>
        /// <returns>The fluent syntax</returns>
        public IConfigureSyntax BindToFactory(Func<IInstanceProvider> instanceProvider)
        {
            return this.BindWith(this.bindingGeneratorFactory.FactoryBindingGenerator(instanceProvider));
        }
#endif

        /// <summary>
        /// Configures the bindings for the specified service with the specified configuration.
        /// </summary>
        /// <typeparam name="TService">The type of service that shall be configured.</typeparam>
        /// <param name="configuration">The configuration.</param>
        /// <returns>The fluent syntax</returns>
        public IConfigureForSyntax ConfigureFor<TService>(ConfigurationAction configuration)
        {
            this.bindingBuilder.ConfigureFor<TService>(configuration);
            return this;
        }

        /// <summary>
        /// Configures the bindings for the specified service with the specified configuration.
        /// </summary>
        /// <typeparam name="TService">The type of service that shall be configured.</typeparam>
        /// <param name="configuration">The configuration.</param>
        /// <returns>The fluent syntax</returns>
        public IConfigureForSyntax ConfigureFor<TService>(ConfigurationActionWithService configuration)
        {
            this.bindingBuilder.ConfigureFor<TService>(configuration);
            return this;
        }

        /// <summary>
        /// Configures the bindings with the specified configuration.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <returns>The fluent syntax</returns>
        public IConfigureForSyntax Configure(ConfigurationAction configuration)
        {
            this.bindingBuilder.Configure(configuration);
            return this;
        }

        /// <summary>
        /// Configures the bindings with the specified configuration.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <returns>The fluent syntax</returns>
        public IConfigureForSyntax Configure(ConfigurationActionWithService configuration)
        {
            this.bindingBuilder.Configure(configuration);
            return this;
        }
        #endregion

#if !NO_SKIP_VISIBILITY
        /// <summary>
        /// Specifies that none public types shall be included.
        /// </summary>
        /// <returns>The fluent syntax.</returns>
        public ISelectSyntax IncludingNonePublicTypes()
        {
            this.bindingBuilder.IncludingNonePublicTypes();
            return this;
        }
#endif
    }
}