// -------------------------------------------------------------------------------------------------
// <copyright file="DefaultInterfaceBindingGenerator.cs" company="Ninject Project Contributors">
//   Copyright (c) 2009-2017 Ninject Project Contributors
//   Licensed under the Apache License, Version 2.0.
// </copyright>
// -------------------------------------------------------------------------------------------------

namespace Ninject.Extensions.Conventions.BindingGenerators
{
    using System;

    using Ninject.Extensions.Conventions.BindingBuilder;

    /// <summary>
    /// Binds the type its interface with the same Name
    /// e.g. IFoo to Foo
    /// </summary>
    public class DefaultInterfaceBindingGenerator : AbstractInterfaceBindingGenerator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultInterfaceBindingGenerator"/> class.
        /// </summary>
        /// <param name="bindableTypeSelector">The bindable type selector.</param>
        /// <param name="bindingCreator">Creator for multiple bindings for one type.</param>
        public DefaultInterfaceBindingGenerator(IBindableTypeSelector bindableTypeSelector, IBindingCreator bindingCreator)
            : base(bindableTypeSelector, bindingCreator, HaveSameName)
        {
        }

        /// <summary>
        /// Checks if the given interface and implementation type have the same name.
        /// </summary>
        /// <param name="implementationType">The type of the implementation.</param>
        /// <param name="interfaceType">The type of the interface.</param>
        /// <returns>True if the names match.</returns>
        private static bool HaveSameName(Type implementationType, Type interfaceType)
        {
            var interfaceName = GetNameWithoutGenericPart(interfaceType);
            var implementationName = GetNameWithoutGenericPart(implementationType);

            return string.Equals(interfaceName, "I" + implementationName, StringComparison.OrdinalIgnoreCase);
        }
    }
}