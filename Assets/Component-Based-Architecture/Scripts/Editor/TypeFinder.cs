using UnityEngine;
using SGS29.CBA;
using System.Collections.Generic;
using System;
using System.Reflection;

namespace SGS29.Editor
{
    public static class TypeFinder
    {
        public static List<Type> GetDerivedTypes()
        {
            List<Type> result = new List<Type>();
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

            foreach (Assembly assembly in assemblies)
            {
                foreach (Type type in assembly.GetTypes())
                {
                    if (IsValidType(type))
                    {
                        result.Add(type);
                    }
                }
            }

            return result;
        }

        public static List<string> GetDerivedTypeNames()
        {
            List<string> result = new List<string>();
            foreach (Type type in GetDerivedTypes())
            {
                result.Add(type.Name);
            }
            return result;
        }

        private static bool IsValidType(Type type)
        {
            return type.BaseType != null &&
                   type.BaseType.IsGenericType &&
                   type.BaseType.GetGenericTypeDefinition() == typeof(AbstractComponentHandler<>) &&
                   typeof(Component).IsAssignableFrom(type);
        }
    }
}
