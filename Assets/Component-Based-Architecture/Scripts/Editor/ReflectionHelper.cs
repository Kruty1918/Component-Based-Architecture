using System.Collections.Generic;
using System;
using System.Linq;

namespace SGS29.Editor
{
    public static class ReflectionHelper
    {
        public static List<Type> GetAllImplementationsOfGenericInterface(Type genericInterfaceType)
        {
            if (!genericInterfaceType.IsInterface || !genericInterfaceType.IsGenericTypeDefinition)
            {
                throw new ArgumentException("Потрібно передати відкритий generic-інтерфейс.");
            }

            return AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => !type.IsAbstract && !type.IsInterface) // Беремо лише конкретні класи
                .Where(type => type.GetInterfaces()
                    .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == genericInterfaceType)) // Фільтр за generic-інтерфейсом
                .ToList();
        }
    }
}
