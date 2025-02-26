using UnityEditor;
using UnityEngine;
using SGS29.CBA;
using System.Collections.Generic;
using System;

namespace SGS29.Editor
{
    [CustomPropertyDrawer(typeof(GroupFilterRule))]
    public class GroupFilterRuleDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            int originalIndent = EditorGUI.indentLevel;
            Rect rect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);

            // Малюємо кнопку з контекстним меню для "Name"
            SerializedProperty nameProp = property.FindPropertyRelative("Name");
            string buttonLabel = string.IsNullOrEmpty(nameProp.stringValue) ? "Вибрати ім'я" : nameProp.stringValue;
            if (GUI.Button(rect, buttonLabel, EditorStyles.popup))
            {
                ShowNameMenu(nameProp);
            }
            rect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

            // Малюємо поле "Priority"
            SerializedProperty priorityProp = property.FindPropertyRelative("Priority");
            EditorGUI.PropertyField(rect, priorityProp);
            rect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

            // Збільшуємо відступ для вкладених властивостей (наприклад, список компонентів)
            EditorGUI.indentLevel++;

            SerializedProperty iterator = property.Copy();
            SerializedProperty endProperty = iterator.GetEndProperty();
            bool enterChildren = true;
            while (iterator.NextVisible(enterChildren) && !SerializedProperty.EqualContents(iterator, endProperty))
            {
                if (iterator.name == "Name" || iterator.name == "Priority")
                    continue;
                float propHeight = EditorGUI.GetPropertyHeight(iterator, true);
                EditorGUI.PropertyField(new Rect(rect.x, rect.y, rect.width, propHeight), iterator, true);
                rect.y += propHeight + EditorGUIUtility.standardVerticalSpacing;
                enterChildren = false;
            }

            EditorGUI.indentLevel = originalIndent;
            EditorGUI.EndProperty();
        }

        private void ShowNameMenu(SerializedProperty nameProp)
        {
            GenericMenu menu = new GenericMenu();
            List<Type> componentTypes = ComponentFinder.GetComponentsImplementingAbstractHandler();

            if (componentTypes.Count > 0)
            {
                foreach (Type type in componentTypes)
                {
                    menu.AddItem(new GUIContent(type.Name), false, () =>
                    {
                        nameProp.stringValue = type.Name;
                        nameProp.serializedObject.ApplyModifiedProperties();
                    });
                }
            }
            else
            {
                menu.AddDisabledItem(new GUIContent("Немає доступних компонентів"));
            }
            menu.ShowAsContext();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            float height = 0f;
            SerializedProperty nameProp = property.FindPropertyRelative("Name");
            SerializedProperty priorityProp = property.FindPropertyRelative("Priority");

            height += EditorGUI.GetPropertyHeight(nameProp) + EditorGUIUtility.standardVerticalSpacing;
            height += EditorGUI.GetPropertyHeight(priorityProp) + EditorGUIUtility.standardVerticalSpacing;

            SerializedProperty iterator = property.Copy();
            SerializedProperty endProperty = iterator.GetEndProperty();
            bool enterChildren = true;
            while (iterator.NextVisible(enterChildren) && !SerializedProperty.EqualContents(iterator, endProperty))
            {
                if (iterator.name == "Name" || iterator.name == "Priority")
                    continue;
                height += EditorGUI.GetPropertyHeight(iterator, true) + EditorGUIUtility.standardVerticalSpacing;
                enterChildren = false;
            }
            return height;
        }
    }
}