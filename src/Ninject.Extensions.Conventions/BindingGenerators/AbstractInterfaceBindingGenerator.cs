// -------------------------------------------------------------------------------------------------
// <copyright file="AbstractInterfaceBindingGenerator.cs" company="Ninject Project Contributors">
//   Copyright (c) 2009-2017 Ninject Project Contributors
//   Licensed under the Apache License, Version 2.0.
// </copyright>
// -------------------------------------------------------------------------------------------------

namespace Ninject.Extensions.Conventions.BindingGenerators
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Ninject.Extensions.Conventions.BindingBuilder;
    using Ninject.Syntax;

    /// <summary>
    /// An abstract implementation of a binding generator that binds the interfaces of a type to the type.
    /// </summary>
    public abstract class AbstractInterfaceBindingGenerator : IBindingGenerator
    {
        /// <summary>
        /// Gets the types that are bindable to a given type.
        /// </summary>
        private readonly IBindableTypeSelector bindableTypeSelector;

        /// <summary>
        /// Creator for multiple bindings for one type.
        /// </summary>
        private readonly IBindingCreator bindingCreator;

        /// <summary>
        /// The filter used to decide if a binding for the interface type is created.
        /// </summary>
        private readonly Func<Type, Type, bool> filter;

        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractInterfaceBindingGenerator"/> class.
        /// </summary>
        /// <param name="bindableTypeSelector">The bindable type selector.</param>
        /// <param name="bindingCreator">Creator for multiple bindings for one type.</param>
        /// <param name="filter">The filter used to decide if a binding for the interface type is created.</param>
        protected AbstractInterfaceBindingGenerator(
            IBindableTypeSelector bindableTypeSelector,
            IBindingCreator bindingCreator,
            Func<Type, Type, bool> filter)
        {
            this.bindableTypeSelector = bindableTypeSelector;
            this.bindingCreator = bindingCreator;
            this.filter = filter;
        }

        /// <summary>
        /// Creates the bindings for a type.
        /// </summary>
        /// <param name="type">The type for which the bindings are created.</param>
        /// <param name="bindingRoot">The binding root that is used to create the bindings.</param>
        /// <returns>
        /// The syntaxes for the created bindings to configure more options.
        /// </returns>
        public IEnumerable<IBindingWhenInNamedWithOrOnSyntax<object>> CreateBindings(Type type, IBindingRoot bindingRoot)
        {
            var interfaces = this.bindableTypeSelector
                .GetBindableInterfaces(type)
                .Where(interfaceType => this.filter(type, interfaceType));
            return this.bindingCreator.CreateBindings(bindingRoot, interfaces, type);
        }

        /// <summary>
        /// Gets the name of a type without the generic part.
        /// </summary>
        /// <param name="type">The type for which the name is returned.</param>
        /// <returns>The name of the given type without the generic type post fix in case of a generic type.</returns>
        protected static string GetNameWithoutGenericPart(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            var name = type.Name;
            if (!type.IsGenericType)
            {
                return name;
            }

            var genericMarkerPosition = name.IndexOf("`", StringComparison.Ordinal);
            if (genericMarkerPosition > -1)
            {
                return name.Substring(0, genericMarkerPosition);
            }
            else
            {
                return name;
            }
        }
    }
}