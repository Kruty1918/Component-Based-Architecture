using System.Collections.Generic;
using UnityEngine;

namespace SGS29.ComponentBasedArchitecture
{
    /// <summary>
    /// Абстрактний клас для груп компонентів.
    /// Містить реалізацію загальних методів для спрощення створення конкретних груп компонентів.
    /// </summary>
    /// <typeparam name="B">Тип компонента, що реалізує <see cref="IComponentHandler{V}"/>.</typeparam>
    /// <typeparam name="V">Тип значення, що обробляється компонентом.</typeparam>
    public abstract class AbstractComponentGroup<B, V> : MonoBehaviour, IComponentGroup<B, V> where B : IComponentHandler<V>
    {
        /// <summary>
        /// Назва групи компонентів.
        /// За замовчуванням повертає ім'я класу групи, але може бути перекрите.
        /// </summary>
        public virtual string Name => GetType().Name;

        /// <summary>
        /// Колекція компонентів, які входять до цієї групи.
        /// Це поле може бути перекрите в конкретній реалізації.
        /// </summary>
        public abstract IEnumerable<B> Components { get; }

        /// <summary>
        /// Виконує обробку всіх компонентів у групі та повертає результат.
        /// Викликає метод <see cref="IComponentHandler{V}.Handle"/> для кожного компонента.
        /// </summary>
        /// <returns>Обчислене значення типу <typeparamref name="V"/> після обробки всіх компонентів.</returns>
        public V Handle()
        {
            V result = default;
            foreach (var component in Components)
            {
                result = component.Handle();
            }
            return result;
        }
    }
}