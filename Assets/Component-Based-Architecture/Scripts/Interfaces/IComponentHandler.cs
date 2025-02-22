using System.Collections.Generic;

namespace SGS29.ComponentBasedArchitecture
{
    /// <summary>
    /// Загальний інтерфейс для обробників компонентів.
    /// Використовується для визначення об'єктів, які можуть обробляти певне значення.
    /// </summary>
    /// <typeparam name="V">Тип значення, що обробляється компонентом.</typeparam>
    public interface IComponentHandler<V> : INamed, IHandling<V>
    {
        /// <summary>
        /// Ініціалізація залежностей компонента.
        /// Це дозволяє компоненту отримати інші необхідні компоненти або сервіси.
        /// </summary>
        /// <param name="dependencies">Колекція залежностей, яку можна використовувати для налаштування.</param>
        void SetDependencies(Dictionary<string, object> dependencies);
    }
}