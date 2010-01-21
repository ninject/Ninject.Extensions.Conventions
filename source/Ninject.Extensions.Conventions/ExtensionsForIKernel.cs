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

#endregion

namespace Ninject.Extensions.Conventions
{
    public static class ExtensionsForIKernel
    {
        /// <summary>
        /// Assembly the types specified by the assembly loader.
        /// </summary>
        /// <param name="kernel">the kernel instance to act upon.</param>
        /// <param name="assemblyScanner">the loader to process.</param>
        public static void Scan( this IKernel kernel, AssemblyScanner assemblyScanner )
        {
            assemblyScanner.Process( kernel );
        }

        /// <summary>
        /// Loads assembly scanner according to the spedicified acion.
        /// </summary>
        /// <param name="kernel">the kernel instance to act upon.</param>
        /// <param name="scan">the scanner sequence to process.</param>
        public static void Scan( this IKernel kernel, Action<AssemblyScanner> scan )
        {
            var assemblyLoader = new AssemblyScanner();
            scan( assemblyLoader );
            assemblyLoader.Process( kernel );
        }
    }
}