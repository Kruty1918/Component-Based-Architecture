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
        private static Dictionary<ControllerNode, bool> controllerFoldoutStates = new Dictionary<ControllerNode, bool>();
        private static Dictionary<GroupNode, bool> groupFoldoutStates = new Dictionary<GroupNode, bool>();
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

            if (!controllerFoldoutStates.ContainsKey(controller))
                controllerFoldoutStates[controller] = true;

            controllerFoldoutStates[controller] = EditorGUI.Foldout(EditorGUILayout.GetControlRect(), controllerFoldoutStates[controller], controller.controllerName, true, foldoutStyle);

            if (controllerFoldoutStates[controller])
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
            float rightMargin = 20f;
            float labelWidth = 40f;
            float inputWidth = 200f;
            float totalWidth = treeWidth - 50;

            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(rightMargin);

            GUIStyle groupStyle = UIStyles.ItalicFoldoutStyle();

            EditorGUILayout.BeginVertical("box", GUILayout.Width(totalWidth));

            if (!groupFoldoutStates.ContainsKey(group))
                groupFoldoutStates[group] = true;

            groupFoldoutStates[group] = EditorGUI.Foldout(EditorGUILayout.GetControlRect(), groupFoldoutStates[group], group.groupName, true, groupStyle);

            if (groupFoldoutStates[group])
            {
                EditorGUILayout.BeginHorizontal();
                GUILayout.Space(10);
                GUILayout.Label("Name", GUILayout.Width(labelWidth));
                group.groupName = EditorGUILayout.TextField(group.groupName, GUILayout.Width(inputWidth - rightMargin));
                EditorGUILayout.EndHorizontal();

                float buttonSize = 20;
                GUIStyle iconButtonStyle = new GUIStyle(GUI.skin.button)
                {
                    normal = { background = UIStyles.MakeTex((int)buttonSize, (int)buttonSize, Color.clear) },
                    focused = { background = null },
                    hover = { background = null },
                    active = { background = null },
                    padding = new RectOffset(0, 0, 0, 0),
                    margin = new RectOffset(0, 0, 0, 0),
                    alignment = TextAnchor.MiddleCenter
                };

                for (int i = 0; i < group.components.Count; i++)
                {
                    EditorGUILayout.BeginHorizontal();
                    GUILayout.Space(rightMargin);
                    float textWidth = inputWidth - 60;
                    EditorGUILayout.LabelField("- " + group.components[i], GUILayout.Width(textWidth));
                    EditorGUILayout.EndHorizontal();
                }
                DrawLine();

                EditorGUILayout.BeginHorizontal();
                GUILayout.Space(rightMargin);

                if (group.components.Count > 0)
                {
                    GUIContent removeIcon = EditorGUIUtility.IconContent("d_Collab.FileDeleted");
                    if (GUILayout.Button(removeIcon, iconButtonStyle, GUILayout.Width(buttonSize), GUILayout.Height(buttonSize)))
                    {
                        group.components.Remove(group.components[^1]);
                    }
                }

                GUIContent addIcon = EditorGUIUtility.IconContent("d_Collab.FileAdded");
                if (GUILayout.Button(addIcon, iconButtonStyle, GUILayout.Width(buttonSize), GUILayout.Height(buttonSize)))
                {
                    group.components.Add("New Component");
                }

                EditorGUILayout.EndHorizontal();

                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.EndVertical();
        }

        private void DrawLine(float width = 0, float height = 0.5f)
        {
            GUILayout.Space(5);
            EditorGUILayout.BeginHorizontal();
            float lineWidth = width == 0 ? treeWidth : width;
            Rect rect = GUILayoutUtility.GetRect(lineWidth, height);
            Color lighterColor = Color.Lerp(UIStyles.BOX_COLOR, Color.white, 0.12f);
            EditorGUI.DrawRect(rect, lighterColor);
            EditorGUILayout.EndHorizontal();
            GUILayout.Space(5);
        }
    }
}
