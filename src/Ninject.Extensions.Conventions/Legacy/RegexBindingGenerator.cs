// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RegexBindingGenerator.cs" company="">
//   
// </copyright>
// <summary>
//   Creates binding based on regex patterns to match interfaces to implementations.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Ninject.Extensions.Conventions
{
    using System;
    using System.Text.RegularExpressions;

    using Ninject.Activation;

    /// <summary>
    /// Creates binding based on regex patterns to match interfaces to implementations.
    /// </summary>
    public class RegexBindingGenerator : IBindingGenerator
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RegexBindingGenerator"/> class. 
        /// The regex binding generator.
        /// </summary>
        /// <param name="pattern">
        /// </param>
        public RegexBindingGenerator(string pattern)
            : this(pattern, RegexOptions.None)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RegexBindingGenerator"/> class. 
        /// The regex binding generator.
        /// </summary>
        /// <param name="pattern">
        /// </param>
        /// <param name="options">
        /// </param>
        public RegexBindingGenerator(string pattern, RegexOptions options)
        {
            this.Regex = new Regex(pattern, options);
        }

        #endregion

        #region Properties

        /// <summary>
        /// The regex.
        /// </summary>
        public Regex Regex { get; set; }

        #endregion

        #region Implemented Interfaces

        #region IBindingGenerator

        /// <summary>
        /// Processes the specified type creating kernel bindings.
        ///   If the type being processed has an interface that matches the regex pattern, 
        ///   a binding will be created for them.
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

            Type[] interfaces = type.GetInterfaces();

            foreach (Type @interface in interfaces)
            {
                if (this.Regex.IsMatch(@interface.Name))
                {
                    kernel.Bind(@interface).To(type).InScope(scopeCallback);
                }
            }
        }

        #endregion

        #endregion
    }
}