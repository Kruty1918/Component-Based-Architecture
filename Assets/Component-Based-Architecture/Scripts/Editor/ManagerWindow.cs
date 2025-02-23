using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace SGS29.Editor
{
    public class ManagerWindow : ScriptableObject
    {
        private static ManagerWindow instance;
        private Vector2 scrollPosition;
        private List<ControllerNode> controllers = new List<ControllerNode>();
        private static Dictionary<object, bool> foldoutStates = new Dictionary<object, bool>();
        private const float treeWidth = 350;

        [SettingsProvider]
        public static SettingsProvider CreateComponentBaseArchitectureSettings()
        {
            return new SettingsProvider("Project/Component Base Architecture", SettingsScope.Project)
            {
                guiHandler = (searchContext) =>
                {
                    if (instance == null)
                    {
                        instance = CreateInstance<ManagerWindow>();
                        instance.LoadData();
                    }

                    instance.OnGUI();
                }
            };
        }

        private void LoadData()
        {
            controllers = CBAReaderWriter.Read();
            if (controllers == null || controllers.Count == 0)
            {
                controllers = new List<ControllerNode>
                {
                    new ControllerNode
                    {
                        controllerName = "MainController",
                        groups = new List<GroupNode>
                        {
                            new GroupNode { groupName = "Group 1", components = new List<string> { "Component1", "Component2" } },
                            new GroupNode { groupName = "Group 2", components = new List<string> { "Component3" } }
                        }
                    }
                };
            }
        }

        private void OnGUI()
        {
            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
            foreach (var controller in controllers)
            {
                DrawController(controller);
            }
            EditorGUILayout.EndScrollView();

            if (GUILayout.Button("Save"))
            {
                CBAReaderWriter.Write(controllers);
                AssetDatabase.Refresh();
            }
        }

        private void DrawController(ControllerNode controller)
        {
            GUIStyle foldoutStyle = UIStyles.BoldFoldoutStyle();
            GUIStyle boxStyle = UIStyles.BoxStyle();

            EditorGUILayout.BeginVertical(boxStyle, GUILayout.Width(treeWidth));

            if (!foldoutStates.ContainsKey(controller))
                foldoutStates[controller] = true;

            foldoutStates[controller] = EditorGUI.Foldout(EditorGUILayout.GetControlRect(), foldoutStates[controller], controller.controllerName, true, foldoutStyle);

            if (foldoutStates[controller])
            {
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Name", GUILayout.Width(50));
                controller.controllerName = EditorGUILayout.TextField(controller.controllerName, GUILayout.Width(200));
                EditorGUILayout.EndHorizontal();

                foreach (var group in controller.groups)
                {
                    DrawGroup(group);
                }
            }

            EditorGUILayout.EndVertical();
        }

        private void DrawGroup(GroupNode group)
        {
            DrawLine();
            // Відступи та розміри
            float rightMargin = 20f;
            float labelWidth = 40f;
            float inputWidth = 200f;
            float totalWidth = treeWidth - 50; // Враховуємо відступи

            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(rightMargin);

            // Стиль для заголовку групи
            GUIStyle groupStyle = UIStyles.ItalicFoldoutStyle();

            EditorGUILayout.BeginVertical("box", GUILayout.Width(totalWidth));

            // Якщо стан розкриття для групи ще не збережено – встановлюємо true
            if (!foldoutStates.ContainsKey(group))
                foldoutStates[group] = true;

            // Заголовок групи з можливістю розкриття
            foldoutStates[group] = EditorGUI.Foldout(EditorGUILayout.GetControlRect(), foldoutStates[group], group.groupName, true, groupStyle);

            if (foldoutStates[group])
            {
                // Рядок редагування назви групи
                EditorGUILayout.BeginHorizontal();
                GUILayout.Space(10);
                GUILayout.Label("Name", GUILayout.Width(labelWidth));
                group.groupName = EditorGUILayout.TextField(group.groupName, GUILayout.Width(inputWidth - rightMargin));
                EditorGUILayout.EndHorizontal();

                float buttonSize = 20;

                // Стиль для кнопки без фону
                GUIStyle iconButtonStyle = new GUIStyle(GUI.skin.button)
                {
                    normal = { background = UIStyles.MakeTex((int)buttonSize, (int)buttonSize, Color.clear) },  // Прибираємо фон кнопки, встановлюємо колір тексту
                    focused = { background = null }, // Прибираємо фон при фокусі
                    hover = { background = null },   // Прибираємо фон при наведенні
                    active = { background = null },  // Прибираємо фон при натисканні
                    padding = new RectOffset(0, 0, 0, 0),  // Вимикаємо відступи
                    margin = new RectOffset(0, 0, 0, 0),   // Вимикаємо маргін
                    alignment = TextAnchor.MiddleCenter // Вирівнюємо іконку по центру
                };

                // Відображення списку компонентів – для кожного компоненту текст і кнопка видалення в одному рядку
                for (int i = 0; i < group.components.Count; i++)
                {
                    EditorGUILayout.BeginHorizontal();
                    GUILayout.Space(rightMargin);
                    // Розраховуємо ширину для тексту (віднімаємо місце під кнопку)
                    float textWidth = inputWidth - 60;
                    EditorGUILayout.LabelField("- " + group.components[i], GUILayout.Width(textWidth));
                    EditorGUILayout.EndHorizontal();
                }

                DrawLine();

                // Відображення кнопок в одному рядку: видалення зліва і додавання праворуч
                EditorGUILayout.BeginHorizontal();

                // Відступ для вирівнювання
                GUILayout.Space(rightMargin);

                if (group.components.Count > 0)
                {
                    // Кнопка видалення для цього компоненту (іконка без фону)
                    GUIContent removeIcon = EditorGUIUtility.IconContent("d_Collab.FileDeleted");
                    if (GUILayout.Button(removeIcon, iconButtonStyle, GUILayout.Width(buttonSize), GUILayout.Height(buttonSize)))
                    {
                        group.components.Remove(group.components[^1]);
                        // Після видалення список змінюється, можна припинити виконання
                    }
                }

                // Кнопка додавання нового компоненту (іконка додавання поруч із кнопкою видалення)
                GUIContent addIcon = EditorGUIUtility.IconContent("d_Collab.FileAdded");
                if (GUILayout.Button(addIcon, iconButtonStyle, GUILayout.Width(buttonSize), GUILayout.Height(buttonSize)))
                {
                    group.components.Add("New Component");
                }

                // Завершення горизонтального контейнера
                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();
        }

        private void DrawLine(float width = 0, float height = 0.5f)
        {
            GUILayout.Space(5);
            EditorGUILayout.BeginHorizontal();

            // Визначаємо ширину лінії, якщо не вказано, використовуємо доступну ширину
            float lineWidth = width == 0 ? treeWidth : width; // використовуємо treeWidth як ширину за замовчуванням

            // Створюємо область для лінії з вказаними шириною та висотою
            Rect rect = GUILayoutUtility.GetRect(lineWidth, height); // Додаємо параметр ширини
            Color lighterColor = Color.Lerp(UIStyles.BOX_COLOR, Color.white, 0.12f);

            EditorGUI.DrawRect(rect, lighterColor); // Малюємо чорну лінію
            EditorGUILayout.EndHorizontal();
            GUILayout.Space(5);
        }

    }
}