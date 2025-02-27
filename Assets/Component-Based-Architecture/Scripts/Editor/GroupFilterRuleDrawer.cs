using UnityEditor;
using UnityEngine;
using SGS29.CBA;
using System.Collections.Generic;
using System;

namespace SGS29.Editor
{
    [CustomPropertyDrawer(typeof(GroupFilterRule))]
    public class GroupFilterRuleDrawer : BaseFilterRuleDrawer
    {
        public const string GROUP_NAME_KEY = "GROUP_Name";

        protected override string GetNameKey()
        {
            return GROUP_NAME_KEY;
        }


        protected override string GetNameField()
        {
            return "Name";
        }

        protected override void PopulateMenu(GenericMenu menu, SerializedProperty nameProperty)
        {
            List<string> groupNames = new List<string>();
            string controllerName = GetName(ControllerFilterRuleDrawer.CONTROLLER_NAME_KEY);
            groupNames = CBAFilterManager.GetGroupNames(controllerName);

            if (groupNames.Count <= 0)
            {
                menu.AddDisabledItem(new GUIContent("No groups available"));
                return;
            }

            foreach (var _name in groupNames)
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