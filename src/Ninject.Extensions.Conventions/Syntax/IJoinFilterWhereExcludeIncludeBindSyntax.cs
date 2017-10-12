// -------------------------------------------------------------------------------------------------
// <copyright file="IJoinFilterWhereExcludeIncludeBindSyntax.cs" company="Ninject Project Contributors">
//   Copyright (c) 2009-2017 Ninject Project Contributors
//   Licensed under the Apache License, Version 2.0.
// </copyright>
// -------------------------------------------------------------------------------------------------

namespace Ninject.Extensions.Conventions.Syntax
{
    /// <summary>
    /// Syntax to filter types, exculde or include types or bind the currently selected types.
    /// </summary>
    public interface IJoinFilterWhereExcludeIncludeBindSyntax : IJoinWhereExcludeIncludeBindSyntax, IFilterSyntax
    {
    }
}