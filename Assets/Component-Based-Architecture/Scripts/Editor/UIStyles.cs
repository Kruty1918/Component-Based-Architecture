using UnityEditor;
using UnityEngine;

namespace SGS29.Editor
{
    public static class UIStyles
    {
        public static Color BOX_COLOR = new Color(0.15f, 0.15f, 0.15f, 1f);

        public static GUIStyle BoldFoldoutStyle()
        {
            return new GUIStyle(EditorStyles.foldout)
            {
                fontStyle = FontStyle.Bold,
                fontSize = 13,
                normal = { textColor = Color.white },
                onNormal = { textColor = Color.white }
            };
        }

        public static GUIStyle ItalicFoldoutStyle()
        {
            return new GUIStyle(EditorStyles.foldout)
            {
                fontStyle = FontStyle.Italic,
                fontSize = 12,
                normal = { textColor = new Color(0.8f, 0.8f, 0.8f) },
                onNormal = { textColor = Color.white }
            };
        }

        public static GUIStyle BoxStyle()
        {
            return new GUIStyle("box")
            {
                padding = new RectOffset(10, 10, 5, 5),
                margin = new RectOffset(5, 5, 5, 5),
                normal = { background = MakeTex(2, 2, BOX_COLOR) }
            };
        }

        public static Texture2D MakeTex(int width, int height, Color col)
        {
            Color[] pix = new Color[width * height];
            for (int i = 0; i < pix.Length; i++)
                pix[i] = col;

            Texture2D result = new Texture2D(width, height);
            result.SetPixels(pix);
            result.Apply();
            return result;
        }
    }
}
