using UnityEngine;

namespace SGS29.ComponentBasedArchitecture.Example
{
    /// <summary>
    /// Компонент для руху, що обробляє введення гравця та повертає напрямок руху.
    /// </summary>
    public class MovementComponent : ComponentBase
    {
        [Tooltip("Швидкість руху персонажа")]
        [SerializeField] private float speed = 5f;

        /// <summary>
        /// Обробляє введення користувача та обчислює напрямок руху.
        /// </summary>
        /// <returns>Вектор швидкості руху.</returns>
        public override Vector2 Handle()
        {
            Vector2 moveInput = GetInput();
            return CalculateMovement(moveInput);
        }

        /// <summary>
        /// Отримує введення гравця через Input.GetAxis().
        /// </summary>
        /// <returns>Вектор введення гравця.</returns>
        private Vector2 GetInput()
        {
            float moveX = Input.GetAxis("Horizontal"); // Ліво/Право (A/D або стрілки)
            float moveZ = Input.GetAxis("Vertical");   // Вперед/Назад (W/S або стрілки)
            return new Vector2(moveX, moveZ);
        }

        /// <summary>
        /// Розраховує рух на основі введеного вектора.
        /// </summary>
        /// <param name="input">Введення користувача.</param>
        /// <returns>Нормалізований вектор руху з урахуванням швидкості.</returns>
        private Vector2 CalculateMovement(Vector2 input)
        {
            return input.normalized * speed + Velocity;
        }
    }
}