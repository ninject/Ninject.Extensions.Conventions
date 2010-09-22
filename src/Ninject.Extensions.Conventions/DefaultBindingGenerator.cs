#region License

// 
// Author: Ian Davis <ian@innovatian.com>
// 
// Dual-licensed under the Apache License, Version 2.0, and the Microsoft Public License (Ms-PL).
// See the file LICENSE.txt for details.
// 

#endregion

#region Using Directives

using System;
#if NETCF
using System.Linq;
#endif
using Ninject.Activation;

#endregion

namespace Ninject.Extensions.Conventions
{
    /// <summary>
    /// Binds I{Name} to {Name} implementation.
    /// </summary>
    public class DefaultBindingGenerator : IBindingGenerator
    {
        #region Implementation of IBindingGenerator

        /// <summary>
        /// Processes the specified type creating kernel bindings.
        /// </summary>
        /// <param name="type">The type to process.</param>
        /// <param name="scopeCallback">the scope callback.</param>
        /// <param name="kernel">The kernel to configure.</param>
        public void Process( Type type, Func<IContext, object> scopeCallback, IKernel kernel )
        {
            if ( type.IsInterface || type.IsAbstract )
            {
                return;
            }

#if NETCF
            Type interfaceForType = type.GetInterfaces()
                .Where( @interface => string.Equals( @interface.Name, "I" + type.Name, StringComparison.OrdinalIgnoreCase ) )
                .FirstOrDefault();
#else
            Type interfaceForType = type.GetInterface( "I" + type.Name, false );
#endif //NETCF
            if ( interfaceForType == null )
            {
                return;
            }

            kernel.Bind( interfaceForType ).To( type ).InScope( scopeCallback );
        }

        #endregion
    }
}