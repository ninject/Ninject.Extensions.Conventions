// -------------------------------------------------------------------------------------------------
// <copyright file="IConfigureForSyntax.cs" company="Ninject Project Contributors">
//   Copyright (c) 2009-2017 Ninject Project Contributors
//   Licensed under the Apache License, Version 2.0.
// </copyright>
// -------------------------------------------------------------------------------------------------

namespace Ninject.Extensions.Conventions.Syntax
{
    using System;

    using Ninject.Syntax;

    /// <summary>
    /// The action to perform to configure the bindings.
    /// </summary>
    /// <param name="syntax">The binding syntax.</param>
    public delegate void ConfigurationAction(IBindingWhenInNamedWithOrOnSyntax<object> syntax);

    /// <summary>
    /// The action to perform to confugure the bindings.
    /// </summary>
    /// <param name="syntax">The binding syntax.</param>
    /// <param name="serviceType">The type of the service.</param>
    public delegate void ConfigurationActionWithService(IBindingWhenInNamedWithOrOnSyntax<object> syntax, Type serviceType);

    /// <summary>
    /// The syntax to configure special instances
    /// </summary>
    public interface IConfigureForSyntax : IFluentSyntax
    {
        /// <summary>
        /// Configures the bindings with the specified configuration.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <param name="configuration">The configuration.</param>
        /// <returns>The fluent syntax.</returns>
        IConfigureForSyntax ConfigureFor<TService>(ConfigurationAction configuration);

        /// <summary>
        /// Configures the bindings with the specified configuration.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <param name="configuration">The configuration.</param>
        /// <returns>The fluent syntax.</returns>
        IConfigureForSyntax ConfigureFor<TService>(ConfigurationActionWithService configuration);
    }
}