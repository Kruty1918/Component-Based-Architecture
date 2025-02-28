using System;
using System.Collections.Generic;
using UnityEngine;

namespace SGS29.CBA
{
    [Serializable]
    public class GroupFilterRule : IRuleCollection
    {
        [SerializeField] public string Name;
        [SerializeField] public int Priority;
        [SerializeField] public List<ComponentFilterRule> components;

        string INamed.Name => Name;

        public FilterData[] GetData()
        {
            FilterData[] data = new FilterData[components.Count];

            for (int i = 0; i < components.Count; i++)
            {
                data[i] = (FilterData)components[i];
            }

            return data;
        }

        public bool IsMatch(string component)
        {
            foreach (var comp in components)
            {
                if (comp.ComponentName == component) return true;
            }

            return false;
        }

        public static explicit operator FilterData(GroupFilterRule v)
        {
            return new FilterData() { Name = v.Name, Priority = v.Priority };
        }
    }
}