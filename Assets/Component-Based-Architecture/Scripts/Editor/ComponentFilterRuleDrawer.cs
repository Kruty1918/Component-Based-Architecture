using UnityEditor;
using UnityEngine;
using SGS29.CBA;
using System.Collections.Generic;
using System;

namespace SGS29.Editor
{
    [CustomPropertyDrawer(typeof(ComponentFilterRule))]
    public class ComponentFilterRuleDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            SerializedProperty nameProperty = property.FindPropertyRelative("Name");
            SerializedProperty priorityProperty = property.FindPropertyRelative("Priority");
            SerializedProperty componentProperty = property.FindPropertyRelative("ComponentName");

            float lineHeight = EditorGUIUtility.singleLineHeight;
            float spacing = 2f;
            float yOffset = position.y;

            // Назва правила
            Rect nameRect = new Rect(position.x, yOffset, position.width, lineHeight);
            EditorGUI.PropertyField(nameRect, nameProperty);
            yOffset += lineHeight + spacing;

            // Пріоритет
            Rect priorityRect = new Rect(position.x, yOffset, position.width, lineHeight);
            EditorGUI.PropertyField(priorityRect, priorityProperty);
            yOffset += lineHeight + spacing;

            // Контекстне меню для вибору компонента
            Rect componentRect = new Rect(position.x, yOffset, position.width, lineHeight);
            if (GUI.Button(componentRect, !string.IsNullOrEmpty(componentProperty.stringValue) ?
                    componentProperty.stringValue : "Вибрати компонент", EditorStyles.popup))
            {
                ShowComponentMenu(componentProperty);
            }

            EditorGUI.EndProperty();
        }


        private void ShowComponentMenu(SerializedProperty componentProperty)
        {
            GenericMenu menu = new GenericMenu();
            List<Type> componentTypes = ComponentFinder.GetComponentsImplementingAbstractHandler();

            if (componentTypes.Count > 0)
            {
                foreach (Type type in componentTypes)
                {
                    menu.AddItem(new GUIContent(type.Name), false, () =>
                    {
                        // Зберігаємо тільки ім'я типу компонента як рядок
                        componentProperty.stringValue = type.Name;
                        componentProperty.serializedObject.ApplyModifiedProperties();
                    });
                }
            }
            else
            {
                menu.AddDisabledItem(new GUIContent("Немає компонентів, що наслідують AbstractComponentHandler"));
            }

            menu.ShowAsContext();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return (EditorGUIUtility.singleLineHeight + 2f) * 3;
        }
    }
}