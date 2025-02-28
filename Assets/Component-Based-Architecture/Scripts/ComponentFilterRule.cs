using System;

namespace SGS29.CBA
{
    [Serializable]
    public class ComponentFilterRule
    {
        public string ComponentName;
        public int Priority;

        public static explicit operator FilterData(ComponentFilterRule v)
        {
            return new FilterData() { Name = v.ComponentName, Priority = v.Priority };
        }
    }
}