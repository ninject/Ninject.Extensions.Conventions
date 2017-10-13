// -------------------------------------------------------------------------------------------------
// <copyright file="ConventionSyntax.cs" company="Ninject Project Contributors">
//   Copyright (c) 2009-2017 Ninject Project Contributors
//   Licensed under the Apache License, Version 2.0.
// </copyright>
// -------------------------------------------------------------------------------------------------

namespace Ninject.Extensions.Conventions.BindingBuilder
{
    using System;
    using Ninject.Extensions.Conventions.Syntax;

    /// <summary>
    /// The syntax to configure the conventions
    /// </summary>
    public partial class ConventionSyntax : IJoinFilterWhereExcludeIncludeBindSyntax, IIncludingNonPublicTypesSelectSyntax
    {
        /// <summary>
        /// Builder for conventions
        /// </summary>
        private readonly IConventionBindingBuilder bindingBuilder;

#if !NO_ASSEMBLY_SCANNING
        /// <summary>
        /// Finder for assemblies.
        /// </summary>
        private readonly IAssemblyFinder assemblyFinder;
#endif

        /// <summary>
        /// Filter for types.
        /// </summary>
        private readonly ITypeFilter typeFilter;

        /// <summary>
        /// Factory to create binding generators.
        /// </summary>
        private readonly IBindingGeneratorFactory bindingGeneratorFactory;

#if !NO_ASSEMBLY_SCANNING
        /// <summary>
        /// Initializes a new instance of the <see cref="ConventionSyntax"/> class.
        /// </summary>
        /// <param name="bindingBuilder">The binding builder.</param>
        /// <param name="assemblyFinder">The assembly finder.</param>
        /// <param name="typeFilter">The type filter.</param>
        /// <param name="bindingGeneratorFactory">The binding generator factory.</param>
        public ConventionSyntax(
            IConventionBindingBuilder bindingBuilder,
            IAssemblyFinder assemblyFinder,
            ITypeFilter typeFilter,
            IBindingGeneratorFactory bindingGeneratorFactory)
        {
            this.bindingBuilder = bindingBuilder;
            this.assemblyFinder = assemblyFinder;
            this.typeFilter = typeFilter;
            this.bindingGeneratorFactory = bindingGeneratorFactory;
        }
#else
        /// <summary>
        /// Initializes a new instance of the <see cref="ConventionSyntax"/> class.
        /// </summary>
        /// <param name="bindingBuilder">The binding builder.</param>
        /// <param name="typeFilter">The type filter.</param>
        /// <param name="bindingGeneratorFactory">The binding generator factory.</param>
        public ConventionSyntax(
            IConventionBindingBuilder bindingBuilder, 
            ITypeFilter typeFilter,
            IBindingGeneratorFactory bindingGeneratorFactory)
        {
            this.bindingBuilder = bindingBuilder;
            this.typeFilter = typeFilter;
            this.bindingGeneratorFactory = bindingGeneratorFactory;
        }
#endif

        /// <inheritdoc />
        public IFromSyntax Join
        {
            get
            {
                return this;
            }
        }

        /// <inheritdoc />
        public IJoinExcludeIncludeBindSyntax Where(Func<Type, bool> filter)
        {
            this.bindingBuilder.Where(filter);
            return this;
        }

#if !NO_SKIP_VISIBILITY
        /// <inheritdoc />
        public ISelectSyntax IncludingNonPublicTypes()
        {
            this.bindingBuilder.IncludingNonPublicTypes();
            return this;
        }
#endif

        private IJoinFilterWhereExcludeIncludeBindSyntax SelectTypes(Func<Type, bool> filter)
        {
            this.bindingBuilder.Where(filter);
            return this;
        }
    }
}