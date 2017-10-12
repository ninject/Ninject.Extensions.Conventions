// -------------------------------------------------------------------------------------------------
// <copyright file="ConventionSyntax.Configure.cs" company="Ninject Project Contributors">
//   Copyright (c) 2009-2017 Ninject Project Contributors
//   Licensed under the Apache License, Version 2.0.
// </copyright>
// -------------------------------------------------------------------------------------------------

namespace Ninject.Extensions.Conventions.BindingBuilder
{
    using Ninject.Extensions.Conventions.Syntax;

    /// <summary>
    /// The syntax to configure the conventions
    /// </summary>
    public partial class ConventionSyntax : IConfigureSyntax
    {
        /// <summary>
        /// Configures the bindings with the specified configuration.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <param name="configuration">The configuration.</param>
        /// <returns>The fluent syntax.</returns>
        public IConfigureForSyntax ConfigureFor<TService>(ConfigurationAction configuration)
        {
            this.bindingBuilder.ConfigureFor<TService>(configuration);
            return this;
        }

        /// <summary>
        /// Configures the bindings with the specified configuration.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <param name="configuration">The configuration.</param>
        /// <returns>The fluent syntax.</returns>
        public IConfigureForSyntax ConfigureFor<TService>(ConfigurationActionWithService configuration)
        {
            this.bindingBuilder.ConfigureFor<TService>(configuration);
            return this;
        }

        /// <summary>
        /// Configures the bindings with the specified configuration.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <returns>The fluent syntax.</returns>
        public IConfigureForSyntax Configure(ConfigurationAction configuration)
        {
            this.bindingBuilder.Configure(configuration);
            return this;
        }

        /// <summary>
        /// Configures the bindings with the specified configuration.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <returns>The fluent syntax.</returns>
        public IConfigureForSyntax Configure(ConfigurationActionWithService configuration)
        {
            this.bindingBuilder.Configure(configuration);
            return this;
        }
    }
}