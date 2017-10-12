// -------------------------------------------------------------------------------------------------
// <copyright file="IConfigureSyntax.cs" company="Ninject Project Contributors">
//   Copyright (c) 2009-2017 Ninject Project Contributors
//   Licensed under the Apache License, Version 2.0.
// </copyright>
// -------------------------------------------------------------------------------------------------

namespace Ninject.Extensions.Conventions.Syntax
{
    /// <summary>
    /// Syntax to configure the binding.
    /// </summary>
    public interface IConfigureSyntax : IConfigureForSyntax
    {
        /// <summary>
        /// Configures the bindings with the specified configuration.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <returns>The fluent syntax.</returns>
        IConfigureForSyntax Configure(ConfigurationAction configuration);

        /// <summary>
        /// Configures the bindings with the specified configuration.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <returns>The fluent syntax.</returns>
        IConfigureForSyntax Configure(ConfigurationActionWithService configuration);
    }
}