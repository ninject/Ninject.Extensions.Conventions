// -------------------------------------------------------------------------------------------------
// <copyright file="AllInterfacesBindingGenerator.cs" company="Ninject Project Contributors">
//   Copyright (c) 2009-2017 Ninject Project Contributors
//   Licensed under the Apache License, Version 2.0.
// </copyright>
// -------------------------------------------------------------------------------------------------

namespace Ninject.Extensions.Conventions.BindingGenerators
{
    using Ninject.Extensions.Conventions.BindingBuilder;

    /// <summary>
    /// Creates bindings for all interfaces of a type.
    /// </summary>
    public class AllInterfacesBindingGenerator : AbstractInterfaceBindingGenerator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AllInterfacesBindingGenerator"/> class.
        /// </summary>
        /// <param name="bindableTypeSelector">The bindable type selector.</param>
        /// <param name="bindingCreator">Creator for multiple bindings for one type.</param>
        public AllInterfacesBindingGenerator(IBindableTypeSelector bindableTypeSelector, IBindingCreator bindingCreator)
            : base(bindableTypeSelector, bindingCreator, (ts, ti) => true)
        {
        }
    }
}