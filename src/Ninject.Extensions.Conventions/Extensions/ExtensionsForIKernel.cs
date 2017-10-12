// -------------------------------------------------------------------------------------------------
// <copyright file="ExtensionsForIKernel.cs" company="Ninject Project Contributors">
//   Copyright (c) 2009-2017 Ninject Project Contributors
//   Licensed under the Apache License, Version 2.0.
// </copyright>
// -------------------------------------------------------------------------------------------------

namespace Ninject.Extensions.Conventions
{
    using System;

    using Ninject.Extensions.Conventions.BindingBuilder;
    using Ninject.Extensions.Conventions.BindingGenerators;
    using Ninject.Extensions.Conventions.Syntax;
    using Ninject.Infrastructure;
    using Ninject.Modules;
    using Ninject.Syntax;

    /// <summary>
    /// Provides extensions for the IKernel interface
    /// </summary>
    public static class ExtensionsForIKernel
    {
        /// <summary>
        /// Creates bindings using conventions
        /// </summary>
        /// <param name="kernel">The kernel for which the bindings are created.</param>
        /// <param name="configure">The binding convention configuration.</param>
        public static void Bind(this IBindingRoot kernel, Action<IFromSyntax> configure)
        {
            if (configure == null)
            {
                throw new ArgumentNullException("configure");
            }

#if !NO_ASSEMBLY_SCANNING
            var assemblyNameRetriever = new AssemblyNameRetriever();
            try
            {
                var builder = new ConventionSyntax(
                    new ConventionBindingBuilder(kernel, new TypeSelector()),
                    new AssemblyFinder(assemblyNameRetriever),
                    new TypeFilter(),
                    new BindingGeneratorFactory(new BindableTypeSelector()));
                configure(builder);
            }
            finally
            {
                assemblyNameRetriever.Dispose();
            }
#else
            var builder = new ConventionSyntax(
                new ConventionBindingBuilder(kernel, new TypeSelector()), 
                new TypeFilter(), 
                new BindingGeneratorFactory(new BindableTypeSelector()));
            configure(builder);
#endif
        }
    }
}