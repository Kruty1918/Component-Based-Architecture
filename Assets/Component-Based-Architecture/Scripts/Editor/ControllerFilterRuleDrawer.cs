using UnityEditor;
using UnityEngine;
using SGS29.CBA;

namespace SGS29.Editor
{
    [CustomPropertyDrawer(typeof(ControllerFilterRule))]
    public class ControllerFilterRuleDrawer : BaseFilterRuleDrawer
    {
        protected override string GetNameField()
        {
            return "Name";
        }

        protected override void PopulateMenu(GenericMenu menu, SerializedProperty nameProperty)
        {
            // Наприклад, для цього класу логіка отримання меню може бути іншою
            // Ми можемо додати свої пункти меню
            menu.AddItem(new GUIContent("CustomComponent1"), false, () =>
            {
                nameProperty.stringValue = "CustomComponent1";
                nameProperty.serializedObject.ApplyModifiedProperties();
                GUIX.ForceRebuild();
            });
            menu.AddItem(new GUIContent("CustomComponent2"), false, () =>
            {
                nameProperty.stringValue = "CustomComponent2";
                nameProperty.serializedObject.ApplyModifiedProperties();
                GUIX.ForceRebuild();
            });
        }
    }
}