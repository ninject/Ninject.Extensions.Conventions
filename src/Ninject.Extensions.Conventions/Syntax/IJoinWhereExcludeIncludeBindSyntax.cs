// -------------------------------------------------------------------------------------------------
// <copyright file="IJoinWhereExcludeIncludeBindSyntax.cs" company="Ninject Project Contributors">
//   Copyright (c) 2009-2017 Ninject Project Contributors
//   Licensed under the Apache License, Version 2.0.
// </copyright>
// -------------------------------------------------------------------------------------------------

namespace Ninject.Extensions.Conventions.Syntax
{
    /// <summary>
    /// From, Where, Exclude, Include, Bind syntax
    /// </summary>
    public interface IJoinWhereExcludeIncludeBindSyntax : IJoinExcludeIncludeBindSyntax, IWhereSyntax
    {
    }
}