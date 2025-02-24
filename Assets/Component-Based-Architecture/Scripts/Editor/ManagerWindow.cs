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

            // Малюємо foldout для контролера
            foldoutStates[controller] = DrawFoldout(controller, controller.controllerName, foldoutStyle);

            if (foldoutStates[controller])
            {
                // Рядок для редагування імені контролера
                controller.controllerName = DrawNameField("Name", controller.controllerName, 50, 200);

                // Відображаємо групи, що належать цьому контролеру
                foreach (var group in controller.groups)
                {
                    DrawGroup(group);
                }

                GroupNode newNode = new GroupNode();
                newNode.groupName = "New Group";
                DrawComponentButtons(controller.groups, () => newNode, indent: 20, buttonSize: 20);
            }

            EditorGUILayout.EndVertical();
        }

        private void DrawGroup(GroupNode group)
        {
            DrawLine(); // Розділююча лінія

            // Налаштування відступів та розмірів
            float rightMargin = 20f;
            float labelWidth = 40f;
            float inputWidth = 200f;
            float totalWidth = treeWidth - 50; // Враховуємо відступи

            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(rightMargin);

            GUIStyle groupStyle = UIStyles.ItalicFoldoutStyle();

            EditorGUILayout.BeginVertical("box", GUILayout.Width(totalWidth));

            // Малюємо foldout для групи
            foldoutStates[group] = DrawFoldout(group, group.groupName, groupStyle);

            if (foldoutStates[group])
            {
                // Рядок для редагування назви групи з додатковим відступом
                group.groupName = DrawNameField("Name", group.groupName, labelWidth, inputWidth - rightMargin, extraIndent: 10);

                // Відображення списку компонентів
                DrawComponentList(group.components, indent: rightMargin, width: inputWidth);

                DrawLine();

                // Відображення кнопок додавання/видалення компонентів
                DrawComponentButtons(group.components, () => "New Component", indent: rightMargin, buttonSize: 20);
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

        // Допоміжний метод для відображення foldout (ініціалізує стан та повертає оновлений)
        private bool DrawFoldout<T>(T node, string label, GUIStyle style)
        {
            if (!foldoutStates.ContainsKey(node))
                foldoutStates[node] = true;

            return EditorGUI.Foldout(EditorGUILayout.GetControlRect(), foldoutStates[node], label, true, style);
        }

        // Метод для відображення поля редагування імені
        private string DrawNameField(string label, string value, float labelWidth = 50, float textFieldWidth = 200, float extraIndent = 0)
        {
            EditorGUILayout.BeginHorizontal();
            if (extraIndent > 0) GUILayout.Space(extraIndent);
            GUILayout.Label(label, GUILayout.Width(labelWidth));
            string newValue = EditorGUILayout.TextField(value, GUILayout.Width(textFieldWidth));
            EditorGUILayout.EndHorizontal();
            return newValue;
        }

        // Метод для відображення списку елементів (універсальний)
        private void DrawComponentList(List<string> components, float indent = 20f, float width = 200f)
        {
            for (int i = 0; i < components.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();
                GUILayout.Space(indent);
                components[i] = EditorGUILayout.TextField(components[i], GUILayout.Width(width - 60)); // Тепер можна редагувати імена
                EditorGUILayout.EndHorizontal();
            }
        }

        // Метод для відображення кнопок додавання/видалення елементів
        private void DrawComponentButtons<T>(List<T> list, System.Func<T> createNewItem, float indent = 20f, float buttonSize = 20f)
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(indent);

            GUIStyle iconButtonStyle = new GUIStyle(GUI.skin.button)
            {
                normal = { background = UIStyles.MakeTex((int)buttonSize, (int)buttonSize, Color.clear) },
                padding = new RectOffset(0, 0, 0, 0),
                alignment = TextAnchor.MiddleCenter
            };

            // Видалення останнього елемента, якщо є що видаляти
            if (list.Count > 0)
            {
                GUIContent removeIcon = EditorGUIUtility.IconContent("d_Collab.FileDeleted");
                if (GUILayout.Button(removeIcon, iconButtonStyle, GUILayout.Width(buttonSize), GUILayout.Height(buttonSize)))
                {
                    list.RemoveAt(list.Count - 1);
                }
            }

            // Додавання нового елемента
            GUIContent addIcon = EditorGUIUtility.IconContent("d_Collab.FileAdded");
            if (GUILayout.Button(addIcon, iconButtonStyle, GUILayout.Width(buttonSize), GUILayout.Height(buttonSize)))
            {
                list.Add(createNewItem());
            }

            EditorGUILayout.EndHorizontal();
        }
    }
}