using System.Collections.Generic;

namespace GlowspireGames.ComponentBasedArchitecture
{
    /// <summary>
    /// Інтерфейс групи компонентів, яка містить список компонентів та забезпечує їх обробку.
    /// </summary>
    /// <typeparam name="B">Тип компонента, що реалізує IComponentHandler&lt;V&gt;.</typeparam>
    /// <typeparam name="V">Тип значення, що обробляється компонентом.</typeparam>
    public interface IComponentGroup<B, V> where B : IComponentHandler<V>
    {
        /// <summary>
        /// Колекція компонентів у цій групі.
        /// </summary>
        IEnumerable<B> Components { get; }

        /// <summary>
        /// Виконує обробку для всіх компонентів у групі.
        /// </summary>
        /// <returns>Результат обробки типу V.</returns>
        V Handle();
    }
}
