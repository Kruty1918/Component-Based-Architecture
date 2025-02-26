using System.Collections.Generic;
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

        // Отримати всі GroupFilterRule
        public List<GroupFilterRule> GetAllGroupRules()
        {
            List<GroupFilterRule> groupRules = new List<GroupFilterRule>();
            groupRules.AddRange(rule.groups);
            return groupRules;
        }

        // Отримати всі ComponentFilterRule
        public List<ComponentFilterRule> GetAllComponentRules()
        {
            List<ComponentFilterRule> componentRules = new List<ComponentFilterRule>();
            foreach (var group in rule.groups)
            {
                componentRules.AddRange(group.components);
            }
            return componentRules;
        }
    }
}