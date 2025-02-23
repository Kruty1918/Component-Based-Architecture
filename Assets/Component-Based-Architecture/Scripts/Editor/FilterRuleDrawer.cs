using UnityEditor;
using UnityEngine;
using SGS29.ComponentBasedArchitecture;

namespace SGS29.Editor
{
    [CustomPropertyDrawer(typeof(FilterRule))]
    public class FilterRuleDrawer : PropertyDrawer
    {
        private FilterRuleContextMenu _contextMenu = new FilterRuleContextMenu();
        private FilterRuleHeightCalculator _heightCalculator = new FilterRuleHeightCalculator();

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property == null)
            {
                Debug.LogError("FilterRuleDrawer: property is null.");
                return;
            }

            EditorGUI.BeginProperty(position, label, property);

            SerializedProperty groupNameProperty = property.FindPropertyRelative("GroupName");
            SerializedProperty priorityProperty = property.FindPropertyRelative("Priority");

            float yOffset = position.y;

            // Малюємо заголовок
            EditorGUI.LabelField(new Rect(position.x, yOffset, EditorGUIUtility.labelWidth, EditorGUIUtility.singleLineHeight), label);

            // Кнопка вибору правила
            if (groupNameProperty != null)
            {
                Rect dropdownRect = new Rect(position.x + EditorGUIUtility.labelWidth, yOffset, position.width - EditorGUIUtility.labelWidth, EditorGUIUtility.singleLineHeight);
                string currentRuleName = string.IsNullOrEmpty(groupNameProperty.stringValue) ? "Невідомо" : groupNameProperty.stringValue;

                if (EditorGUI.DropdownButton(dropdownRect, new GUIContent(currentRuleName), FocusType.Keyboard))
                {
                    _contextMenu.Show(groupNameProperty);
                }

                yOffset += EditorGUIUtility.singleLineHeight + 2;
            }

            // Малюємо GroupName
            if (groupNameProperty != null)
            {
                Rect groupNameRect = new Rect(position.x, yOffset, position.width, EditorGUI.GetPropertyHeight(groupNameProperty));
                EditorGUI.PropertyField(groupNameRect, groupNameProperty);
                yOffset += EditorGUI.GetPropertyHeight(groupNameProperty) + 2;
            }

            // Малюємо Priority
            if (priorityProperty != null)
            {
                Rect priorityRect = new Rect(position.x, yOffset, position.width, EditorGUI.GetPropertyHeight(priorityProperty));
                EditorGUI.PropertyField(priorityRect, priorityProperty);
            }

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return _heightCalculator.CalculateHeight(property);
        }
    }
}
