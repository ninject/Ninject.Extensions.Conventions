// -------------------------------------------------------------------------------------------------
// <copyright file="TypeSelector.cs" company="Ninject Project Contributors">
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