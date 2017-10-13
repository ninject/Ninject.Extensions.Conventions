// -------------------------------------------------------------------------------------------------
// <copyright file="ConventionBindingBuilder.cs" company="Ninject Project Contributors">
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
    using Ninject.Extensions.Conventions.BindingGenerators;
    using Ninject.Extensions.Conventions.Syntax;
    using Ninject.Syntax;

    /// <summary>
    /// Builder for conventions configurations.
    /// </summary>
    public class ConventionBindingBuilder : IConventionBindingBuilder
    {
        private readonly ITypeSelector typeSelector;
        private readonly IBindingRoot bindingRoot;
        private readonly IDictionary<Type, IEnumerable<IBindingWhenInNamedWithOrOnSyntax<object>>> bindingSyntax =
            new Dictionary<Type, IEnumerable<IBindingWhenInNamedWithOrOnSyntax<object>>>();

        private IEnumerable<Assembly> selectedAssemblies;
        private IEnumerable<Type> allTypes = Enumerable.Empty<Type>();
        private IEnumerable<Type> currentTypes = Enumerable.Empty<Type>();
        private List<Type> types;
        private bool selectionComplete;

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
            if (this.selectionComplete)
            {
                this.Reset();
            }

            this.selectedAssemblies = assemblies;
            this.UnionTypes();
            this.currentTypes = this.typeSelector.GetExportedTypes(this.selectedAssemblies);
        }

#if !NO_SKIP_VISIBILITY
        /// <summary>
        /// Includes none public types.
        /// </summary>
        public void IncludingNonPublicTypes()
        {
            this.currentTypes = this.typeSelector.GetAllTypes(this.selectedAssemblies);
        }
#endif

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
        /// <param name="includedTypes">The types to be included.</param>
        public void Including(IEnumerable<Type> includedTypes)
        {
            if (includedTypes == null)
            {
                throw new ArgumentNullException("includedTypes");
            }

            this.CreateTypeList();

            foreach (var type in includedTypes)
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
        /// <param name="excludedTypes">The types to be excluded.</param>
        public void Excluding(IEnumerable<Type> excludedTypes)
        {
            if (excludedTypes == null)
            {
                throw new ArgumentNullException("excludedTypes");
            }

            this.CreateTypeList();

            foreach (var type in excludedTypes)
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
            this.selectionComplete = true;
            if (generator == null)
            {
                throw new ArgumentNullException("generator");
            }

            this.CreateTypeList();

            foreach (var type in this.types)
            {
                this.bindingSyntax[type] = generator.CreateBindings(type, this.bindingRoot).ToList();
            }
        }

        /// <summary>
        /// Configures the bindings using the specified configuration.
        /// </summary>
        /// <param name="configuration">The configuration that is applies to the bindings.</param>
        public void Configure(ConfigurationAction configuration)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException("configuration");
            }

            foreach (var bindingSyntaxEntry in this.bindingSyntax)
            {
                foreach (var syntax in bindingSyntaxEntry.Value)
                {
                    configuration(syntax);
                }
            }
        }

        /// <summary>
        /// Configures the bindings using the specified configuration.
        /// </summary>
        /// <param name="configuration">The configuration that is applies to the bindings.</param>
        public void Configure(ConfigurationActionWithService configuration)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException("configuration");
            }

            foreach (var bindingSyntaxEntry in this.bindingSyntax)
            {
                foreach (var syntax in bindingSyntaxEntry.Value)
                {
                    configuration(syntax, bindingSyntaxEntry.Key);
                }
            }
        }

        /// <summary>
        /// Configures the binding of the specified type using the specified configuration.
        /// </summary>
        /// <typeparam name="T">The type to be configured.</typeparam>
        /// <param name="configuration">The configuration that is applies to the bindings.</param>
        public void ConfigureFor<T>(ConfigurationAction configuration)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException("configuration");
            }

            var type = typeof(T);
            foreach (var syntax in this.bindingSyntax[type])
            {
                configuration(syntax);
            }
        }

        /// <summary>
        /// Configures the binding of the specified type using the specified configuration.
        /// </summary>
        /// <typeparam name="T">The type to be configured.</typeparam>
        /// <param name="configuration">The configuration that is applies to the bindings.</param>
        public void ConfigureFor<T>(ConfigurationActionWithService configuration)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException("configuration");
            }

            var type = typeof(T);
            foreach (var syntax in this.bindingSyntax[type])
            {
                configuration(syntax, type);
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

        private void Reset()
        {
            this.bindingSyntax.Clear();
            this.selectedAssemblies = null;
            this.allTypes = Enumerable.Empty<Type>();
            this.currentTypes = Enumerable.Empty<Type>();
            this.types = null;
            this.selectionComplete = false;
        }
    }
}