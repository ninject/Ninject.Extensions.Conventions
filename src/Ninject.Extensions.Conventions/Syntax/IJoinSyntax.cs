﻿// -------------------------------------------------------------------------------------------------
// <copyright file="IJoinSyntax.cs" company="Ninject Project Contributors">
//   Copyright (c) 2009-2017 Ninject Project Contributors
//   Licensed under the Apache License, Version 2.0.
// </copyright>
// -------------------------------------------------------------------------------------------------
namespace Ninject.Extensions.Conventions.Syntax
{
    using Ninject.Syntax;

    /// <summary>
    /// Syntax to allow multiple From.Select statements
    /// </summary>
    public interface IJoinSyntax : IFluentSyntax
    {
        /// <summary>
        /// Gets the from syntax to select additional types from different assemblies
        /// </summary>
        /// <value>The fluent syntax.</value>
        IFromSyntax Join { get; }
    }
}