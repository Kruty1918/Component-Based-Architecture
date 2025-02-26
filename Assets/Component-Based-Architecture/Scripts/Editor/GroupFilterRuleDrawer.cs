using UnityEditor;
using UnityEngine;
using SGS29.CBA;
using System.Collections.Generic;
using System;

namespace SGS29.Editor
{
    [CustomPropertyDrawer(typeof(GroupFilterRule))]
    public class GroupFilterRuleDrawer : BaseFilterRuleDrawer
    {
        protected override string GetNameField()
        {
            return "Name";
        }

        protected override void PopulateMenu(GenericMenu menu, SerializedProperty nameProperty)
        {
            // Тут можна реалізувати власну логіку для груп, наприклад, отримати список з іншого джерела
            List<Type> types = ComponentFinder.GetComponentsImplementingAbstractHandler(); // або інший метод
            if (types.Count > 0)
            {
                foreach (Type type in types)
                {
                    menu.AddItem(new GUIContent(type.Name), false, () =>
                    {
                        nameProperty.stringValue = type.Name;
                        nameProperty.serializedObject.ApplyModifiedProperties();
                    });
                }
            }
            else
            {
                menu.AddDisabledItem(new GUIContent("Немає доступних компонентів"));
            }
        }
    }
}