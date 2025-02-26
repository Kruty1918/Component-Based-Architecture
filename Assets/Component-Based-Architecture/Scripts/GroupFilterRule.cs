using System;
using System.Collections.Generic;
using UnityEngine;

namespace SGS29.CBA
{
    [Serializable]
    public class GroupFilterRule
    {
        [SerializeField] public string Name;
        [SerializeField] public int Priority;
        [SerializeField] public List<ComponentFilterRule> components;

        public bool IsMatch(string component)
        {
            foreach (var comp in components)
            {
                if (comp.ComponentName == component) return true;
            }

            return false;
        }
    }
}