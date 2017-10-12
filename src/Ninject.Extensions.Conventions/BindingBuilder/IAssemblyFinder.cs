// -------------------------------------------------------------------------------------------------
// <copyright file="IAssemblyFinder.cs" company="Ninject Project Contributors">
//   Copyright (c) 2009-2017 Ninject Project Contributors
//   Licensed under the Apache License, Version 2.0.
// </copyright>
// -------------------------------------------------------------------------------------------------

namespace Ninject.Extensions.Conventions.BindingBuilder
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

#if !NO_ASSEMBLY_SCANNING
    /// <summary>
    /// Provides functionality to detect assemblies.
    /// </summary>
    public interface IAssemblyFinder
    {
        /// <summary>
        /// Finds assemblies using filenames or assembly names.
        /// </summary>
        /// <param name="assemblies">The assemblies to search.</param>
        /// <param name="filter">A filter to filter certain assemblies.</param>
        /// <returns>The matching assemblies.</returns>
        IEnumerable<Assembly> FindAssemblies(IEnumerable<string> assemblies, Predicate<Assembly> filter);

        /// <summary>
        /// Searches for assemblies in the given path.
        /// </summary>
        /// <param name="path">The path to search.</param>
        /// <returns>The names of the detected assemblies.</returns>
        IEnumerable<string> FindAssembliesInPath(string path);

        /// <summary>
        /// Searches for assemblies that match one of the given pattern.
        /// </summary>
        /// <param name="patterns">The patterns.</param>
        /// <returns>The names of the detected assemblies.</returns>
        IEnumerable<string> FindAssembliesMatching(IEnumerable<string> patterns);
    }
#endif
}