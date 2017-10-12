// -------------------------------------------------------------------------------------------------
// <copyright file="TypeSelector.cs" company="Ninject Project Contributors">
//   Copyright (c) 2009-2017 Ninject Project Contributors
//   Licensed under the Apache License, Version 2.0.
// </copyright>
// -------------------------------------------------------------------------------------------------

namespace Ninject.Extensions.Conventions.BindingBuilder
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using Ninject.Extensions.Conventions.Extensions;

    /// <summary>
    /// Selects types from the given assemblies.
    /// </summary>
    public class TypeSelector : ITypeSelector
    {
        /// <summary>
        /// Gets all exported types from the specified assemblies.
        /// </summary>
        /// <param name="assemblies">The assemblies to search for types.</param>
        /// <returns>
        /// All exported types from the specified assemblies.
        /// </returns>
        public IEnumerable<Type> GetExportedTypes(IEnumerable<Assembly> assemblies)
        {
            return assemblies.SelectMany(asm => asm.GetExportedTypesPlatformSafe());
        }

#if !NO_SKIP_VISIBILITY
        /// <summary>
        /// Gets all types from the specified assemblies.
        /// </summary>
        /// <param name="assemblies">The assemblies to search for types.</param>
        /// <returns>All types from the specified assemblies.</returns>
        public IEnumerable<Type> GetAllTypes(IEnumerable<Assembly> assemblies)
        {
            return assemblies.SelectMany(asm => asm.GetTypes());
        }
#endif
    }
}