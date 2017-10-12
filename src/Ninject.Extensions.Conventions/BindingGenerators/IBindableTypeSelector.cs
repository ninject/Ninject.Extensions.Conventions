// -------------------------------------------------------------------------------------------------
// <copyright file="IBindableTypeSelector.cs" company="Ninject Project Contributors">
//   Copyright (c) 2009-2017 Ninject Project Contributors
//   Licensed under the Apache License, Version 2.0.
// </copyright>
// -------------------------------------------------------------------------------------------------

namespace Ninject.Extensions.Conventions.BindingGenerators
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Returns the interfaces and base types for which a binding to the given type can be created.
    /// </summary>
    public interface IBindableTypeSelector
    {
        /// <summary>
        /// Gets interfaces for which a binding can be created.
        /// e.g. an open generic type requires an open generic interface.
        /// </summary>
        /// <param name="type">The type for which the bindable interfaces shall be returned.</param>
        /// <returns>The interfaces for which a binding to the given type can be created.</returns>
        IEnumerable<Type> GetBindableInterfaces(Type type);

        /// <summary>
        /// Gets base types for which a binding can be created.
        /// e.g. an open generic type requires an open generic base type.
        /// </summary>
        /// <param name="type">The type for which the bindable base types shall be returned.</param>
        /// <returns>The base types for which a binding to the given type can be created.</returns>
        IEnumerable<Type> GetBindableBaseTypes(Type type);
    }
}