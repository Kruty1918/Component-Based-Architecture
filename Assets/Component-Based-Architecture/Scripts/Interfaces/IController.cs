using System.Collections.Generic;

namespace GlowspireGames.ComponentBasedArchitecture
{
    /// <summary>
    /// Інтерфейс контролера, який містить групи компонентів.
    /// </summary>
    /// <typeparam name="B">Тип компонента, що реалізує IComponentHandler&lt;V&gt;.</typeparam>
    /// <typeparam name="V">Тип значення, що обробляється компонентом.</typeparam>
    public interface IController<B, V> where B : IComponentHandler<V>
    {
        /// <summary>
        /// Колекція груп компонентів, що керуються цим контролером.
        /// </summary>
        IEnumerable<IComponentGroup<B, V>> Groups { get; }
    }
}
