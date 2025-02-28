using System;
using System.Collections.Generic;
using UnityEngine;

namespace SGS29.CBA
{
    /// <summary>
    /// Правило фільтрації з назвою та пріоритетом.
    /// </summary>
    [Serializable]
    public class ControllerFilterRule : IRuleCollection
    {
        [SerializeField] public string Name;
        [SerializeField] public int Priority;
        [SerializeField] public List<GroupFilterRule> groups;

        string INamed.Name => Name;

        public FilterData[] GetData()
        {
            FilterData[] data = new FilterData[groups.Count];

            for (int i = 0; i < groups.Count; i++)
            {
                data[i] = (FilterData)groups[i];
            }

            return data;
        }

        public bool IsMatch(string component)
        {
            foreach (var group in groups)
            {
                if (group.IsMatch(component)) return true;
            }

            return false;
        }
    }

    public interface IRuleCollection : INamed
    {
        FilterData[] GetData();
    }
}