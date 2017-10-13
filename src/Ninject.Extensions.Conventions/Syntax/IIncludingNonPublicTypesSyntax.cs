// -------------------------------------------------------------------------------------------------
// <copyright file="IIncludingNonPublicTypesSyntax.cs" company="Ninject Project Contributors">
//   Copyright (c) 2009-2017 Ninject Project Contributors
//   Licensed under the Apache License, Version 2.0.
// </copyright>
// -------------------------------------------------------------------------------------------------

namespace Ninject.Extensions.Conventions.Syntax
{
    /// <summary>
    /// The syntax to specify that none public types shall be included.
    /// </summary>
    public interface IIncludingNonPublicTypesSyntax
    {
#if !NO_SKIP_VISIBILITY
        /// <summary>
        /// Specifies that none public types shall be included.
        /// </summary>
        /// <returns>The fluent syntax.</returns>
        ISelectSyntax IncludingNonPublicTypes();
#endif
    }
}