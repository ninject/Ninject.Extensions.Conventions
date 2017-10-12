// -------------------------------------------------------------------------------------------------
// <copyright file="ITypeSelector.cs" company="Ninject Project Contributors">
//   Copyright (c) 2009-2017 Ninject Project Contributors
//   Licensed under the Apache License, Version 2.0.
// </copyright>
// -------------------------------------------------------------------------------------------------

namespace Ninject.Extensions.Conventions.BindingBuilder
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    /// <summary>
    /// Selects types from the given assemblies
    /// </summary>
    public interface ITypeSelector
    {
        /// <summary>
        /// Gets all exported types from the specified assemblies.
        /// </summary>
        /// <param name="assemblies">The assemblies to search for types.</param>
        /// <returns>All exported types from the specified assemblies.</returns>
        IEnumerable<Type> GetExportedTypes(IEnumerable<Assembly> assemblies);

#if !NO_SKIP_VISIBILITY
        /// <summary>
        /// Gets all types from the specified assemblies.
        /// </summary>
        /// <param name="assemblies">The assemblies to search for types.</param>
        /// <returns>All types from the specified assemblies.</returns>
        IEnumerable<Type> GetAllTypes(IEnumerable<Assembly> assemblies);
#endif
    }
}