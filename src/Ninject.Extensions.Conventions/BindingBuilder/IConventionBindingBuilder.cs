// -------------------------------------------------------------------------------------------------
// <copyright file="IConventionBindingBuilder.cs" company="Ninject Project Contributors">
//   Copyright (c) 2009-2017 Ninject Project Contributors
//   Licensed under the Apache License, Version 2.0.
// </copyright>
// -------------------------------------------------------------------------------------------------

namespace Ninject.Extensions.Conventions.BindingBuilder
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    using Ninject.Extensions.Conventions.BindingGenerators;
    using Ninject.Extensions.Conventions.Syntax;

    /// <summary>
    /// Builder for conventions configurations.
    /// </summary>
    public interface IConventionBindingBuilder
    {
        /// <summary>
        /// Defines that types are selectes from the specified assemblies.
        /// </summary>
        /// <param name="assemblies">The assemblies from which the types are selected.</param>
        void SelectAllTypesFrom(IEnumerable<Assembly> assemblies);

        /// <summary>
        /// Selects the types matching the specified filter from the current selected types.
        /// </summary>
        /// <param name="filter">The filter used to select the types.</param>
        void Where(Func<Type, bool> filter);

        /// <summary>
        /// Includes the specified types.
        /// </summary>
        /// <param name="includedTypes">The types to be included.</param>
        void Including(IEnumerable<Type> includedTypes);

        /// <summary>
        /// Excludes the specified types.
        /// </summary>
        /// <param name="excludedTypes">The types to be excluded.</param>
        void Excluding(IEnumerable<Type> excludedTypes);

        /// <summary>
        /// Creates the bindings using the specified generator.
        /// </summary>
        /// <param name="generator">The generator to use to create the bindings.</param>
        void BindWith(IBindingGenerator generator);

        /// <summary>
        /// Configures the bindings using the specified configuration.
        /// </summary>
        /// <param name="configuration">The configuration that is applies to the bindings.</param>
        void Configure(ConfigurationAction configuration);

        /// <summary>
        /// Configures the bindings using the specified configuration.
        /// </summary>
        /// <param name="configuration">The configuration that is applies to the bindings.</param>
        void Configure(ConfigurationActionWithService configuration);

#if !NO_SKIP_VISIBILITY
        /// <summary>
        /// Includes none public types.
        /// </summary>
        void IncludingNonePublicTypes();
#endif

        /// <summary>
        /// Configures the binding of the specified type using the specified configuration.
        /// </summary>
        /// <typeparam name="T">The type to be configured.</typeparam>
        /// <param name="configuration">The configuration that is applies to the bindings.</param>
        void ConfigureFor<T>(ConfigurationAction configuration);

        /// <summary>
        /// Configures the binding of the specified type using the specified configuration.
        /// </summary>
        /// <typeparam name="T">The type to be configured.</typeparam>
        /// <param name="configuration">The configuration that is applies to the bindings.</param>
        void ConfigureFor<T>(ConfigurationActionWithService configuration);
    }
}