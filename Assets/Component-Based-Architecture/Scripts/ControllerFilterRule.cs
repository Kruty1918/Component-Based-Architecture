using System;
using System.Collections.Generic;
using UnityEngine;

namespace SGS29.CBA
{
    /// <summary>
    /// Правило фільтрації з назвою та пріоритетом.
    /// </summary>
    [Serializable]
    public class ControllerFilterRule
    {
        [SerializeField] public string Name;
        [SerializeField] public int Priority;
        [SerializeField] public List<GroupFilterRule> groups;

        public bool IsMatch(string component)
        {
            foreach (var group in groups)
            {
                if (group.IsMatch(component)) return true;
            }

            return false;
        }
    }
}