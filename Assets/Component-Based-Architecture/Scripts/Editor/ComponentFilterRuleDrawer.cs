using UnityEditor;
using SGS29.CBA;
using UnityEngine;

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
            // Наприклад, для цього класу логіка отримання меню може бути іншою
            // Ми можемо додати свої пункти меню
            menu.AddItem(new GUIContent("CustomComponent1"), false, () =>
            {
                nameProperty.stringValue = "CustomComponent1";
                nameProperty.serializedObject.ApplyModifiedProperties();
            });
            menu.AddItem(new GUIContent("CustomComponent2"), false, () =>
            {
                nameProperty.stringValue = "CustomComponent2";
                nameProperty.serializedObject.ApplyModifiedProperties();
            });
        }
    }
}
