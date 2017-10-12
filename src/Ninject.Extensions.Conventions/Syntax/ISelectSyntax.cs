// -------------------------------------------------------------------------------------------------
// <copyright file="ISelectSyntax.cs" company="Ninject Project Contributors">
//   Copyright (c) 2009-2017 Ninject Project Contributors
//   Licensed under the Apache License, Version 2.0.
// </copyright>
// -------------------------------------------------------------------------------------------------

namespace Ninject.Extensions.Conventions.Syntax
{
    using System;

    using Ninject.Syntax;

    /// <summary>
    /// Syntax to select the types
    /// </summary>
    public interface ISelectSyntax : IFluentSyntax
    {
        /// <summary>
        /// Selects the types using the specified filter.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns>The fluent syntax</returns>
        IJoinExcludeIncludeBindSyntax Select(Func<Type, bool> filter);

        /// <summary>
        /// Selects all types.
        /// </summary>
        /// <returns>The fluent syntax</returns>
        IJoinFilterWhereExcludeIncludeBindSyntax SelectAllTypes();

        /// <summary>
        /// Selects all none abstract classes.
        /// </summary>
        /// <returns>The fluent syntax</returns>
        IJoinFilterWhereExcludeIncludeBindSyntax SelectAllClasses();

        /// <summary>
        /// Selects all calsses including abstract ones.
        /// </summary>
        /// <returns>The fluent syntax</returns>
        IJoinFilterWhereExcludeIncludeBindSyntax SelectAllIncludingAbstractClasses();

        /// <summary>
        /// Selects all abstract classes.
        /// </summary>
        /// <returns>The fluent syntax</returns>
        IJoinFilterWhereExcludeIncludeBindSyntax SelectAllAbstractClasses();

        /// <summary>
        /// Selects all interfaces.
        /// </summary>
        /// <returns>The fluent syntax</returns>
        IJoinFilterWhereExcludeIncludeBindSyntax SelectAllInterfaces();
    }
}