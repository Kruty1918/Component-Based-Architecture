using System;
using System.Collections.Generic;
using System.Linq;
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

        // Flag to indicate that there are unsaved changes.
        private bool _hasChanges = false;

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
            _hasChanges = false;
        }

        private void OnGUI()
        {
            EditorGUILayout.BeginVertical();

            // Верхній тулбар з кнопками
            DrawToolbar();

            DrawHeader("Controllers");

            // Відображення списку контролерів.
            for (int i = 0; i < controllers.Count; i++)
            {
                bool toggle = i % 2 == 0;
                DrawBackground(toggle, () => DrawCategory(controllers[i], toggle));
            }

            // Кнопки для додавання нових контролерів, груп та компонентів.
            GUIX.DrawListButtons(
                controllers,
                () =>
                {
                    _hasChanges = true;
                    var newController = new ControllerNode
                    {
                        controllerName = GetUniqueControllerName(),
                        groups = new List<GroupNode>()
                    };

                    var newGroup = new GroupNode
                    {
                        groupName = GetUniqueGroupName(newController),
                        components = new List<ComponentNode>()
                    };

                    newGroup.components.Add(new ComponentNode
                    {
                        componentName = GetUniqueComponentName(newGroup)
                    });

                    newController.groups.Add(newGroup);

                    return newController;
                },
                (ControllerNode node) => { _hasChanges = true; }
            );

            EditorGUILayout.EndVertical();
        }

        /// <summary>
        /// Малює верхній тулбар з квадратними кнопками для збереження та скасування змін.
        /// </summary>
        private void DrawToolbar()
        {
            EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);

            GUILayout.FlexibleSpace(); // Вирівнює кнопки праворуч

            GUIStyle iconSaveButtonStyle = new GUIStyle(GUI.skin.button)
            {
                fixedWidth = 45,
                fixedHeight = 18,
                padding = new RectOffset(2, 2, 2, 2) // Зменшує внутрішні відступи, щоб іконка була більшою
            };

            GUIStyle iconRevertButtonStyle = new GUIStyle(GUI.skin.button)
            {
                fixedWidth = 55,
                fixedHeight = 18,
                padding = new RectOffset(2, 2, 2, 2) // Зменшує внутрішні відступи, щоб іконка була більшою
            };

            GUI.enabled = _hasChanges; // Вмикаємо або вимикаємо кнопки в залежності від змін

            if (GUILayout.Button("Revert", iconRevertButtonStyle))
            {
                if (EditorUtility.DisplayDialog("Revert Changes", "Are you sure you want to revert unsaved changes?", "Yes", "No"))
                {
                    controllers = provider.Load();
                    _hasChanges = false;

                    GUIX.ForceRebuild();
                }
            }

            if (GUILayout.Button("Save", iconSaveButtonStyle))
            {
                provider.Save(controllers);
                _hasChanges = false;
            }

            GUI.enabled = true; // Повертаємо стандартний стан кнопок

            EditorGUILayout.EndHorizontal();
        }

        private void DrawHeader(string title)
        {
            EditorGUILayout.BeginHorizontal("toolbar");
            GUILayout.Label(title);
            EditorGUILayout.EndHorizontal();
        }

        private void DrawCategory(ControllerNode controller, bool toggle)
        {
            DrawBackground(toggle, () =>
            {
                foldout.Draw(controller.controllerName, controller.controllerName, () =>
                {
                    EditorGUI.indentLevel++;

                    // Validate the controller name for uniqueness across all controllers.
                    string updatedName = DrawValidatedTextField(
                        controller.controllerName,
                        newName => !controllers.Any(c => c != controller && c.controllerName == newName),
                        "Error",
                        "This controller name already exists."
                    );
                    if (updatedName != controller.controllerName)
                    {
                        controller.controllerName = updatedName;
                        _hasChanges = true;
                    }

                    // Draw each group within the controller.
                    for (int i = 0; i < controller.groups.Count; i++)
                    {
                        bool groupToggle = i % 2 == 0;
                        DrawBackground(groupToggle, () => DrawGroup(controller.groups[i], controller, groupToggle));
                    }

                    EditorGUI.indentLevel--;
                });
            });
        }

        private void DrawGroup(GroupNode group, ControllerNode controller, bool toggle)
        {
            DrawBackground(toggle, () =>
            {
                foldout.Draw(group.groupName, group.groupName, () =>
                {
                    EditorGUI.indentLevel++;

                    // Validate the group name within the current controller.
                    string updatedGroupName = DrawValidatedTextField(
                        group.groupName,
                        newName => !controller.groups.Any(g => g != group && g.groupName == newName),
                        "Error",
                        "This group name already exists in this controller."
                    );
                    if (updatedGroupName != group.groupName)
                    {
                        group.groupName = updatedGroupName;
                        _hasChanges = true;
                    }

                    // Draw each component in the group.
                    for (int i = 0; i < group.components.Count; i++)
                    {
                        bool compToggle = i % 2 == 0;
                        DrawBackground(compToggle, () => DrawComponent(group.components[i], group, compToggle));
                    }

                    // When adding a new component, mark changes and generate a unique component name.
                    GUIX.DrawListButtons(
                        group.components,
                        () =>
                        {
                            _hasChanges = true;
                            return new ComponentNode
                            {
                                componentName = GetUniqueComponentName(group)
                            };
                        }, (ComponentNode node) => { _hasChanges = true; });

                    // When adding a new group, mark changes and generate a unique group name.
                    GUIX.DrawListButtons(
                        controller.groups,
                        () =>
                        {
                            _hasChanges = true;
                            return new GroupNode
                            {
                                groupName = GetUniqueGroupName(controller),
                                components = new List<ComponentNode>()
                            };
                        }, (GroupNode node) => { _hasChanges = true; });

                    EditorGUI.indentLevel--;
                });
            });
        }

        private void DrawComponent(ComponentNode component, GroupNode group, bool toggle)
        {
            DrawBackground(toggle, () =>
            {
                foldout.Draw(component.componentName, component.componentName, () =>
                {
                    // Validate the component name within the group.
                    string updatedComponentName = DrawValidatedTextField(
                        component.componentName,
                        newName => !group.components.Any(c => c != component && c.componentName == newName),
                        "Error",
                        "This component name already exists in this group."
                    );
                    if (updatedComponentName != component.componentName)
                    {
                        component.componentName = updatedComponentName;
                        _hasChanges = true;
                    }
                });
            });
        }

        /// <summary>
        /// Renders a text field that only accepts a new value if it passes the uniqueness check.
        /// If the check fails, an error dialog is shown and the original value is retained.
        /// </summary>
        private string DrawValidatedTextField(
            string currentValue,
            Func<string, bool> isUnique,
            string errorTitle,
            string errorMessage)
        {
            EditorGUI.BeginChangeCheck();
            string newValue = EditorGUILayout.TextField(currentValue);
            if (EditorGUI.EndChangeCheck())
            {
                if (!isUnique(newValue))
                {
                    EditorUtility.DisplayDialog(errorTitle, errorMessage, "OK");
                    return currentValue;
                }
                if (!newValue.Equals(currentValue))
                {
                    _hasChanges = true;
                }
                return newValue;
            }
            return currentValue;
        }

        /// <summary>
        /// Draws a background rectangle with a color that depends on the toggle value.
        /// </summary>
        private void DrawBackground(bool toggle, Action content)
        {
            float multiplier = 0.2f; // Determines how much lighter the color becomes
            Color baseColor = toggle ? Color.black : Color.white;
            Color targetColor = toggle ? Color.white : Color.black;
            multiplier = toggle ? multiplier : multiplier * 3.8f;
            Color backgroundColor = Color.Lerp(baseColor, targetColor, multiplier);

            Rect rect = EditorGUILayout.BeginVertical();
            EditorGUI.DrawRect(rect, backgroundColor);
            content?.Invoke();
            EditorGUILayout.EndVertical();
        }

        private string GetUniqueControllerName()
        {
            List<string> names = controllers.Select(c => c.controllerName).ToList();
            NamesValidator validator = new NamesValidator(names);
            return validator.CreateName("Controller");
        }

        private string GetUniqueGroupName(ControllerNode controller)
        {
            List<string> names = controller.groups.Select(g => g.groupName).ToList();
            NamesValidator validator = new NamesValidator(names);
            return validator.CreateName("Group");
        }

        private string GetUniqueComponentName(GroupNode group)
        {
            List<string> names = group.components.Select(c => c.componentName).ToList();
            NamesValidator validator = new NamesValidator(names);
            return validator.CreateName("Component");
        }
    }
}
