using UnityEngine;

namespace SGS29.ComponentBasedArchitecture.Example
{
    /// <summary>
    /// Компонент, що обмежує максимальну швидкість об'єкта.
    /// </summary>
    public class MovementClamp : ComponentBase
    {
        [Tooltip("Максимальна швидкість руху об'єкта.")]
        [SerializeField] private float maxSpeed = 5f;

        /// <summary>
        /// Обмежує швидкість об'єкта заданим значенням.
        /// </summary>
        /// <returns>Вектор швидкості з обмеженою величиною.</returns>
        public override Vector2 Handle()
        {
            return ClampVelocity();
        }

        /// <summary>
        /// Обмежує вектор швидкості відповідно до максимального значення.
        /// </summary>
        /// <returns>Вектор швидкості після обмеження.</returns>
        private Vector2 ClampVelocity()
        {
            return Vector2.ClampMagnitude(Velocity, maxSpeed);
        }
    }
}