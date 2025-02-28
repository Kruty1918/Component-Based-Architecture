using System.Collections.Generic;
using UnityEngine;

namespace SGS29.CBA
{
    /// <summary>
    /// Базовий клас для контролера, що працює як MonoBehaviour.
    /// Цей клас забезпечує основні функції для роботи з групами компонентів та їх обробниками.
    /// </summary>
    /// <typeparam name="B">Тип компонента, який обробляється контролером, що реалізує <see cref="IComponentHandler{V}"/>.</typeparam>
    /// <typeparam name="V">Тип значення, яке обробляється компонентами.</typeparam>
    public abstract class AbstractMonoController<B, V> : MonoBehaviour, IController<B, V>
        where B : IComponentHandler<V>
    {
        /// <summary>
        /// 
        /// </summary>
        [SerializeField] private CBAFilterRule filter;

        /// <summary>
        /// 
        /// </summary>
        protected void Awake()
        {
            FilterBy(filter);
            Awake();
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void OnAwake() { }

        protected virtual void FilterBy(CBAFilterRule filter)
        {

        }

        /// <summary>
        /// Назва контролера.
        /// Використовується для ідентифікації конкретного контролера.
        /// За замовчуванням повертає ім'я класу, але може бути перекрите у нащадках.
        /// </summary>
        public virtual string Name => GetType().Name;

        /// <summary>
        /// Колекція груп компонентів, якими керує цей контролер.
        /// Цей список повертається через метод <see cref="GetGroups"/>.
        /// </summary>
        public IEnumerable<IComponentGroup<B, V>> Groups => GetGroups();

        /// <summary>
        /// Метод, який має повертати групи компонентів, якими керує контролер.
        /// Цей метод необхідно реалізувати у нащадках для визначення конкретних груп компонентів.
        /// </summary>
        /// <returns>
        /// Перелік груп компонентів, які контролер має обробляти.
        /// </returns>
        protected abstract IEnumerable<IComponentGroup<B, V>> GetGroups();
        /// <summary>
        /// Встановлює залежності для всіх груп компонентів, якими керує цей контролер.
        /// Цей метод ітерує через всі групи і передає їм загальний набір залежностей.
        /// </summary>
        /// <param name="dependencies">
        /// Словник залежностей, де ключем є назва залежності (як рядок),
        /// а значенням — об'єкт, що представляє залежність (наприклад, інші компоненти або сервіси).
        /// </param>
        protected void SetComponentsDependencies(Dictionary<string, object> dependencies)
        {
            // Ітеруємо через всі групи компонентів та ініціалізуємо їх залежності
            foreach (var group in Groups)
            {
                group.SetDependencies(dependencies);
            }
        }
    }
}