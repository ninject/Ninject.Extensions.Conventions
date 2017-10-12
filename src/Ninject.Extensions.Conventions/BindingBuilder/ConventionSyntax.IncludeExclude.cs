// -------------------------------------------------------------------------------------------------
// <copyright file="ConventionSyntax.IncludeExclude.cs" company="Ninject Project Contributors">
//   Copyright (c) 2009-2017 Ninject Project Contributors
//   Licensed under the Apache License, Version 2.0.
// </copyright>
// -------------------------------------------------------------------------------------------------

namespace Ninject.Extensions.Conventions.BindingBuilder
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Ninject.Extensions.Conventions.Syntax;

    /// <summary>
    /// The syntax to configure the conventions
    /// </summary>
    public partial class ConventionSyntax
    {
        /// <summary>
        /// Includes the given type
        /// </summary>
        /// <typeparam name="T">The type to be included</typeparam>
        /// <returns>The fluent syntax</returns>
        public IExcludeIncludeBindSyntax Including<T>()
        {
            return this.Including(typeof(T));
        }

        /// <summary>
        /// Includes the given types
        /// </summary>
        /// <param name="types">The types to be included.</param>
        /// <returns>The fluent syntax</returns>
        public IExcludeIncludeBindSyntax Including(params Type[] types)
        {
            return this.Including(types.AsEnumerable());
        }

        /// <summary>
        /// Includes the given types
        /// </summary>
        /// <param name="types">The types to be included.</param>
        /// <returns>The fluent syntax</returns>
        public IExcludeIncludeBindSyntax Including(IEnumerable<Type> types)
        {
            this.bindingBuilder.Including(types);
            return this;
        }

        /// <summary>
        /// Excludes the given type
        /// </summary>
        /// <typeparam name="T">The type to be excluded</typeparam>
        /// <returns>The fluent syntax</returns>
        public IExcludeIncludeBindSyntax Excluding<T>()
        {
            return this.Excluding(typeof(T));
        }

        /// <summary>
        /// Excludes the given types
        /// </summary>
        /// <param name="types">The types to be excluded.</param>
        /// <returns>The fluent syntax</returns>
        public IExcludeIncludeBindSyntax Excluding(params Type[] types)
        {
            return this.Excluding(types.AsEnumerable());
        }

        /// <summary>
        /// Excludes the given types
        /// </summary>
        /// <param name="types">The types to be excluded.</param>
        /// <returns>The fluent syntax</returns>
        public IExcludeIncludeBindSyntax Excluding(IEnumerable<Type> types)
        {
            this.bindingBuilder.Excluding(types);
            return this;
        }
    }
}