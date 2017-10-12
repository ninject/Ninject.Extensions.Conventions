// -------------------------------------------------------------------------------------------------
// <copyright file="DefaultInterfacesBindingGenerator.cs" company="Ninject Project Contributors">
//   Copyright (c) 2009-2017 Ninject Project Contributors
//   Licensed under the Apache License, Version 2.0.
// </copyright>
// -------------------------------------------------------------------------------------------------

namespace Ninject.Extensions.Conventions.BindingGenerators
{
    using System;

    using Ninject.Extensions.Conventions.BindingBuilder;

    /// <summary>
    /// Binds all interfaces
    /// </summary>
    public class DefaultInterfacesBindingGenerator : AbstractInterfaceBindingGenerator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultInterfacesBindingGenerator"/> class.
        /// </summary>
        /// <param name="bindableTypeSelector">The bindable type selector.</param>
        /// <param name="bindingCreator">Creator for multiple bindings for one type.</param>
        public DefaultInterfacesBindingGenerator(IBindableTypeSelector bindableTypeSelector, IBindingCreator bindingCreator)
            : base(bindableTypeSelector, bindingCreator, ImplementationTypeContainsinterfaceName)
        {
        }

        /// <summary>
        /// Checks if the implementation type contains the name of the interface.
        /// </summary>
        /// <param name="implementationType">The type of the implementation.</param>
        /// <param name="interfaceType">The type of the interface.</param>
        /// <returns>True if the name of the interface is contained in the name of the implementation type.</returns>
        private static bool ImplementationTypeContainsinterfaceName(Type implementationType, Type interfaceType)
        {
            var interfaceName = GetNameWithoutGenericPart(interfaceType);
            var implementationName = GetNameWithoutGenericPart(implementationType);

            return implementationName.Contains(interfaceName.Substring(1));
        }
    }
}