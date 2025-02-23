using UnityEditor;
using UnityEngine;

namespace SGS29.Editor
{
    // Створення нового налаштування для вкладки
    public class ManagerWindow : ScriptableObject
    {
        [SettingsProvider]
        public static SettingsProvider CreateComponentBaseArchitectureSettings()
        {
            // Створюємо нову вкладку в ProjectSettings
            return new SettingsProvider("Project/Component Base Architecture", SettingsScope.Project)
            {
                guiHandler = (searchContext) =>
                {
                    // Рендеримо інтерфейс налаштувань
                    GUILayout.Label("Component Base Architecture Settings", EditorStyles.boldLabel);

                    // Тут можна додавати інші елементи налаштувань
                    EditorGUILayout.Space();
                    EditorGUILayout.LabelField("Your custom settings go here.");
                }
            };
        }
    }
}
