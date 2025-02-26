using UnityEngine;
using SGS29.CBA;
using System.Collections.Generic;
using System;
using System.Linq;

namespace SGS29.Editor
{
    public static class ComponentFinder
    {
        public static List<Type> GetComponentsImplementingInterface(Type interfaceType)
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => interfaceType.IsAssignableFrom(type) && !type.IsAbstract && type.IsSubclassOf(typeof(Component)))
                .ToList();
        }

        public static List<Type> GetComponentsImplementingAbstractHandler()
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => type.BaseType != null &&
                               type.BaseType.IsGenericType &&
                               type.BaseType.GetGenericTypeDefinition() == typeof(AbstractComponentHandler<>) &&
                               typeof(Component).IsAssignableFrom(type))
                .ToList();
        }
    }
}
