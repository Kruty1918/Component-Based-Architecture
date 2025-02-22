using System.Collections.Generic;
using UnityEngine;

namespace SGS29.ComponentBasedArchitecture
{
    /// <summary>
    /// Базовий клас для контролера, що працює як MonoBehaviour.
    /// </summary>
    public abstract class AbstractMonoController<B, V> : MonoBehaviour, IController<B, V>
        where B : IComponentHandler<V>
    {
        /// <summary>
        /// Назва контролера.
        /// Використовується для ідентифікації конкретного контролера.
        /// За замовчуванням повертає ім'я класу, але може бути перекрите.
        /// </summary>
        public virtual string Name => GetType().Name;

        /// <summary>
        /// Колекція груп компонентів, якими керує цей контролер.
        /// </summary>
        IEnumerable<IComponentGroup<B, V>> IController<B, V>.Groups => GetGroups();

        /// <summary>
        /// Метод, який має повертати групи компонентів.
        /// </summary>
        /// <returns>Перелік груп компонентів.</returns>
        protected abstract IEnumerable<IComponentGroup<B, V>> GetGroups();
    }
}
