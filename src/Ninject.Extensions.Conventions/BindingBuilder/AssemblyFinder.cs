// -------------------------------------------------------------------------------------------------
// <copyright file="AssemblyFinder.cs" company="Ninject Project Contributors">
//   Copyright (c) 2009-2017 Ninject Project Contributors
//   Licensed under the Apache License, Version 2.0.
// </copyright>
// -------------------------------------------------------------------------------------------------

#if !NO_ASSEMBLY_SCANNING
namespace Ninject.Extensions.Conventions.BindingBuilder
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;

    using Ninject.Modules;

    /// <summary>
    /// Provides functionality to detect assemblies.
    /// </summary>
    public class AssemblyFinder : IAssemblyFinder
    {
        /// <summary>
        /// Retrieves the name of an assembly form its file name.
        /// </summary>
        private readonly IAssemblyNameRetriever assemblyNameRetriever;

        /// <summary>
        /// Initializes a new instance of the <see cref="AssemblyFinder"/> class.
        /// </summary>
        /// <param name="assemblyNameRetriever">The assembly name retriever.</param>
        public AssemblyFinder(IAssemblyNameRetriever assemblyNameRetriever)
        {
            this.assemblyNameRetriever = assemblyNameRetriever;
        }

        /// <summary>
        /// Finds assemblies using filenames or assembly names.
        /// </summary>
        /// <param name="assemblies">The assemblies to search.</param>
        /// <param name="filter">A filter to filter certain assemblies.</param>
        /// <returns>The matching assemblies.</returns>
        public IEnumerable<Assembly> FindAssemblies(IEnumerable<string> assemblies, Predicate<Assembly> filter)
        {
            return this.assemblyNameRetriever
                .GetAssemblyNames(assemblies, filter)
                .Select<AssemblyName, Assembly>(Assembly.Load);
        }

        /// <summary>
        /// Searches for assemblies in the given path.
        /// </summary>
        /// <param name="path">The path to search.</param>
        /// <returns>The names of the detected assemblies.</returns>
        public IEnumerable<string> FindAssembliesInPath(string path)
        {
            return Directory.GetFiles(path).Where(IsAssemblyFile);
        }

        /// <summary>
        /// Searches for assemblies that match one of the given pattern.
        /// </summary>
        /// <param name="patterns">The patterns.</param>
        /// <returns>The names of the detected assemblies.</returns>
        public IEnumerable<string> FindAssembliesMatching(IEnumerable<string> patterns)
        {
            return patterns.SelectMany<string, string>(GetFilesMatchingPattern).Where(IsAssemblyFile);
        }

        private static bool IsAssemblyFile(string fileName)
        {
            string extension = Path.GetExtension(fileName);
            return HasAssemblyExtension(extension);
        }

        private static bool HasAssemblyExtension(string extension)
        {
            const StringComparison Comparison = StringComparison.OrdinalIgnoreCase;
            return string.Equals(extension, ".dll", Comparison) ||
                   string.Equals(extension, ".exe", Comparison);
        }

        private static IEnumerable<string> GetFilesMatchingPattern(string pattern)
        {
            string path = NormalizePath(Path.GetDirectoryName(pattern));
            string glob = Path.GetFileName(pattern);

            return Directory.GetFiles(path, glob);
        }

        private static string NormalizePath(string path)
        {
            if (!Path.IsPathRooted(path))
            {
                path = Path.Combine(GetBaseDirectory(), path);
            }

            return Path.GetFullPath(path);
        }

        private static string GetBaseDirectory()
        {
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string searchPath = AppDomain.CurrentDomain.RelativeSearchPath;

            return string.IsNullOrEmpty(searchPath) ? baseDirectory : Path.Combine(baseDirectory, searchPath);
        }
    }
}
#endif