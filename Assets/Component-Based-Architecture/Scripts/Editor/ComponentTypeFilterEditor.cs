using UnityEditor;
using UnityEngine;
using SGS29.CBA;

namespace SGS29.Editor
{
    [CustomEditor(typeof(ComponentTypeFilter))]
    public class ComponentTypeFilterEditor : UnityEditor.Editor
    {
        private SerializedProperty _controllerNameProperty;
        private SerializedProperty _rulesProperty;

        private void OnEnable()
        {
            _controllerNameProperty = serializedObject.FindProperty("controllerName");
            _rulesProperty = serializedObject.FindProperty("_rules");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            // Поле для назви контролера
            EditorGUILayout.PropertyField(_controllerNameProperty);

            // Відображення списку правил
            EditorGUILayout.LabelField("Правила фільтрації", EditorStyles.boldLabel);

            // Малюємо список правил з коригуванням висоти
            EditorGUILayout.PropertyField(_rulesProperty, new GUIContent("Список правил"), true);

            // Додаємо кнопки для керування списком правил
            DrawRuleListButtons();

            serializedObject.ApplyModifiedProperties();
        }


        private void DrawRuleListButtons()
        {
            if (GUILayout.Button("Додати правило"))
            {
                _rulesProperty.arraySize++;
            }

            if (GUILayout.Button("Видалити останнє правило") && _rulesProperty.arraySize > 0)
            {
                _rulesProperty.arraySize--;
            }
        }
    }
}
