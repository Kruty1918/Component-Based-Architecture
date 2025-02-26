using UnityEditor;
using UnityEngine;

namespace SGS29.Editor
{
    public abstract class BaseFilterRuleDrawer : PropertyDrawer
    {
        protected abstract string GetNameField();
        protected abstract void PopulateMenu(GenericMenu menu, SerializedProperty nameProperty);

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            float lineHeight = EditorGUIUtility.singleLineHeight;
            float spacing = EditorGUIUtility.standardVerticalSpacing;
            float y = position.y;

            SerializedProperty nameProp = property.FindPropertyRelative(GetNameField());
            SerializedProperty priorityProp = property.FindPropertyRelative("Priority");

            if (nameProp == null || priorityProp == null)
            {
                EditorGUI.HelpBox(new Rect(position.x, y, position.width, lineHeight * 2),
                    "Помилка: Властивість не знайдено!", MessageType.Error);
                return;
            }

            string buttonLabel = string.IsNullOrEmpty(nameProp.stringValue) ? "Вибрати ім'я" : nameProp.stringValue;
            Rect buttonRect = new Rect(position.x, y, position.width, lineHeight);

            if (GUI.Button(buttonRect, new GUIContent(buttonLabel, "Оберіть ім'я для правила фільтрації"), EditorStyles.popup))
            {
                GenericMenu menu = new GenericMenu();
                PopulateMenu(menu, nameProp);
                menu.ShowAsContext();
            }

            y += lineHeight + spacing;

            bool hasValidName = !string.IsNullOrEmpty(nameProp.stringValue);
            bool isParentValid = IsParentValid(property);

            if (!hasValidName && isParentValid)
            {
                Rect warningRect = new Rect(position.x, y, position.width, lineHeight * 2);
                EditorGUI.HelpBox(warningRect, "Необхідно вибрати ім'я!", MessageType.Warning);
                y += warningRect.height + spacing;
            }

            GUI.enabled = hasValidName && isParentValid;
            Rect priorityRect = new Rect(position.x, y, position.width, lineHeight);
            EditorGUI.PropertyField(priorityRect, priorityProp, new GUIContent("Пріоритет", "Задайте рівень пріоритету для правила"));
            y += lineHeight + spacing;

            SerializedProperty iterator = property.Copy();
            SerializedProperty endProp = iterator.GetEndProperty();
            bool enterChildren = true;

            while (iterator.NextVisible(enterChildren) && !SerializedProperty.EqualContents(iterator, endProp))
            {
                if (iterator.name == GetNameField() || iterator.name == "Priority")
                    continue;

                float propertyHeight = EditorGUI.GetPropertyHeight(iterator, true);
                Rect propRect = new Rect(position.x, y, position.width, propertyHeight);
                EditorGUI.PropertyField(propRect, iterator, new GUIContent(iterator.displayName), true);

                y += propertyHeight + spacing;
                enterChildren = false;
            }

            GUI.enabled = true;
            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            float height = 0f;
            float spacing = EditorGUIUtility.standardVerticalSpacing;

            SerializedProperty nameProp = property.FindPropertyRelative(GetNameField());
            SerializedProperty priorityProp = property.FindPropertyRelative("Priority");

            if (nameProp == null || priorityProp == null)
            {
                return EditorGUIUtility.singleLineHeight * 2 + spacing;
            }

            height += EditorGUIUtility.singleLineHeight + spacing;
            if (string.IsNullOrEmpty(nameProp.stringValue))
                height += EditorGUIUtility.singleLineHeight * 2 + spacing;

            height += EditorGUIUtility.singleLineHeight + spacing;

            SerializedProperty iterator = property.Copy();
            SerializedProperty endProp = iterator.GetEndProperty();
            bool enterChildren = true;

            while (iterator.NextVisible(enterChildren) && !SerializedProperty.EqualContents(iterator, endProp))
            {
                if (iterator.name == GetNameField() || iterator.name == "Priority")
                    continue;

                height += EditorGUI.GetPropertyHeight(iterator, true) + spacing;
                enterChildren = false;
            }

            return height;
        }

        private bool IsParentValid(SerializedProperty property)
        {
            string[] pathParts = property.propertyPath.Split('.');
            if (pathParts.Length < 2)
                return true;

            string parentPath = string.Join(".", pathParts, 0, pathParts.Length - 1);
            SerializedProperty parentProp = property.serializedObject.FindProperty(parentPath);

            while (parentProp != null)
            {
                SerializedProperty parentName = parentProp.FindPropertyRelative(GetNameField());
                if (parentName != null && string.IsNullOrEmpty(parentName.stringValue))
                    return false;

                pathParts = parentPath.Split('.');
                if (pathParts.Length < 2)
                    break;
                parentPath = string.Join(".", pathParts, 0, pathParts.Length - 1);
                parentProp = property.serializedObject.FindProperty(parentPath);
            }

            return true;
        }
    }
}
