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
    /// Gernerates bindings for the given type in the the kernel.
    /// </summary>
    public interface IBindingGenerator
    {
        /// <summary>
        /// Processes the specified type creating kernel bindings.
        /// </summary>
        /// <param name="type">The type to process.</param>
        /// <param name="scopeCallback">the scope callback.</param>
        /// <param name="kernel">The kernel to configure.</param>
        void Process( Type type, Func<IContext, object> scopeCallback, IKernel kernel );
    }
}