// -------------------------------------------------------------------------------------------------
// <copyright file="IFilterSyntax.cs" company="Ninject Project Contributors">
//   Copyright (c) 2009-2017 Ninject Project Contributors
//   Licensed under the Apache License, Version 2.0.
// </copyright>
// -------------------------------------------------------------------------------------------------

namespace Ninject.Extensions.Conventions.Syntax
{
    using System;
    using System.Collections.Generic;

    using Ninject.Syntax;

    /// <summary>
    /// Syntax for the selection of the types.
    /// </summary>
    public interface IFilterSyntax : IFluentSyntax
    {
        /// <summary>
        /// Selects the types in the specified namespaces.
        /// </summary>
        /// <param name="namespaces">The namespaces from which the types are selected.</param>
        /// <returns>The fluent syntax</returns>
        IJoinFilterWhereExcludeIncludeBindSyntax InNamespaces(IEnumerable<string> namespaces);

        /// <summary>
        /// Selects the types in the specified namespaces.
        /// </summary>
        /// <param name="namespaces">The namespaces from which the types are selected.</param>
        /// <returns>The fluent syntax</returns>
        IJoinFilterWhereExcludeIncludeBindSyntax InNamespaces(params string[] namespaces);

        /// <summary>
        /// Selects the types in the same namespace as the given type.
        /// </summary>
        /// <typeparam name="T">The type defining the namespace.</typeparam>
        /// <returns>The fluent syntax</returns>
        IJoinFilterWhereExcludeIncludeBindSyntax InNamespaceOf<T>();

        /// <summary>
        /// Selects the types in the same namespaces as the given types.
        /// </summary>
        /// <param name="types">The types defining the namespaces.</param>
        /// <returns>The fluent syntax</returns>
        IJoinFilterWhereExcludeIncludeBindSyntax InNamespaceOf(params Type[] types);

        /// <summary>
        /// Selects all types not in the given namespaces.
        /// </summary>
        /// <param name="namespaces">The namespaces from which the types are not selected.</param>
        /// <returns>The fluent syntax</returns>
        IJoinFilterWhereExcludeIncludeBindSyntax NotInNamespaces(IEnumerable<string> namespaces);

        /// <summary>
        /// Selects all types not in the given namespaces.
        /// </summary>
        /// <param name="namespaces">The namespaces from which the types are not selected.</param>
        /// <returns>The fluent syntax</returns>
        IJoinFilterWhereExcludeIncludeBindSyntax NotInNamespaces(params string[] namespaces);

        /// <summary>
        /// Selects all types not in same namespaces as the given type.
        /// </summary>
        /// <typeparam name="T">The type defining the namespace.</typeparam>
        /// <returns>The fluent syntax</returns>
        IJoinFilterWhereExcludeIncludeBindSyntax NotInNamespaceOf<T>();

        /// <summary>
        /// Selects all types not in same namespaces as the given types.
        /// </summary>
        /// <param name="types">The types defining the namespace.</param>
        /// <returns>The fluent syntax</returns>
        IJoinFilterWhereExcludeIncludeBindSyntax NotInNamespaceOf(params Type[] types);

        /// <summary>
        /// Selects the types inherited from any of the given types.
        /// </summary>
        /// <param name="types">The ancestor types.</param>
        /// <returns>The fluent syntax</returns>
        IJoinFilterWhereExcludeIncludeBindSyntax InheritedFromAny(IEnumerable<Type> types);

        /// <summary>
        /// Selects the types inherited from any of the given types.
        /// </summary>
        /// <param name="types">The ancestor types.</param>
        /// <returns>The fluent syntax</returns>
        IJoinFilterWhereExcludeIncludeBindSyntax InheritedFromAny(params Type[] types);

        /// <summary>
        /// Selects the types inherited from the given types.
        /// </summary>
        /// <param name="type">The ancestor type.</param>
        /// <returns>The fluent syntax</returns>
        IJoinFilterWhereExcludeIncludeBindSyntax InheritedFrom(Type type);

        /// <summary>
        /// Selects the types inherited from the given types.
        /// </summary>
        /// <typeparam name="T">The ancestor type.</typeparam>
        /// <returns>The fluent syntax</returns>
        IJoinFilterWhereExcludeIncludeBindSyntax InheritedFrom<T>();

        /// <summary>
        /// Selects the types with the specified attribute.
        /// </summary>
        /// <typeparam name="T">The type of the attribute</typeparam>
        /// <returns>The fluent syntax</returns>
        IJoinFilterWhereExcludeIncludeBindSyntax WithAttribute<T>()
            where T : Attribute;

        /// <summary>
        /// Selects the types that matches the specified attribute predicate.
        /// </summary>
        /// <typeparam name="T">The type of the attribute</typeparam>
        /// <param name="predicate">A function to test if an attribute matches.</param>
        /// <returns>The fluent syntax</returns>
        IJoinFilterWhereExcludeIncludeBindSyntax WithAttribute<T>(Func<T, bool> predicate)
            where T : Attribute;

        /// <summary>
        /// Selects the types with the specified attribute.
        /// </summary>
        /// <param name="attributeType">The type of the attribute.</param>
        /// <returns>The fluent syntax</returns>
        IJoinFilterWhereExcludeIncludeBindSyntax WithAttribute(Type attributeType);

        /// <summary>
        /// Selects the types without the specified attribute.
        /// </summary>
        /// <typeparam name="T">The type of the attribute</typeparam>
        /// <returns>The fluent syntax</returns>
        IJoinFilterWhereExcludeIncludeBindSyntax WithoutAttribute<T>()
            where T : Attribute;

        /// <summary>
        /// Selects the types that do not match the specified attribute predicate.
        /// </summary>
        /// <typeparam name="T">The type of the attribute</typeparam>
        /// <param name="predicate">A function to test if an attribute matches.</param>
        /// <returns>The fluent syntax</returns>
        IJoinFilterWhereExcludeIncludeBindSyntax WithoutAttribute<T>(Func<T, bool> predicate)
            where T : Attribute;

        /// <summary>
        /// Selects the types without the specified attribute.
        /// </summary>
        /// <param name="attributeType">The type of the attribute.</param>
        /// <returns>The fluent syntax</returns>
        IJoinFilterWhereExcludeIncludeBindSyntax WithoutAttribute(Type attributeType);

        /// <summary>
        /// Selects the types that are generic.
        /// </summary>
        /// <returns>The fluent syntax</returns>
        IJoinFilterWhereExcludeIncludeBindSyntax WhichAreGeneric();

        /// <summary>
        /// Selects the types that are not generic.
        /// </summary>
        /// <returns>The fluent syntax</returns>
        IJoinFilterWhereExcludeIncludeBindSyntax WhichAreNotGeneric();

        /// <summary>
        /// Selects all types that have the specified postfix.
        /// </summary>
        /// <param name="postfix">The postfix.</param>
        /// <returns>The fluent syntax</returns>
        IJoinFilterWhereExcludeIncludeBindSyntax EndingWith(string postfix);

        /// <summary>
        /// Selects all types that have the specified prefix.
        /// </summary>
        /// <param name="prefix">The prefix.</param>
        /// <returns>The fluent syntax</returns>
        IJoinFilterWhereExcludeIncludeBindSyntax StartingWith(string prefix);
    }
}