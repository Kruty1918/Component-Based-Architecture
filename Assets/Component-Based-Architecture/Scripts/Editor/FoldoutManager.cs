using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace SGS29.Editor
{
    public class FoldoutManager : IFoldout
    {
        private Dictionary<string, bool> foldoutStates = new Dictionary<string, bool>();
        private GUIStyle style;

        public FoldoutManager(GUIStyle style = null)
        {
            this.style = style ?? EditorStyles.foldout;
        }

        public void Draw(string key, string label, System.Action drawContent)
        {
            if (!foldoutStates.ContainsKey(key))
                foldoutStates[key] = false;

            foldoutStates[key] = EditorGUILayout.Foldout(foldoutStates[key], label, true, style);

            if (foldoutStates[key])
            {
                EditorGUI.indentLevel++;
                drawContent?.Invoke();
                EditorGUI.indentLevel--;
            }
        }

        public void SetActive(string key, bool active)
        {
            if (foldoutStates.ContainsKey(key))
            {
                foldoutStates[key] = active;
            }
        }

        public void Add(string key, bool defaultActive = false)
        {
            if (!foldoutStates.ContainsKey(key))
            {
                foldoutStates[key] = defaultActive;
            }
        }

        public void Remove(string key)
        {
            if (foldoutStates.ContainsKey(key))
            {
                foldoutStates.Remove(key);
            }
        }
    }
}