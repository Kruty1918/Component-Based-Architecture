using UnityEditor;
using UnityEngine;
using SGS29.CBA;
using System.Collections.Generic;

namespace SGS29.Editor
{
    [CustomPropertyDrawer(typeof(ControllerFilterRule))]
    public class ControllerFilterRuleDrawer : BaseFilterRuleDrawer
    {
        public const string CONTROLLER_NAME_KEY = "CONTROLLER_Name";

        protected override string GetNameKey()
        {
            return CONTROLLER_NAME_KEY;
        }

        protected override string GetNameField()
        {
            return "Name";
        }

        protected override void PopulateMenu(GenericMenu menu, SerializedProperty nameProperty)
        {
            List<string> controllerNames = new List<string>();
            controllerNames = CBAFilterManager.GetControllerNames();

            if (controllerNames.Count <= 0)
            {
                menu.AddDisabledItem(new GUIContent("No controllers available"));
                return;
            }

            foreach (var _name in controllerNames)
            {
                // Наприклад, для цього класу логіка отримання меню може бути іншою
                // Ми можемо додати свої пункти меню
                menu.AddItem(new GUIContent(_name), false, () =>
                {
                    nameProperty.stringValue = _name;
                    nameProperty.serializedObject.ApplyModifiedProperties();
                    GUIX.ForceRebuild();
                });
            }
        }
    }
}