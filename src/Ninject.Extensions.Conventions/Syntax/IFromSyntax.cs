// -------------------------------------------------------------------------------------------------
// <copyright file="IFromSyntax.cs" company="Ninject Project Contributors">
//   Copyright (c) 2009-2017 Ninject Project Contributors. All rights reserved.
//
//   Dual-licensed under the Apache License, Version 2.0, and the Microsoft Public License (Ms-PL).
//   You may not use this file except in compliance with one of the Licenses.
//   You may obtain a copy of the License at
//
//       http://www.apache.org/licenses/LICENSE-2.0
//   or
//       http://www.microsoft.com/opensource/licenses.mspx
//
//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
// </copyright>
// -------------------------------------------------------------------------------------------------

namespace Ninject.Extensions.Conventions.Syntax
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    using Ninject.Syntax;

    /// <summary>
    /// The fluent syntax to select assemblies
    /// </summary>
    public interface IFromSyntax : IFluentSyntax
    {
        /// <summary>
        /// Scans the specified assemblies.
        /// </summary>
        /// <param name="assemblies">The assemblies.</param>
        /// <returns>The fluent syntax.</returns>
        IIncludingNonPublicTypesSelectSyntax From(params Assembly[] assemblies);

        /// <summary>
        /// Scans the specified assemblies.
        /// </summary>
        /// <param name="assemblies">The assemblies.</param>
        /// <returns>The fluent syntax.</returns>
        IIncludingNonPublicTypesSelectSyntax From(IEnumerable<Assembly> assemblies);

        /// <summary>
        /// Scans the calling assembly.
        /// </summary>
        /// <returns>The fluent syntax.</returns>
        IIncludingNonPublicTypesSelectSyntax FromThisAssembly();

        /// <summary>
        /// Scans the assembly containing the specified type.
        /// </summary>
        /// <typeparam name="T">The type that specifies the assembly.</typeparam>
        /// <returns>The fluent syntax.</returns>
        IIncludingNonPublicTypesSelectSyntax FromAssemblyContaining<T>();

        /// <summary>
        /// Scans the assembly containing the specified type..
        /// </summary>
        /// <param name="types">The types that specify the assemblies.</param>
        /// <returns>The fluent syntax.</returns>
        IIncludingNonPublicTypesSelectSyntax FromAssemblyContaining(params Type[] types);

        /// <summary>
        /// Scans the assembly containing the specified type..
        /// </summary>
        /// <param name="types">The types that specify the assemblies.</param>
        /// <returns>The fluent syntax.</returns>
        IIncludingNonPublicTypesSelectSyntax FromAssemblyContaining(IEnumerable<Type> types);

        /// <summary>
        /// Scans the specified assemblies.
        /// </summary>
        /// <param name="assemblies">The name of the assemblies to be scanned.</param>
        /// <returns>The fluent syntax.</returns>
        IIncludingNonPublicTypesSelectSyntax From(params string[] assemblies);

        /// <summary>
        /// Scans the specified assemblies.
        /// </summary>
        /// <param name="assemblies">The name of the assemblies to be scanned.</param>
        /// <returns>The fluent syntax.</returns>
        IIncludingNonPublicTypesSelectSyntax From(IEnumerable<string> assemblies);

        /// <summary>
        /// Scans the specified assemblies.
        /// </summary>
        /// <param name="assemblies">The name of the assemblies to be scanned.</param>
        /// <param name="filter">The filter for filtering the assemblies.</param>
        /// <returns>The fluent syntax.</returns>
        IIncludingNonPublicTypesSelectSyntax From(IEnumerable<string> assemblies, Predicate<Assembly> filter);

        /// <summary>
        /// Scans the assemblies in the given path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>The fluent syntax.</returns>
        IIncludingNonPublicTypesSelectSyntax FromAssembliesInPath(string path);

        /// <summary>
        /// Scans the assemblies in the given path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="filter">The filter used to filter the assemblies.</param>
        /// <returns>The fluent syntax.</returns>
        IIncludingNonPublicTypesSelectSyntax FromAssembliesInPath(string path, Predicate<Assembly> filter);

        /// <summary>
        /// Scans the assemblies that matching one of the given assembly name pattern.
        /// </summary>
        /// <param name="patterns">The patterns to match the assemblies.</param>
        /// <returns>The fluent syntax.</returns>
        IIncludingNonPublicTypesSelectSyntax FromAssembliesMatching(params string[] patterns);

        /// <summary>
        /// Scans the assemblies that matching one of the given assembly name pattern.
        /// </summary>
        /// <param name="patterns">The patterns to match the assemblies.</param>
        /// <returns>The fluent syntax.</returns>
        IIncludingNonPublicTypesSelectSyntax FromAssembliesMatching(IEnumerable<string> patterns);

        /// <summary>
        /// Scans the assemblies that matching one of the given assembly name pattern.
        /// </summary>
        /// <param name="patterns">The patterns to match the assemblies.</param>
        /// <param name="filter">The filter for filtering the assemblies.</param>
        /// <returns>The fluent syntax.</returns>
        IIncludingNonPublicTypesSelectSyntax FromAssembliesMatching(IEnumerable<string> patterns, Predicate<Assembly> filter);
    }
}