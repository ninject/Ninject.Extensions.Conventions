// -------------------------------------------------------------------------------------------------
// <copyright file="IExcludeIncludeBindSyntax.cs" company="Ninject Project Contributors">
//   Copyright (c) 2009-2017 Ninject Project Contributors
//   Licensed under the Apache License, Version 2.0.
// </copyright>
// -------------------------------------------------------------------------------------------------

namespace Ninject.Extensions.Conventions.Syntax
{
    /// <summary>
    /// The syntax for exclude, include and binding definition.
    /// </summary>
    public interface IExcludeIncludeBindSyntax : IExcludeSyntax, IIncludeSyntax, IBindSyntax
    {
    }
}