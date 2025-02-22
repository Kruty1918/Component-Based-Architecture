using System.Collections.Generic;

namespace SGS29.ComponentBasedArchitecture
{
    /// <summary>
    /// Інтерфейс групи компонентів, яка містить список компонентів та забезпечує їх обробку.
    /// </summary>
    /// <typeparam name="B">Тип компонента, що реалізує <see cref="IComponentHandler{V}"/>.</typeparam>
    /// <typeparam name="V">Тип значення, що обробляється компонентом.</typeparam>
    public interface IComponentGroup<B, V> where B : IComponentHandler<V>
    {
        /// <summary>
        /// Назва групи компонентів.
        /// Використовується для ідентифікації та організації компонентів.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Колекція компонентів, які входять до цієї групи.
        /// Кожен компонент обробляє певний тип даних <typeparamref name="V"/>.
        /// </summary>
        IEnumerable<B> Components { get; }

        /// <summary>
        /// Виконує обробку всіх компонентів у групі та повертає результат.
        /// </summary>
        /// <returns>Обчислене значення типу <typeparamref name="V"/> після обробки всіх компонентів.</returns>
        V Handle();
    }
}