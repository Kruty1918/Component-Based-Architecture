using System;
using System.Collections.Generic;
using System.Linq;
using SGS29.ComponentBasedArchitecture.Example;
using UnityEngine;


namespace SGS29.ComponentBasedArchitecture
{
    [CreateAssetMenu(fileName = "NewComponentFilter", menuName = "SGS29/ComponentBasedArchitecture/ComponentFilter")]
    public class ComponentFilter : ScriptableObject
    {
        [SerializeField, Tooltip("Список правил фільтрації з їхніми пріоритетами")]
        private List<FilterRule> _rules;

        private readonly List<GroupBase> _cachedFilteredGroups = new();
        private List<GroupBase> _lastInputList;

        public List<GroupBase> Apply(List<GroupBase> groups)
        {
            if (groups == null || groups.Count == 0) return groups;

            // Якщо список той самий, повертаємо кеш
            if (_lastInputList == groups) return _cachedFilteredGroups;
            _lastInputList = groups;

            // Спочатку фільтруємо і сортуємо групи, які є в _rules
            var filteredGroups = groups.Where(group =>
                _rules.Any(rule => rule.GroupName == group.Name) // Фільтруємо тільки ті групи, для яких є правило
            ).ToList();

            // Сортуємо за пріоритетом
            filteredGroups = filteredGroups.OrderBy(group =>
                _rules.First(rule => rule.GroupName == group.Name).Priority
            ).ToList();

            // Додаємо групи, для яких немає відповідних правил, в кінець
            var groupsWithoutRule = groups.Where(group =>
                !_rules.Any(rule => rule.GroupName == group.Name) // Якщо для групи немає правила
            ).ToList();

            // Повертаємо комбінований список
            filteredGroups.AddRange(groupsWithoutRule);
            _cachedFilteredGroups.Clear();
            _cachedFilteredGroups.AddRange(filteredGroups);

            Debug.Log(123);
            return _cachedFilteredGroups;
        }

    }

    /// <summary>
    /// Правило фільтрації з назвою та пріоритетом.
    /// </summary>
    [Serializable]
    public class FilterRule
    {
        public string GroupName;
        public int Priority;
    }
}