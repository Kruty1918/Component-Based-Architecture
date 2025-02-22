using UnityEngine;

namespace SGS29.ComponentBasedArchitecture.Example
{
    /// <summary>
    /// Абстрактний базовий компонент для обробки векторних значень.
    /// </summary>
    public abstract class ComponentBase : MonoBehaviour, IComponentHandler<Vector2>
    {
        [Tooltip("Назва компонента.")]
        [SerializeField] private string _name;

        /// <summary>
        /// Поточна швидкість, що використовується компонентом.
        /// </summary>
        public Vector2 Velocity { protected get; set; }

        /// <summary>
        /// Назва компонента.
        /// </summary>
        public string Name => _name;

        /// <summary>
        /// Метод для обробки логіки компонента. Має бути реалізований у похідних класах.
        /// </summary>
        /// <returns>Оброблене значення Vector2.</returns>
        public abstract Vector2 Handle();
    }
}