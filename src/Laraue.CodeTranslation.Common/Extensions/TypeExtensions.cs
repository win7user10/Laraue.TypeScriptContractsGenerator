﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Laraue.CodeTranslation.Common.Extensions
{
    public static class TypeExtensions
    {
        /// <summary>
        /// Returns does the type or it interfaces implements <see cref="IEnumerable{T}"/>.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsGenericEnumerable(this Type type) => type.HasGenericEnumerableDefinition()
            || type.GetInterfaces().Any(x => x.HasGenericEnumerableDefinition());

        /// <summary>
        /// Returns type of {T} from <see cref="IEnumerable{T}"/> type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Type GetGenericEnumerableType(this Type type) =>
            type.HasGenericEnumerableDefinition()
                ? type.GenericTypeArguments.First()
                : type.GetInterfaces().First(x => x.HasGenericEnumerableDefinition()).GenericTypeArguments.First();

        /// <summary>
        /// Returns true is passed type directly implements <see cref="IEnumerable{T}"/>.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private static bool HasGenericEnumerableDefinition(this Type type)
            => type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IEnumerable<>);


        /// <summary>
        /// Returns does the type or it interfaces implements <see cref="IDictionary{TKey, TValue}"/>.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsDictionary(this Type type) => type.HasDictionaryDefinition()
            || type.GetInterfaces().Any(x => x.HasDictionaryDefinition());

        /// <summary>
        /// Returns true is passed type directly implements <see cref="IDictionary{TKey, TValue}"/>.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private static bool HasDictionaryDefinition(this Type type)
            => type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IDictionary<,>);

        /// <summary>
        /// Returns types of {TKey} and {TValue} from <see cref="IDictionary{TKey, TValue}"/>.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Type[] GetDictionaryTypes(this Type type)
        {
            if (!type.IsDictionary())
            {
                return default;
            }

            return type.HasDictionaryDefinition()
                ? type.GenericTypeArguments
                : type.GetInterfaces().First(x => x.HasDictionaryDefinition()).GenericTypeArguments;
        }

    }
}
