//-------------------------------------------------------------------------------
// <copyright file="ConventionBindingBuilder.cs" company="Ninject Project Contributors">
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
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Ninject.Extensions.Conventions.BindingGenerators;
    using Ninject.Syntax;

    /// <summary>
    /// Builder for conventions configurations.
    /// </summary>
    public class ConventionBindingBuilder : IConventionBindingBuilder
    {
        private readonly ITypeSelector typeSelector;
        private readonly IBindingRoot bindingRoot;

        private IEnumerable<Type> allTypes = Enumerable.Empty<Type>();
        private IEnumerable<Type> currentTypes = Enumerable.Empty<Type>();
        private List<Type> types;
        private IEnumerable<IBindingWhenInNamedWithOrOnSyntax<object>> bindingSyntax = 
            Enumerable.Empty<IBindingWhenInNamedWithOrOnSyntax<object>>();

        /// <summary>
        /// Initializes a new instance of the <see cref="ConventionBindingBuilder"/> class.
        /// </summary>
        /// <param name="bindingRoot">The binding root.</param>
        /// <param name="typeSelector">The type selector.</param>
        public ConventionBindingBuilder(IBindingRoot bindingRoot, ITypeSelector typeSelector)
        {
            this.bindingRoot = bindingRoot;
            this.typeSelector = typeSelector;
        }

        /// <summary>
        /// Defines that types are selectes from the specified assemblies.
        /// </summary>
        /// <param name="assemblies">The assemblies from which the types are selected.</param>
        public void SelectAllTypesFrom(IEnumerable<Assembly> assemblies)
        {
            this.UnionTypes();
            this.currentTypes = this.typeSelector.GetTypes(assemblies);
        }

        /// <summary>
        /// Selects the types matching the specified filter from the current selected types.
        /// </summary>
        /// <param name="filter">The filter used to select the types.</param>
        public void Where(Func<Type, bool> filter)
        {
            this.currentTypes = this.currentTypes.Where(filter);
        }

        /// <summary>
        /// Includes the specified types.
        /// </summary>
        /// <param name="types">The types to be included.</param>
        public void Including(IEnumerable<Type> types)
        {
            this.CreateTypeList();

            foreach (var type in types)
            {
                if (!this.types.Contains(type))
                {
                    this.types.Add(type);
                }
            }
        }

        /// <summary>
        /// Excludes the specified types.
        /// </summary>
        /// <param name="types">The types to be excluded.</param>
        public void Excluding(IEnumerable<Type> types)
        {
            this.CreateTypeList();

            foreach (var type in types)
            {
                this.types.Remove(type);
            }
        }

        /// <summary>
        /// Creates the bindings using the specified generator.
        /// </summary>
        /// <param name="generator">The generator to use to create the bindings.</param>
        public void BindWith(IBindingGenerator generator)
        {
            this.CreateTypeList();
            
            foreach (var type in this.types)
            {
                this.bindingSyntax = this.bindingSyntax.Union(generator.CreateBindings(type, this.bindingRoot));
            }

            this.bindingSyntax = this.bindingSyntax.ToList();
        }

        /// <summary>
        /// Configures the bindings using the specified configuration.
        /// </summary>
        /// <param name="configuration">The configuration that is applies to the bindings.</param>
        public void Configure(Action<IBindingWhenInNamedWithOrOnSyntax<object>> configuration)
        {
            foreach (var syntax in this.bindingSyntax)
            {
                configuration(syntax);
            }
        }

        private void UnionTypes()
        {
            this.allTypes = this.allTypes.Union(this.currentTypes);
        }

        private void CreateTypeList()
        {
            if (this.types == null)
            {
                this.UnionTypes();
                this.types = this.allTypes.Distinct().ToList();
                this.allTypes = null;
                this.currentTypes = null;
            }
        }
    }
}