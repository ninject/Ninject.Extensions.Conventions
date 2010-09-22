#region License

// 
// Author: Nate Kohari <nate@enkari.com>
// Copyright (c) 2007-2009, Enkari, Ltd.
// 
// Dual-licensed under the Apache License, Version 2.0, and the Microsoft Public License (Ms-PL).
// See the file LICENSE.txt for details.
// 

#endregion

#region Using Directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Ninject.Modules;

#endregion

namespace Ninject.Extensions.Conventions
{
    internal static class ExtensionsForAssembly
    {
        /// <summary>
        /// Used instead of Type.EmptyTypes to support netcf
        /// </summary>
        private static readonly Type[] EmptyTypes = new Type[0];

        public static bool HasNinjectModules( this Assembly assembly )
        {
            return assembly.GetExportedTypesPlatformSafe().Any( IsLoadableModule );
        }

        public static IEnumerable<INinjectModule> GetNinjectModules( this Assembly assembly )
        {
            return assembly                .GetExportedTypesPlatformSafe()                .Where( IsLoadableModule )                .Select( type => Activator.CreateInstance( type ) as INinjectModule );
        }

        private static bool IsLoadableModule( Type type )
        {
            return typeof (INinjectModule).IsAssignableFrom( type )
                   && !type.IsAbstract
                   && !type.IsInterface
                   && type.GetConstructor( EmptyTypes ) != null;
        }

        public static IEnumerable<Type> GetExportedTypesPlatformSafe( this Assembly assembly )
        {
#if NETCF
            return assembly.GetTypes().Where( type => type.IsPublic );
#else
            return assembly.GetExportedTypes();
#endif
        }
    }
}