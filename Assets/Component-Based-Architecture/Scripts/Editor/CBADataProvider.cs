using System.Collections.Generic;
using UnityEditor;

namespace SGS29.Editor
{
    public class CBADataProvider : ICBADataProvider<List<ControllerNode>>
    {
        public List<ControllerNode> Load()
        {
            List<ControllerNode> data = new();
            data = CBAReaderWriter.Read();
            if (data == null || data.Count == 0)
            {
                data = new List<ControllerNode>
                {
                    new ControllerNode
                    {
                        controllerName = "MainController",
                        groups = new List<GroupNode>
                        {
                            new GroupNode { groupName = "Group 1", components = new List<string> { "Component1", "Component2" } },
                            new GroupNode { groupName = "Group 2", components = new List<string> { "Component3" } }
                        }
                    }
                };
            }

            return data;
        }

        public void Save(List<ControllerNode> data)
        {
            CBAReaderWriter.Write(data);
            AssetDatabase.Refresh();
        }
    }
}