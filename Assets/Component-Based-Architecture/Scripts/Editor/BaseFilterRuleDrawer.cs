using UnityEditor;
using UnityEngine;

namespace SGS29.Editor
{
    public abstract class BaseFilterRuleDrawer : PropertyDrawer
    {
        /// <summary>
        /// Повертає ім'я властивості, яку потрібно використовувати для кнопки (наприклад, "Name" або "ComponentName").
        /// </summary>
        protected abstract string GetNameField();

        /// <summary>
        /// Заповнює меню GenericMenu пунктами для вибору.
        /// Виклик буде відрізнятися для різних типів, тому цей метод абстрактний.
        /// </summary>
        protected abstract void PopulateMenu(GenericMenu menu, SerializedProperty nameProperty);

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            float lineHeight = EditorGUIUtility.singleLineHeight;
            float spacing = EditorGUIUtility.standardVerticalSpacing;
            float y = position.y;

            // Отримуємо потрібну властивість для кнопки (наприклад, Name чи ComponentName)
            SerializedProperty nameProp = property.FindPropertyRelative(GetNameField());
            if (nameProp == null)
            {
                EditorGUI.HelpBox(new Rect(position.x, y, position.width, lineHeight * 2), "Помилка: Не знайдено властивість " + GetNameField(), MessageType.Error);
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

            // Малюємо поле "Priority"
            SerializedProperty priorityProp = property.FindPropertyRelative("Priority");
            if (priorityProp == null)
            {
                EditorGUI.HelpBox(new Rect(position.x, y, position.width, lineHeight * 2), "Помилка: Властивість 'Priority' не знайдено", MessageType.Error);
                return;
            }

            Rect priorityRect = new Rect(position.x, y, position.width, lineHeight);
            EditorGUI.PropertyField(priorityRect, priorityProp, new GUIContent("Priority", "Задайте рівень пріоритету для правила"));

            y += lineHeight + spacing;

            // Малюємо дочірні властивості, окрім тих, що вже намальовані
            SerializedProperty iterator = property.Copy();
            SerializedProperty endProp = iterator.GetEndProperty();
            bool enterChildren = true;

            while (iterator.NextVisible(enterChildren) && !SerializedProperty.EqualContents(iterator, endProp))
            {
                if (iterator.name == GetNameField() || iterator.name == "Priority")
                    continue;

                float propertyHeight = EditorGUI.GetPropertyHeight(iterator, true);
                Rect propRect = new Rect(position.x, y, position.width, propertyHeight);
                EditorGUI.PropertyField(propRect, iterator, new GUIContent(iterator.displayName, "Значення: " + iterator.name), true);

                y += propertyHeight + spacing;
                enterChildren = false;
            }

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            float height = 0f;
            float spacing = EditorGUIUtility.standardVerticalSpacing * 2;

            SerializedProperty nameProp = property.FindPropertyRelative(GetNameField());
            SerializedProperty priorityProp = property.FindPropertyRelative("Priority");

            if (nameProp == null || priorityProp == null)
            {
                return EditorGUIUtility.singleLineHeight * 2 + spacing;
            }

            height += EditorGUI.GetPropertyHeight(nameProp) + spacing;
            height += EditorGUI.GetPropertyHeight(priorityProp) + spacing;

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
    }
}
