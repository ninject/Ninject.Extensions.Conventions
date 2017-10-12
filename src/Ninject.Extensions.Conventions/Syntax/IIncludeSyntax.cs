// -------------------------------------------------------------------------------------------------
// <copyright file="IIncludeSyntax.cs" company="Ninject Project Contributors">
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
    /// The fluent syntax to include some types
    /// </summary>
    public interface IIncludeSyntax : IFluentSyntax
    {
        /// <summary>
        /// Includes the given type
        /// </summary>
        /// <typeparam name="T">The type to be included</typeparam>
        /// <returns>The fluent syntax</returns>
        IExcludeIncludeBindSyntax Including<T>();

        /// <summary>
        /// Includes the given types
        /// </summary>
        /// <param name="types">The types to be included.</param>
        /// <returns>The fluent syntax</returns>
        IExcludeIncludeBindSyntax Including(params Type[] types);

        /// <summary>
        /// Includes the given types
        /// </summary>
        /// <param name="types">The types to be included.</param>
        /// <returns>The fluent syntax</returns>
        IExcludeIncludeBindSyntax Including(IEnumerable<Type> types);
    }
}