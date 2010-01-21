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
using System.Text.RegularExpressions;
using Ninject.Activation;

#endregion

namespace Ninject.Extensions.Conventions
{
    /// <summary>
    /// Creates binding based on regex patterns to match interfaces to implementations.
    /// </summary>
    public class RegexBindingGenerator : IBindingGenerator
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pattern"></param>
        public RegexBindingGenerator( string pattern )
            : this( pattern, RegexOptions.None )
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pattern"></param>
        /// <param name="options"></param>
        public RegexBindingGenerator( string pattern, RegexOptions options )
        {
            Regex = new Regex( pattern, options );
        }

        #region Implementation of IBindingGenerator

        /// <summary>
        /// Processes the specified type creating kernel bindings.
        /// If the type being processed has an interface that matches the regex pattern, 
        /// a binding will be created for them.
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

            Type[] interfaces = type.GetInterfaces();

            foreach ( Type @interface in interfaces )
            {
                if ( Regex.IsMatch( @interface.Name ) )
                {
                    kernel.Bind( @interface ).To( type ).InScope( scopeCallback );
                }
            }
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        public Regex Regex { get; set; }
    }
}