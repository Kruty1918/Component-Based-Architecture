using UnityEditor;

namespace SGS29.Editor
{
    /// <summary>
    /// Відповідає за обчислення висоти елемента у редакторі
    /// </summary>
    public class FilterRuleHeightCalculator
    {
        public float CalculateHeight(SerializedProperty property)
        {
            float height = EditorGUIUtility.singleLineHeight + 2; // Висота dropdown
            SerializedProperty groupNameProperty = property.FindPropertyRelative("GroupName");
            SerializedProperty priorityProperty = property.FindPropertyRelative("Priority");

            if (groupNameProperty != null)
                height += EditorGUI.GetPropertyHeight(groupNameProperty) + 2;

            if (priorityProperty != null)
                height += EditorGUI.GetPropertyHeight(priorityProperty) + 2;

            return height;
        }
    }
}
