using UnityEditor;
using SGS29.CBA;
using UnityEngine;
using System.Collections.Generic;

namespace SGS29.Editor
{
    [CustomPropertyDrawer(typeof(ComponentFilterRule))]
    public class ComponentFilterRuleDrawer : BaseFilterRuleDrawer
    {
        protected override string GetNameField()
        {
            return "ComponentName";
        }

        protected override void PopulateMenu(GenericMenu menu, SerializedProperty nameProperty)
        {
            List<string> componentTypes = TypeFinder.GetDerivedTypeNames();
            string controllerName = GetName(ControllerFilterRuleDrawer.CONTROLLER_NAME_KEY);
            List<string> controllerComponents = CBAFilterManager.GetControllerComponentsPath(controllerName);

            if (componentTypes.Count == 0)
            {
                menu.AddDisabledItem(new GUIContent("No components available"));
                return;
            }

            if (controllerComponents == null || controllerComponents.Count == 0)
            {
                menu.AddDisabledItem(new GUIContent("Unidentified"));
                return;
            }

            bool addedItem = false;
            foreach (string type in componentTypes)
            {
                if (controllerComponents.Contains(type))
                {
                    menu.AddItem(new GUIContent(type), false, () => ApplySelection(nameProperty, type));
                    addedItem = true;
                }
            }

            if (!addedItem)
            {
                menu.AddDisabledItem(new GUIContent("No objects matching the filter"));
            }
        }

        private void ApplySelection(SerializedProperty nameProperty, string value)
        {
            nameProperty.stringValue = value;
            nameProperty.serializedObject.ApplyModifiedProperties();
            nameProperty.serializedObject.Update();
            GUIX.ForceRebuild();
        }
    }
}
