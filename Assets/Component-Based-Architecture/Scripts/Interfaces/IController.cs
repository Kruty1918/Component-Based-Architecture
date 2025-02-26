using System.Collections.Generic;

namespace SGS29.CBA
{
    /// <summary>
    /// Загальний інтерфейс контролера, що містить групи компонентів.
    /// Використовується для керування взаємодією між компонентами через групи.
    /// </summary>
    /// <typeparam name="B">Тип компонента, який реалізує <see cref="IComponentHandler{V}"/>.</typeparam>
    /// <typeparam name="V">Тип значення, що обробляється компонентом.</typeparam>
    public interface IController<B, V> : INamed where B : IComponentHandler<V>
    {
        /// <summary>
        /// Колекція груп компонентів, якими керує цей контролер.
        /// </summary>
        IEnumerable<IComponentGroup<B, V>> Groups { get; }
    }
}