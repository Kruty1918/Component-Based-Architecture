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
        private const float treeWidth = 350;
        private IFoldout foldout;
        private ICBADataProvider<List<ControllerNode>> provider;

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
                        instance.foldout = new FoldoutManager();
                        instance.provider = new CBADataProvider();
                        instance.controllers = instance.provider.Load();
                    }
                    instance.OnGUI();
                }
            };
        }

        private void OnGUI()
        {
            using (var scroll = new EditorGUILayout.ScrollViewScope(scrollPosition))
            {
                scrollPosition = scroll.scrollPosition;
                DrawControllersList();
            }

            if (GUILayout.Button("Save"))
            {
                provider.Save(controllers);
            }
        }

        private void DrawControllersList()
        {
            foreach (var controller in controllers)
            {
                DrawController(controller);
            }

            DrawAddGroupController(controllers);
        }

        private void DrawController(ControllerNode controller)
        {
            using (new EditorGUILayout.VerticalScope(UIStyles.BoxStyle(), GUILayout.Width(treeWidth)))
            {
                foldout.Draw(controller.controllerName, controller.controllerName, () =>
                {
                    controller.controllerName = GUIX.DrawNameField("Name", controller.controllerName, 50, 200);
                    foreach (var group in controller.groups)
                    {
                        DrawGroup(group);
                    }
                    DrawAddGroupButton(controller.groups);
                });
            }
        }

        private void DrawGroup(GroupNode group)
        {
            DrawLine();

            using (new EditorGUILayout.HorizontalScope())
            {
                GUILayout.Space(20);
                using (new EditorGUILayout.VerticalScope("box", GUILayout.Width(treeWidth - 50)))
                {
                    foldout.Draw(group.groupName, group.groupName, () =>
                    {
                        group.groupName = GUIX.DrawNameField("Name", group.groupName, 40, 200, 10);
                        DrawComponentList(group.components);
                        DrawLine();
                        DrawAddGroupComponent(group.components);
                    });
                }
            }
        }

        private void DrawComponentList(List<string> components)
        {
            for (int i = 0; i < components.Count; i++)
            {
                string newValue = components[i];
                DrawComponentField(ref newValue);
                components[i] = newValue;
            }

        }

        private void DrawComponentField(ref string component)
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                GUILayout.Space(20);
                component = EditorGUILayout.TextField(component, GUILayout.Width(200));
            }
        }

        private void DrawLine(float width = 0, float height = 0.5f) => GUIX.DrawLine(treeWidth, height, width);

        private void DrawComponentButtons<T>(List<T> list, System.Func<T> createNewItem, float buttonSize = 20f)
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                GUILayout.Space(20);

                if (list.Count > 0)
                {
                    // Використовуємо DrawButtonWithIcon для кнопки видалення
                    GUIX.DrawButtonWithIcon("d_Collab.FileDeleted", () => list.RemoveAt(list.Count - 1), buttonSize);
                }

                // Використовуємо DrawButtonWithIcon для кнопки додавання
                GUIX.DrawButtonWithIcon("d_Collab.FileAdded", () => list.Add(createNewItem()), buttonSize);
            }
        }

        private void DrawAddGroupController(List<ControllerNode> controllers)
        {
            DrawComponentButtons(controllers, () => new ControllerNode { controllerName = "New Controller" });
        }

        private void DrawAddGroupButton(List<GroupNode> groups)
        {
            DrawComponentButtons(groups, () => new GroupNode { groupName = "New Group" });
        }

        private void DrawAddGroupComponent(List<string> components)
        {
            DrawComponentButtons(components, () => "New Component");
        }
    }
}