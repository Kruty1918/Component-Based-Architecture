using UnityEditor;
using UnityEngine;
using SGS29.CBA;
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
            List<string> controllerNames = CBAFilterManager.GetControllerNames();

            if (controllerNames.Count == 0)
            {
                menu.AddDisabledItem(new GUIContent("No controllers available"));
            }
            else
            {
                foreach (string controllerName in controllerNames)
                {
                    menu.AddItem(new GUIContent(controllerName), controllerName == ruleNameProperty.stringValue, () =>
                    {
                        ruleNameProperty.stringValue = controllerName;
                        ruleNameProperty.serializedObject.ApplyModifiedProperties();
                    });
                }
            }

            menu.ShowAsContext();
        }
    }
}
