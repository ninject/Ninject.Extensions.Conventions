// -------------------------------------------------------------------------------------------------
// <copyright file="IWhereSyntax.cs" company="Ninject Project Contributors">
//   Copyright (c) 2009-2017 Ninject Project Contributors
//   Licensed under the Apache License, Version 2.0.
// </copyright>
// -------------------------------------------------------------------------------------------------

namespace Ninject.Extensions.Conventions.Syntax
{
    using System;

    using Ninject.Syntax;

    /// <summary>
    /// Fluent syntax to filter the selected types
    /// </summary>
    public interface IWhereSyntax : IFluentSyntax
    {
        /// <summary>
        /// Filters the selected types.
        /// </summary>
        /// <param name="filter">The filter used to filter the selected types.</param>
        /// <returns>The fluent syntax</returns>
        IJoinExcludeIncludeBindSyntax Where(Func<Type, bool> filter);
    }
}