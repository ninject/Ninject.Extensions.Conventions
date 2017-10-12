// -------------------------------------------------------------------------------------------------
// <copyright file="ConventionSyntax.Select.cs" company="Ninject Project Contributors">
//   Copyright (c) 2009-2017 Ninject Project Contributors
//   Licensed under the Apache License, Version 2.0.
// </copyright>
// -------------------------------------------------------------------------------------------------

namespace Ninject.Extensions.Conventions.BindingBuilder
{
    using System;
    using Ninject.Extensions.Conventions.Syntax;
    using Ninject.Syntax;

    /// <summary>
    /// The syntax to configure the conventions
    /// </summary>
    public partial class ConventionSyntax : IFluentSyntax
    {
        /// <summary>
        /// Selects the types using the specified filter.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns>The fluent syntax</returns>
        public IJoinExcludeIncludeBindSyntax Select(Func<Type, bool> filter)
        {
            this.bindingBuilder.Where(filter);
            return this;
        }

        /// <summary>
        /// Selects all types.
        /// </summary>
        /// <returns>The fluent syntax</returns>
        public IJoinFilterWhereExcludeIncludeBindSyntax SelectAllTypes()
        {
            return this.SelectTypes(t => true);
        }

        /// <summary>
        /// Selects all none abstract classes.
        /// </summary>
        /// <returns>The fluent syntax</returns>
        public IJoinFilterWhereExcludeIncludeBindSyntax SelectAllClasses()
        {
            return this.SelectTypes(t => t.IsClass && !t.IsAbstract);
        }

        /// <summary>
        /// Selects all calsses including abstract ones.
        /// </summary>
        /// <returns>The fluent syntax</returns>
        public IJoinFilterWhereExcludeIncludeBindSyntax SelectAllIncludingAbstractClasses()
        {
            return this.SelectTypes(t => t.IsClass);
        }

        /// <summary>
        /// Selects all abstract classes.
        /// </summary>
        /// <returns>The fluent syntax</returns>
        public IJoinFilterWhereExcludeIncludeBindSyntax SelectAllAbstractClasses()
        {
            return this.SelectTypes(t => t.IsClass && t.IsAbstract);
        }

        /// <summary>
        /// Selects all interfaces.
        /// </summary>
        /// <returns>The fluent syntax</returns>
        public IJoinFilterWhereExcludeIncludeBindSyntax SelectAllInterfaces()
        {
            return this.SelectTypes(t => t.IsInterface);
        }
    }
}