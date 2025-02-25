using System.Collections.Generic;

namespace SGS29
{
    [System.Serializable]
    public class GroupNode
    {
        public string groupName;
        public List<ComponentNode> components = new List<ComponentNode>();
    }
}