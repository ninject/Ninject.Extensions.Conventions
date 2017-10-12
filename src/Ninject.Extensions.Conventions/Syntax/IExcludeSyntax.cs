// -------------------------------------------------------------------------------------------------
// <copyright file="IExcludeSyntax.cs" company="Ninject Project Contributors">
//   Copyright (c) 2009-2017 Ninject Project Contributors
//   Licensed under the Apache License, Version 2.0.
// </copyright>
// -------------------------------------------------------------------------------------------------

namespace Ninject.Extensions.Conventions.Syntax
{
    using System;
    using System.Collections.Generic;

    using Ninject.Syntax;

    /// <summary>
    /// The syntax to define types that are excluded.
    /// </summary>
    public interface IExcludeSyntax : IFluentSyntax
    {
        /// <summary>
        /// Excludes the given type
        /// </summary>
        /// <typeparam name="T">The type to be excluded</typeparam>
        /// <returns>The fluent syntax</returns>
        IExcludeIncludeBindSyntax Excluding<T>();

        /// <summary>
        /// Excludes the given types
        /// </summary>
        /// <param name="types">The types to be excluded.</param>
        /// <returns>The fluent syntax</returns>
        IExcludeIncludeBindSyntax Excluding(params Type[] types);

        /// <summary>
        /// Excludes the given types
        /// </summary>
        /// <param name="types">The types to be excluded.</param>
        /// <returns>The fluent syntax</returns>
        IExcludeIncludeBindSyntax Excluding(IEnumerable<Type> types);
    }
}