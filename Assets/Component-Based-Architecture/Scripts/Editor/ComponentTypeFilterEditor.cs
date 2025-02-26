using UnityEditor;
using UnityEngine;
using SGS29.CBA;

namespace SGS29.Editor
{
    [CustomEditor(typeof(ComponentTypeFilter))]
    public class ComponentTypeFilterEditor : UnityEditor.Editor
    {
        private SerializedProperty _rulesProperty;
        private ComponentTypeFilter filter;

        private void OnEnable()
        {
            _rulesProperty = serializedObject.FindProperty("rule");
            filter = (ComponentTypeFilter)target;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            // Заголовок для правил
            EditorGUILayout.LabelField("Правила фільтрації", EditorStyles.boldLabel);

            // Примусово розгортаємо властивість rule
            _rulesProperty.isExpanded = true;
            EditorGUILayout.PropertyField(_rulesProperty, new GUIContent("Список правил"), true);

            serializedObject.ApplyModifiedProperties();
        }

    }
}
