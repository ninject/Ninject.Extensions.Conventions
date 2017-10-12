// -------------------------------------------------------------------------------------------------
// <copyright file="FactoryBindingGenerator.cs" company="Ninject Project Contributors">
//   Copyright (c) 2009-2017 Ninject Project Contributors
//   Licensed under the Apache License, Version 2.0.
// </copyright>
// -------------------------------------------------------------------------------------------------

#if !SILVERLIGHT_20 && !WINDOWS_PHONE && !NETCF_35
namespace Ninject.Extensions.Conventions.BindingGenerators
{
    using System;
    using System.Collections.Generic;

    using Ninject.Extensions.Factory;
    using Ninject.Syntax;

    /// <summary>
    /// Binds interfaces to factories using the factory extension.
    /// </summary>
    public class FactoryBindingGenerator : IBindingGenerator
    {
        private readonly Func<IInstanceProvider> instanceProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="FactoryBindingGenerator"/> class.
        /// </summary>
        /// <param name="instanceProvider">The instance provider.</param>
        public FactoryBindingGenerator(Func<IInstanceProvider> instanceProvider)
        {
            this.instanceProvider = instanceProvider;
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
            if (bindingRoot == null)
            {
                throw new ArgumentNullException("bindingRoot");
            }

            var binding = bindingRoot.Bind(type);
            var bindingConfiguration = this.instanceProvider == null
                ? binding.ToFactory(type)
                : binding.ToFactory(this.instanceProvider, type);
            return new[] { bindingConfiguration };
        }
    }
}
#endif