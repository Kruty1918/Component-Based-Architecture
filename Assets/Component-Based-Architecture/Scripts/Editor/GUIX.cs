using System;
using UnityEditor;
using UnityEngine;

namespace SGS29.Editor
{
    public static class GUIX
    {
        public static void DrawLine(float treeWidth, float width = 0, float height = 0.5f)
        {
            float lineWidth = width == 0 ? treeWidth : width;
            Rect rect = GUILayoutUtility.GetRect(lineWidth, height);
            EditorGUI.DrawRect(rect, Color.Lerp(UIStyles.BOX_COLOR, Color.white, 0.12f));
        }

        public static void DrawButtonWithIcon(string iconName, System.Action action, float size)
        {
            GUIStyle iconButtonStyle = new GUIStyle(GUI.skin.button)
            {
                normal = { background = UIStyles.MakeTex((int)size, (int)size, Color.clear) },
                padding = new RectOffset(0, 0, 0, 0),
                alignment = TextAnchor.MiddleCenter
            };

            GUIContent icon = EditorGUIUtility.IconContent(iconName);
            if (GUILayout.Button(icon, iconButtonStyle, GUILayout.Width(size), GUILayout.Height(size)))
            {
                action();
            }
        }

        public static string DrawNameField(string label, string value, float labelWidth = 50, float textFieldWidth = 200, float extraIndent = 0)
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                if (extraIndent > 0) GUILayout.Space(extraIndent);
                GUILayout.Label(label, GUILayout.Width(labelWidth));
                return EditorGUILayout.TextField(value, GUILayout.Width(textFieldWidth));
            }
        }
    }
}