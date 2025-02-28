using System.Collections.Generic;
using UnityEngine;

namespace SGS29.CBA
{
    /// <summary>
    /// Абстрактний клас для груп компонентів.
    /// Цей клас містить загальні методи для полегшення створення конкретних груп компонентів.
    /// Кожна група може містити набір компонентів, що працюють з певними типами значень, і може виконувати їх обробку.
    /// </summary>
    /// <typeparam name="B">Тип компонента, що реалізує <see cref="IComponentHandler{V}"/>. Це компонент, який буде обробляти значення типу <typeparamref name="V"/>.</typeparam>
    /// <typeparam name="V">Тип значення, яке обробляється компонентами в групі. Це може бути будь-який тип, який визначає специфікацію компонентів.</typeparam>
    public abstract class AbstractComponentGroup<B, V> : MonoBehaviour, IComponentGroup<B, V> where B : IComponentHandler<V>
    {
        /// <summary>
        /// Назва групи компонентів.
        /// За замовчуванням повертає ім'я класу групи, але може бути перекрите у нащадках для специфічних випадків.
        /// </summary>
        public virtual string Name => GetType().Name;

        /// <summary>
        /// Колекція компонентів, які входять до цієї групи.
        /// Це поле може бути перекрите в конкретній реалізації, щоб визначити конкретні компоненти в групі.
        /// </summary>
        public abstract List<B> Components { get; protected set; }

        /// <summary>
        /// Виконує обробку всіх компонентів у групі та повертає результат.
        /// Для кожного компонента викликається метод <see cref="IComponentHandler{V}.Handle"/>, який обробляє значення і повертає результат.
        /// </summary>
        /// <returns>
        /// Обчислене значення типу <typeparamref name="V"/> після обробки всіх компонентів.
        /// Це значення формується на основі оброблених результатів усіх компонентів у групі.
        /// </returns>
        public abstract V Handle();

        /// <summary>
        /// Отримує перелік компонентів групи як перераховувану колекцію.
        /// Цей метод має бути реалізований у нащадках для надання доступу до компонентів групи.
        /// </summary>
        /// <returns>
        /// Перераховувана колекція компонентів групи.
        /// </returns>
        public abstract IEnumerable<B> GetEnumerable();

        /// <summary>
        /// Встановлює залежності для всіх компонентів у групі.
        /// Цей метод передає загальний набір залежностей кожному компоненту в групі для коректної роботи.
        /// </summary>
        /// <param name="dependencies">
        /// Словник залежностей, де ключем є назва залежності (як рядок), 
        /// а значенням — об'єкт, що представляє залежність (наприклад, інші компоненти або сервіси).
        /// </param>
        public void SetDependencies(Dictionary<string, object> dependencies)
        {
            // Ітеруємо через всі компоненти групи та встановлюємо їх залежності
            foreach (var component in Components)
            {
                component.SetDependencies(dependencies);
            }
        }

        public virtual void FilterBy(FilterData[] data)
        {
            if (data == null || data.Length == 0)
                return;

            // Створюємо словник для швидкого пошуку пріоритету за назвою
            Dictionary<string, int> priorityMap = new();
            foreach (var filter in data)
            {
                priorityMap[filter.Name] = filter.Priority;
            }

            // Фільтруємо лише ті компоненти, які є в `FilterData`
            var sortedComponents = new List<B>();
            foreach (var component in Components)
            {
                if (priorityMap.TryGetValue(component.Name, out int priority))
                {
                    InsertSorted(sortedComponents, component, priority, priorityMap);
                }
            }

            // Оновлюємо колекцію компонентів у групі
            Components = sortedComponents;
        }

        /// <summary>
        /// Вставляє компонент у список відповідно до його пріоритету.
        /// </summary>
        private void InsertSorted(List<B> list, B component, int priority, Dictionary<string, int> priorityMap)
        {
            int index = list.FindIndex(c => priorityMap[c.Name] > priority);
            if (index == -1)
                list.Add(component);
            else
                list.Insert(index, component);
        }
    }
}