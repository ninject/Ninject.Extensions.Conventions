// -------------------------------------------------------------------------------------------------
// <copyright file="ExtensionsForAssembly.cs" company="Ninject Project Contributors">
//   Copyright (c) 2009-2017 Ninject Project Contributors
//   Licensed under the Apache License, Version 2.0.
// </copyright>
// -------------------------------------------------------------------------------------------------

namespace Ninject.Extensions.Conventions.Extensions
{
    using System;
    using System.Collections.Generic;
#if NETCF
    using System.Linq;
#endif
    using System.Reflection;

    /// <summary>
    /// The extensions for assembly.
    /// </summary>
    internal static class ExtensionsForAssembly
    {
        /// <summary>
        /// Gets all exported types of the specified assembly.
        /// Plattform independent.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <returns>
        /// All exported types of the specified assembly.
        /// </returns>
        public static IEnumerable<Type> GetExportedTypesPlatformSafe(this Assembly assembly)
        {
#if NETCF
            return assembly.GetTypes().Where( type => type.IsPublic );
#else
            return assembly.GetExportedTypes();
#endif
        }
    }
}