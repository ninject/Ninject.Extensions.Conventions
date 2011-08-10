// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DefaultBindingGenerator.cs" company="">
//   
// </copyright>
// <summary>
//   Binds I{Name} to {Name} implementation.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

#region Using Directives

#if NETCF
using System.Linq;
#endif

#endregion

namespace Ninject.Extensions.Conventions
{
    using System;

    using Ninject.Activation;

    /// <summary>
    /// Binds I{Name} to {Name} implementation.
    /// </summary>
    public class DefaultBindingGenerator : IBindingGenerator
    {
        #region Implemented Interfaces

        #region IBindingGenerator

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
            if (type.IsInterface || type.IsAbstract)
            {
                return;
            }

#if NETCF
            Type interfaceForType = type.GetInterfaces()
                .Where( @interface => string.Equals( @interface.Name, "I" + type.Name, StringComparison.OrdinalIgnoreCase ) )
                .FirstOrDefault();
#else
            Type interfaceForType = type.GetInterface("I" + type.Name, false);
#endif

            // NETCF
            if (interfaceForType == null)
            {
                return;
            }

            kernel.Bind(interfaceForType).To(type).InScope(scopeCallback);
        }

        #endregion

        #endregion
    }
}