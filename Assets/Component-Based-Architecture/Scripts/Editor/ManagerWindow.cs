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

            DrawHeader("Controllers");

            // Display list of controllers.
            for (int i = 0; i < controllers.Count; i++)
            {
                bool toggle = i % 2 == 0;
                DrawBackground(toggle, () => DrawCategory(controllers[i], toggle));
            }

            // When adding a new controller, generate a unique controller name, add a default group and a default component.
            GUIX.DrawListButtons(
                controllers,
                () =>
                {
                    _hasChanges = true;
                    // Create a new controller with a unique name.
                    var newController = new ControllerNode
                    {
                        controllerName = GetUniqueControllerName(),
                        groups = new List<GroupNode>()
                    };

                    // Create a new default group with a unique group name.
                    var newGroup = new GroupNode
                    {
                        groupName = GetUniqueGroupName(newController),
                        components = new List<ComponentNode>()
                    };

                    // Add a default component to the new group using a unique component name.
                    newGroup.components.Add(new ComponentNode
                    {
                        componentName = GetUniqueComponentName(newGroup)
                    });

                    // Add the default group (with its default component) to the new controller.
                    newController.groups.Add(newGroup);

                    return newController;
                });

            EditorGUILayout.EndVertical();

            // Display the Save button only if there are unsaved changes.
            if (_hasChanges)
            {
                if (GUILayout.Button("Save"))
                {
                    provider.Save(controllers);
                    _hasChanges = false;
                }
            }
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
                        });

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
                        });

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
