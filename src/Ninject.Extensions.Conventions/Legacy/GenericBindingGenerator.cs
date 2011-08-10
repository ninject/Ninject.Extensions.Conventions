// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GenericBindingGenerator.cs" company="">
//   
// </copyright>
// <summary>
//   Creates bindings on open generic types.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Ninject.Extensions.Conventions
{
    using System;

    using Ninject.Activation;

    /// <summary>
    /// Creates bindings on open generic types.
    /// </summary>
    public class GenericBindingGenerator : IBindingGenerator
    {
        /// <summary>
        /// The type of object.
        /// </summary>
        private static readonly Type TypeOfObject = typeof(object);

        /// <summary>
        /// The _contract type.
        /// </summary>
        private readonly Type contractType;

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericBindingGenerator"/> class.
        /// </summary>
        /// <param name="contractType">
        /// Type of the contract.
        /// </param>
        public GenericBindingGenerator(Type contractType)
        {
            if (!(contractType.IsGenericType || contractType.ContainsGenericParameters))
            {
                throw new ArgumentException("The contract must be an open generic type.", "contractType");
            }

            this.contractType = contractType;
        }

        /// <summary>
        /// Resolves the closing interface.
        /// </summary>
        /// <param name="targetType">
        /// Type of the target.
        /// </param>
        /// <returns>
        /// </returns>
        public Type ResolveClosingInterface(Type targetType)
        {
            if (targetType.IsInterface || targetType.IsAbstract)
            {
                return null;
            }

            do
            {
                Type[] interfaces = targetType.GetInterfaces();
                foreach (Type @interface in interfaces)
                {
                    if (!@interface.IsGenericType)
                    {
                        continue;
                    }

                    if (@interface.GetGenericTypeDefinition() == this.contractType)
                    {
                        return @interface;
                    }
                }

                targetType = targetType.BaseType;
            }
            while (targetType != TypeOfObject);

            return null;
        }

        /// <summary>
        /// Processes the specified type creating kernel bindings.
        /// </summary>
        /// <param name="type">
        /// The type to process.
        /// </param>
        /// <param name="scopeCallback">
        /// the scope callback.
        /// </param>
        /// <param name="kernel">
        /// The kernel to configure.
        /// </param>
        public void Process(Type type, Func<IContext, object> scopeCallback, IKernel kernel)
        {
            Type interfaceType = this.ResolveClosingInterface(type);
            if (interfaceType != null)
            {
                kernel.Bind(interfaceType).To(type).InScope(scopeCallback);
            }
        }
    }
}