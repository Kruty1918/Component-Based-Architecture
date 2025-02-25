using System.Collections.Generic;

namespace SGS29
{
    [System.Serializable]
    public class ControllerNode
    {
        public string controllerName;
        public List<GroupNode> groups = new List<GroupNode>();
    }
}