using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace SGS29.Editor
{
    public class ManagerWindow : ScriptableObject
    {
        private static ManagerWindow instance;
        private List<ControllerNode> controllers = new List<ControllerNode>();
        private ICBADataProvider<List<ControllerNode>> provider;
        private IFoldout foldout;

        [SettingsProvider]
        public static SettingsProvider CreateComponentBaseArchitectureSettings()
        {
            return new SettingsProvider("Project/Component Base Architecture", SettingsScope.Project)
            {
                guiHandler = searchContext =>
                {
                    if (instance == null)
                    {
                        instance = CreateInstance<ManagerWindow>();
                        instance.Initialize();
                    }
                    instance.OnGUI();
                }
            };
        }

        private void Initialize()
        {
            provider = new CBADataProvider();
            foldout = new FoldoutManager();
            controllers = provider.Load();
        }

        private void OnGUI()
        {
            EditorGUILayout.BeginVertical();

            // Горизонтальна панель для заголовка
            EditorGUILayout.BeginHorizontal("toolbar");
            GUILayout.Label("Controllers");
            EditorGUILayout.EndHorizontal();

            // Відображення списку контролерів в контейнері "box"
            EditorGUILayout.BeginVertical("box");
            foreach (ControllerNode controller in controllers)
            {
                DrawCategory(controller);
            }
            EditorGUILayout.EndVertical();

            EditorGUILayout.EndVertical();

            // Кнопка збереження
            if (GUILayout.Button("Save"))
            {
                provider.Save(controllers);
            }
        }

        private void DrawCategory(ControllerNode controller)
        {
            EditorGUILayout.BeginVertical("box");

            // Використовуємо foldout для категорії контролера
            foldout.Draw(controller.controllerName, controller.controllerName, () =>
            {
                EditorGUI.indentLevel++; // Збільшуємо відступ для вмісту

                // Редагування назви контролера
                controller.controllerName = EditorGUILayout.TextField(controller.controllerName);

                // Відображення груп всередині контролера
                foreach (GroupNode group in controller.groups)
                {
                    DrawGroup(group);
                }

                EditorGUI.indentLevel--;
            });

            EditorGUILayout.EndVertical();
        }

        private void DrawGroup(GroupNode group)
        {
            EditorGUILayout.BeginVertical();
            GUILayout.Space(10);  // Невеликий відступ перед назвою групи

            // Використовуємо foldout для групи
            foldout.Draw(group.groupName, group.groupName, () =>
            {
                EditorGUI.indentLevel++; // Збільшуємо відступ для вмісту групи

                // Редагування назви групи
                group.groupName = EditorGUILayout.TextField(group.groupName);

                // Відображення компонентів всередині групи
                foreach (ComponentNode component in group.components)
                {
                    DrawComponent(component);
                }

                EditorGUI.indentLevel--;
            });

            EditorGUILayout.EndVertical();
        }

        public void DrawComponent(ComponentNode component)
        {
            // Відображення компонента
            EditorGUILayout.LabelField(component.componentName);
        }
    }
}
