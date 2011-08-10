//-------------------------------------------------------------------------------
// <copyright file="IBindingGeneratorFactory.cs" company="Ninject Project Contributors">
//   Copyright (c) 2009-2011 Ninject Project Contributors
//   Authors: Remo Gloor (remo.gloor@gmail.com)
//           
//   Dual-licensed under the Apache License, Version 2.0, and the Microsoft Public License (Ms-PL).
//   you may not use this file except in compliance with one of the Licenses.
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
//-------------------------------------------------------------------------------

namespace Ninject.Extensions.Conventions.BindingBuilder
{
    using System.Text.RegularExpressions;

    using Ninject.Components;
    using Ninject.Extensions.Conventions.BindingGenerators;
    using Ninject.Extensions.Conventions.Syntax;

    /// <summary>
    /// Factory for binding generators.
    /// </summary>
    public interface IBindingGeneratorFactory : INinjectComponent
    {
        /// <summary>
        /// Creates a regex binding generator.
        /// </summary>
        /// <param name="pattern">The regex pattern.</param>
        /// <returns>The newly created generator.</returns>
        IBindingGenerator CreateRegexBindingGenerator(string pattern);

        /// <summary>
        /// Creates a regex binding generator.
        /// </summary>
        /// <param name="pattern">The regex pattern.</param>
        /// <param name="options">The regex options.</param>
        /// <returns>The newly created generator.</returns>
        IBindingGenerator CreateRegexBindingGenerator(string pattern, RegexOptions options);

        /// <summary>
        /// Creates an all interfaces binding generator.
        /// </summary>
        /// <returns>The newly created generator.</returns>
        IBindingGenerator CreateAllInterfacesBindingGenerator();

        /// <summary>
        /// Creates a base binding generator.
        /// </summary>
        /// <returns>The newly created generator.</returns>
        IBindingGenerator CreateBaseBindingGenerator();

        /// <summary>
        /// Creates a default interface binding generator.
        /// </summary>
        /// <returns>The newly created generator.</returns>
        IBindingGenerator CreateDefaultInterfaceBindingGenerator();

        /// <summary>
        /// Creates a default interfaces binding generator.
        /// </summary>
        /// <returns>The newly created generator.</returns>
        IBindingGenerator CreateDefaultInterfacesBindingGenerator();

        /// <summary>
        /// Creates a single interface binding generator.
        /// </summary>
        /// <returns>The newly created generator.</returns>
        IBindingGenerator CreateSingleInterfaceBindingGenerator();

        /// <summary>
        /// Creates a self binding generator.
        /// </summary>
        /// <returns>The newly created generator.</returns>
        IBindingGenerator CreateSelfBindingGenerator();

        /// <summary>
        /// Creates a selector binding generator.
        /// </summary>
        /// <param name="selector">The selector.</param>
        /// <returns>The newly created generator.</returns>
        IBindingGenerator CreateSelectorBindingGenerator(ServiceSelector selector);
    }
}