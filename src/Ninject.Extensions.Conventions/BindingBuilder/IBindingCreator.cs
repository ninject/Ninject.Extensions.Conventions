// -------------------------------------------------------------------------------------------------
// <copyright file="IBindingCreator.cs" company="Ninject Project Contributors">
//   Copyright (c) 2009-2017 Ninject Project Contributors
//   Licensed under the Apache License, Version 2.0.
// </copyright>
// -------------------------------------------------------------------------------------------------

namespace Ninject.Extensions.Conventions.BindingBuilder
{
    using System;
    using System.Collections.Generic;

    using Ninject.Syntax;

    /// <summary>
    /// Creates bindings.
    /// </summary>
    public interface IBindingCreator
    {
        /// <summary>
        /// Creates the bindings for the specified services.
        /// </summary>
        /// <param name="bindingRoot">The binding root.</param>
        /// <param name="serviceTypes">The service types.</param>
        /// <param name="implementationType">The implementation type.</param>
        /// <returns>The syntax of the created bindings.</returns>
        IEnumerable<IBindingWhenInNamedWithOrOnSyntax<object>> CreateBindings(
            IBindingRoot bindingRoot,
            IEnumerable<Type> serviceTypes,
            Type implementationType);
    }
}