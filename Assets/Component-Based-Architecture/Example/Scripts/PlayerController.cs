using System.Collections.Generic;
using UnityEngine;

namespace SGS29.ComponentBasedArchitecture.Example
{
    /// <summary>
    /// Контролер гравця, який обробляє компоненти та керує їхньою взаємодією.
    /// </summary>
    public class PlayerController : MonoBehaviour, IController<ComponentBase, Vector2>
    {
        [SerializeField, Tooltip("Ім'я контролеру")]
        private string _controllerName;

        [SerializeField, Tooltip("Список груп компонентів, які взаємодіють із контролером")]
        private List<GroupBase> _componentGroups;

        /// <summary>
        /// Поточна швидкість гравця.
        /// </summary>
        public Vector2 Velocity { get; private set; }

        /// <summary>
        /// Ім'я контролеру.
        /// </summary>
        public string Name => _controllerName;

        /// <summary>
        /// Колекція груп компонентів, які керують логікою гравця.
        /// </summary>
        private IEnumerable<IComponentGroup<ComponentBase, Vector2>> Groups => _componentGroups;

        IEnumerable<IComponentGroup<ComponentBase, Vector2>> IController<ComponentBase, Vector2>.Groups => Groups;

        private void FixedUpdate()
        {
            UpdateVelocity();
            MovePlayer();
        }

        /// <summary>
        /// Оновлює швидкість гравця на основі даних з компонентних груп.
        /// </summary>
        private void UpdateVelocity()
        {
            Velocity = Vector2.zero;
            foreach (var group in Groups)
            {
                Velocity += group.Handle();
            }
        }

        /// <summary>
        /// Переміщує гравця відповідно до його швидкості.
        /// </summary>
        private void MovePlayer()
        {
            transform.position += new Vector3(Velocity.x, 0, Velocity.y) * Time.fixedDeltaTime;
        }
    }
}