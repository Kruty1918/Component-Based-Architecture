using UnityEngine;

namespace SGS29.CBA.Example
{
    /// <summary>
    /// Абстрактний базовий компонент для обробки векторних значень.
    /// </summary>
    public abstract class ComponentBase : AbstractComponentHandler<Vector2>
    {
        /// <summary>
        /// Поточна швидкість, що використовується компонентом.
        /// </summary>
        public Vector2 Velocity { protected get; set; }

        /// <summary>
        /// Метод для обробки логіки компонента. Має бути реалізований у похідних класах.
        /// </summary>
        /// <returns>Оброблене значення Vector2.</returns>
        public override abstract Vector2 Handle();
    }
}