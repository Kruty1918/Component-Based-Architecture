using UnityEditor;
using SGS29.CBA;
using UnityEngine;
using System.Collections.Generic;
using System;

namespace SGS29.Editor
{
    [CustomPropertyDrawer(typeof(ComponentFilterRule))]
    public class ComponentFilterRuleDrawer : BaseFilterRuleDrawer
    {
        protected override string GetNameField()
        {
            return "ComponentName";
        }

        protected override void PopulateMenu(GenericMenu menu, SerializedProperty nameProperty)
        {
            // Приклад: отримання списку через ComponentFinder
            List<Type> componentTypes = ComponentFinder.GetComponentsImplementingAbstractHandler();
            if (componentTypes.Count > 0)
            {
                foreach (Type type in componentTypes)
                {
                    menu.AddItem(new GUIContent(type.Name), false, () =>
                    {
                        nameProperty.stringValue = type.Name;
                        nameProperty.serializedObject.ApplyModifiedProperties();
                        nameProperty.serializedObject.Update();
                        GUIX.ForceRebuild();
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
