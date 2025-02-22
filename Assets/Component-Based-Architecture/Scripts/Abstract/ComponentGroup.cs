using System.Collections.Generic;
using UnityEngine;

namespace GlowspireGames.ComponentBasedArchitecture
{
    /// <summary>
    /// Контролер, який керує групами компонентів, що обробляють значення типу Vector2.
    /// </summary>
    public class SameController : MonoBehaviour, IController<IComponentHandler<Vector2>, Vector2>
    {
        /// <summary>
        /// Група компонентів, що відповідає за рух.
        /// </summary>
        public MovementGroup movementGroup;

        /// <summary>
        /// Інша система, яка також є групою компонентів.
        /// </summary>
        public OtherSystem otherSystem;

        /// <summary>
        /// Колекція груп компонентів, що керуються цим контролером.
        /// </summary>
        public IEnumerable<IComponentGroup<IComponentHandler<Vector2>, Vector2>> Groups
        {
            get
            {
                yield return (IComponentGroup<IComponentHandler<Vector2>, Vector2>)movementGroup;
                yield return (IComponentGroup<IComponentHandler<Vector2>, Vector2>)otherSystem;
            }
        }
    }

    /// <summary>
    /// Абстрактна система, яка може виступати як група компонентів та обробник значень Vector2.
    /// </summary>
    public abstract class OtherSystem : MonoBehaviour, IComponentGroup<OtherSystem, Vector2>, IComponentHandler<Vector2>
    {
        /// <summary>
        /// Колекція компонентів, що належать цій групі.
        /// </summary>
        public IEnumerable<OtherSystem> Components => GetEnumerable();

        /// <summary>
        /// Повертає перерахування, що містить тільки цей об'єкт.
        /// </summary>
        public IEnumerable<OtherSystem> GetEnumerable()
        {
            yield return this;
        }

        /// <summary>
        /// Обробка значення Vector2 (не реалізовано).
        /// </summary>
        public Vector2 Handle()
        {
            throw new System.NotImplementedException();
        }
    }

    /// <summary>
    /// Група рухових компонентів, що обробляють значення Vector2.
    /// </summary>
    public class MovementGroup : MonoBehaviour, IComponentGroup<MovementComponent, Vector2>
    {
        /// <summary>
        /// Список рухових компонентів.
        /// </summary>
        public List<MovementComponent> components;

        /// <summary>
        /// Колекція рухових компонентів у групі.
        /// </summary>
        public IEnumerable<MovementComponent> Components => components;

        private Vector2 val;

        /// <summary>
        /// Виконує обробку всіх компонентів у групі та повертає їхню суму.
        /// </summary>
        public Vector2 Handle()
        {
            foreach (var com in components)
            {
                val += com.Handle();
            }
            return val;
        }
    }

    /// <summary>
    /// Абстрактний руховий компонент, який обробляє значення Vector2.
    /// </summary>
    public abstract class MovementComponent : MonoBehaviour, IComponentHandler<Vector2>
    {
        /// <summary>
        /// Виконує обробку значення Vector2 (повинно бути реалізовано в похідних класах).
        /// </summary>
        public abstract Vector2 Handle();
    }

    /// <summary>
    /// Конкретний руховий компонент, що реалізує IComponentHandler<Vector2>.
    /// </summary>
    public class MovementX : MonoBehaviour, IComponentHandler<Vector2>
    {
        /// <summary>
        /// Обробка значення Vector2 (не реалізовано).
        /// </summary>
        public Vector2 Handle()
        {
            throw new System.NotImplementedException();
        }
    }

    /// <summary>
    /// Інший варіант рухового компонента, який обробляє значення Vector2.
    /// </summary>
    public class MovementClamped : MonoBehaviour, IComponentHandler<Vector2>
    {
        /// <summary>
        /// Обробка значення Vector2 (не реалізовано).
        /// </summary>
        public Vector2 Handle()
        {
            throw new System.NotImplementedException();
        }
    }

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

    /// <summary>
    /// Інтерфейс обробника компонентів.
    /// </summary>
    /// <typeparam name="V">Тип значення, що обробляється.</typeparam>
    public interface IComponentHandler<V>
    {
        /// <summary>
        /// Метод обробки компонента.
        /// </summary>
        /// <returns>Результат обробки.</returns>
        V Handle();
    }
}
