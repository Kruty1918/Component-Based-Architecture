using System.Collections.Generic;
using UnityEngine;

namespace SGS29
{
    public static class CBAReaderWriter
    {
        private const string CBADATA_FILE_PATH = "Assets/Component-Based-Architecture/Resources/CBAList.json";

        public static List<ControllerNode> Read()
        {
            if (!System.IO.File.Exists(CBADATA_FILE_PATH))
                return new List<ControllerNode>();

            string jsonData = System.IO.File.ReadAllText(CBADATA_FILE_PATH);
            return JsonUtility.FromJson<Wrapper>(jsonData)?.controllers ?? new List<ControllerNode>();
        }

        public static void Write(List<ControllerNode> controllers)
        {
            string jsonData = JsonUtility.ToJson(new Wrapper { controllers = controllers }, true);
            System.IO.File.WriteAllText(CBADATA_FILE_PATH, jsonData);
        }

        [System.Serializable]
        private class Wrapper
        {
            public List<ControllerNode> controllers;
        }
    }
}