using System;
using System.Collections.Generic;
using UnityEngine;


namespace SGS29.CBA
{
    public abstract class CBAFilterRule : ScriptableObject
    {
        public abstract bool IsMatch(string component);
    }

    [CreateAssetMenu(fileName = "NewComponentFilter", menuName = "SGS29/CBA/Component Type Filter")]
    public class ComponentTypeFilter : CBAFilterRule
    {
        [SerializeField] private string controllerName;

        [SerializeField, Tooltip("Список правил фільтрації з їхніми пріоритетами")]
        private List<ControllerFilterRule> _rules;

        public override bool IsMatch(string component)
        {
            foreach (var rule in _rules)
            {
                if (rule.IsMatch(component)) return true;
            }

            return true;
        }

        // Отримати всі GroupFilterRule
        public List<GroupFilterRule> GetAllGroupRules()
        {
            List<GroupFilterRule> groupRules = new List<GroupFilterRule>();
            foreach (var rule in _rules)
            {
                groupRules.AddRange(rule.groups);
            }
            return groupRules;
        }

        // Отримати всі ComponentFilterRule
        public List<ComponentFilterRule> GetAllComponentRules()
        {
            List<ComponentFilterRule> componentRules = new List<ComponentFilterRule>();
            foreach (var rule in _rules)
            {
                foreach (var group in rule.groups)
                {
                    componentRules.AddRange(group.components);
                }
            }
            return componentRules;
        }
    }

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

    [Serializable]
    public class ComponentFilterRule
    {
        public string Name;
        public int Priority;
        public string ComponentName;
    }
}