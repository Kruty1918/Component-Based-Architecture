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

        public ControllerFilterRule Get() => rule;
    }
}