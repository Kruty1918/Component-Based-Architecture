using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SGS29.CBA
{
    [CreateAssetMenu(fileName = "NewComponentFilter", menuName = "SGS29/CBA/Component Type Filter")]
    public class ComponentTypeFilter : CBAFilterRule
    {
        [SerializeField, Tooltip("Список правил фільтрації з їхніми пріоритетами")]
        private ControllerFilterRule rule;

        public override bool IsMatch(string component)
        {
            return rule.IsMatch(component);
        }

        public ControllerFilterRule Get() => rule;

        public override void FilterBy<T>(T type)
        {
            CBALevel level = FindLevel(type.Name);

            List<IRuleCollection> rules = GetGroupsRuleCollections();
            List<FilterData> filtersData = new();

            foreach (var rule in rules)
            {
                filtersData.AddRange(rule.GetData().ToList());
            }

            FilterData[] data = filtersData.ToArray();

            type.FilterBy(data);
        }

        public CBALevel FindLevel(string levelName)
        {
            if (rule.Name == levelName)
            {
                return CBALevel.Controller;
            }

            foreach (var group in rule.groups)
            {
                if (group.Name == levelName)
                {
                    return CBALevel.Group;
                }
            }

            foreach (var group in rule.groups)
            {
                foreach (var component in group.components)
                {
                    if (component.ComponentName == levelName)
                    {
                        return CBALevel.Component;
                    }
                }
            }

            return default;
        }

        public List<IRuleCollection> GetGroupsRuleCollections()
        {
            List<IRuleCollection> ruleCollections = new();

            foreach (var group in rule.groups)
            {
                ruleCollections.Add(group);
            }

            return ruleCollections;
        }
    }
}