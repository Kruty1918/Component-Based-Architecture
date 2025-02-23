using UnityEditor;
using UnityEngine;
using SGS29.ComponentBasedArchitecture;
using System.Collections.Generic;
using System;

namespace SGS29.Editor
{
    /// <summary>
    /// Відповідає за створення контекстного меню вибору правил
    /// </summary>
    public class FilterRuleContextMenu
    {
        public void Show(SerializedProperty ruleNameProperty)
        {
            GenericMenu menu = new GenericMenu();
            List<Type> componentGroups = ReflectionHelper.GetAllImplementationsOfGenericInterface(typeof(IComponentGroup<,>));

            foreach (Type type in componentGroups)
            {
                menu.AddItem(new GUIContent(type.Name), type.Name == ruleNameProperty.stringValue, () =>
                {
                    ruleNameProperty.stringValue = type.Name;
                    ruleNameProperty.serializedObject.ApplyModifiedProperties();
                });
            }

            menu.ShowAsContext();
        }
    }
}