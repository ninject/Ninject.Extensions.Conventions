// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IBindingGenerator.cs" company="">
//   
// </copyright>
// <summary>
//   Gernerates bindings for the given type in the the kernel.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Ninject.Extensions.Conventions
{
    using System;

    using Ninject.Activation;

    /// <summary>
    /// Gernerates bindings for the given type in the the kernel.
    /// </summary>
    public interface IBindingGenerator
    {
        #region Public Methods

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
        void Process(Type type, Func<IContext, object> scopeCallback, IKernel kernel);

        #endregion
    }
}