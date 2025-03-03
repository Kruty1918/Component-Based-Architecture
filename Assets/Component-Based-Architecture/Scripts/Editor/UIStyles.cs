using UnityEditor;
using UnityEngine;

namespace SGS29.Editor
{
    public static class UIStyles
    {
        public static Color BOX_COLOR = new Color(0.15f, 0.15f, 0.15f, 1f);

        public static readonly GUIStyle StaticBox;


        static UIStyles()
        {
            StaticBox = new GUIStyle();
            // Змінено колір фону на темніший відтінок
            StaticBox.normal.background = MakeTex(2, 2, new Color(0.6f, 0.6f, 0.6f, 1f));
            StaticBox.border = new RectOffset(2, 2, 2, 2);
            StaticBox.margin = new RectOffset(5, 5, 5, 5);
            StaticBox.padding = new RectOffset(5, 5, 5, 5);
        }

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
            {
                pix[i] = col;
            }
            Texture2D result = new Texture2D(width, height);
            result.SetPixels(pix);
            result.Apply();
            return result;
        }
    }

    public class BackgroundColorScope : GUI.Scope
    {
        private readonly Color previousColor;

        public BackgroundColorScope(Color newColor)
        {
            previousColor = GUI.backgroundColor;
            GUI.backgroundColor = newColor;
        }

        protected override void CloseScope()
        {
            GUI.backgroundColor = previousColor;
        }
    }
}
