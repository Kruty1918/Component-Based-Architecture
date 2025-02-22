using UnityEngine;

namespace SGS29.ComponentBasedArchitecture
{
    /// <summary>
    /// Абстрактний клас для обробників компонентів.
    /// Містить реалізацію загальних методів для спрощення створення конкретних обробників.
    /// </summary>
    /// <typeparam name="V">Тип значення, що обробляється компонентом.</typeparam>
    public abstract class AbstractComponentHandler<V> : MonoBehaviour, IComponentHandler<V>
    {
        /// <summary>
        /// Назва компонента.
        /// Використовується для ідентифікації конкретного обробника.
        /// За замовчуванням повертає ім'я класу, але може бути перекрите.
        /// </summary>
        public virtual string Name => GetType().Name;

        /// <summary>
        /// Виконує обробку компонента та повертає результат.
        /// Метод може бути перекритий в нащадках для специфічної логіки.
        /// </summary>
        /// <returns>Результат обробки у вигляді значення типу <typeparamref name="V"/>.</returns>
        public abstract V Handle();
    }
}