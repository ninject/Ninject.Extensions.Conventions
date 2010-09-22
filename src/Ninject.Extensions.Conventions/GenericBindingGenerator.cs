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
using Ninject.Activation;

#endregion

namespace Ninject.Extensions.Conventions
{
    /// <summary>
    /// Creates bindings on open generic types.
    /// </summary>
    public class GenericBindingGenerator : IBindingGenerator
    {
        private static readonly Type TypeOfObject = typeof (object);
        private readonly Type _contractType;

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericBindingGenerator"/> class.
        /// </summary>
        /// <param name="contractType">Type of the contract.</param>
        public GenericBindingGenerator( Type contractType )
        {
            if ( !( contractType.IsGenericType || contractType.ContainsGenericParameters ) )
            {
                throw new ArgumentException( "The contract must be an open generic type.", "contractType" );
            }
            _contractType = contractType;
        }

        #region Implementation of IBindingGenerator

        /// <summary>
        /// Processes the specified type creating kernel bindings.
        /// </summary>
        /// <param name="type">The type to process.</param>
        /// <param name="scopeCallback">the scope callback.</param>
        /// <param name="kernel">The kernel to configure.</param>
        public void Process( Type type, Func<IContext, object> scopeCallback, IKernel kernel )
        {
            Type interfaceType = ResolveClosingInterface( type );
            if ( interfaceType != null )
            {
                kernel.Bind( interfaceType ).To( type ).InScope( scopeCallback );
            }
        }

        #endregion

        /// <summary>
        /// Resolves the closing interface.
        /// </summary>
        /// <param name="targetType">Type of the target.</param>
        /// <returns></returns>
        public Type ResolveClosingInterface( Type targetType )
        {
            if ( targetType.IsInterface || targetType.IsAbstract )
            {
                return null;
            }

            do
            {
                Type[] interfaces = targetType.GetInterfaces();
                foreach ( Type @interface in interfaces )
                {
                    if ( !@interface.IsGenericType )
                    {
                        continue;
                    }

                    if ( @interface.GetGenericTypeDefinition() == _contractType )
                    {
                        return @interface;
                    }
                }
                targetType = targetType.BaseType;
            } while ( targetType != TypeOfObject );

            return null;
        }
    }
}