namespace Ninject.Extensions.Conventions.BindingGenerators
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using Ninject.Syntax;

    /// <summary>
    /// Binds the type to all specified service types.
    /// </summary>
    public class SpecificTypesBindingGenerator : IBindingGenerator
    {
        private readonly Type[] serviceTypes;

        /// <summary>
        /// Initializes a new instance of the <see cref="SpecificTypesBindingGenerator"/> class.
        /// </summary>
        /// <param name="serviceTypes">The service types the bindings should be created for.</param>
        public SpecificTypesBindingGenerator(Type[] serviceTypes)
        {
            this.serviceTypes = serviceTypes;
        }

        /// <summary>
        /// Creates the bindings for a type.
        /// </summary>
        /// <param name="type">The type for which the bindings are created.</param>
        /// <param name="bindingRoot">The binding root that is used to create the bindings.</param>
        /// <returns>
        /// The syntaxes of the created bindings to configure more options.
        /// </returns>
        public IEnumerable<IBindingWhenInNamedWithOrOnSyntax<object>> CreateBindings(Type type, IBindingRoot bindingRoot)
        {
            if (bindingRoot == null)
            {
                throw new ArgumentNullException("bindingRoot");
            }

            this.AssertTypeImplementsServiceTypes(type);

            return new[]
            {
                bindingRoot.Bind(this.serviceTypes).To(type)
            };
        }

        private void AssertTypeImplementsServiceTypes(Type type)
        {
            var notImplementedServiceTypes = this.serviceTypes
                .Where(serviceType => !serviceType.IsAssignableFrom(type))
                .ToList();

            if (notImplementedServiceTypes.Any())
            {
                var stringBuilder = new StringBuilder();
                stringBuilder
                    .AppendFormat(CultureInfo.InvariantCulture, "The type '{0}' does not implement the following service types: ", type.FullName)
                    .AppendLine();

                foreach (Type serviceType in notImplementedServiceTypes)
                {
                    stringBuilder
                        .AppendFormat(CultureInfo.InvariantCulture, " - {0}", serviceType)
                        .AppendLine();
                }

                stringBuilder.Append("Fix your binding to only bind service types which are implemented.");
                throw new InvalidOperationException(stringBuilder.ToString());
            }
        }
    }
}