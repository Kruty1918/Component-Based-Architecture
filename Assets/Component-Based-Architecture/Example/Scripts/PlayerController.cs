using System.Collections.Generic;
using UnityEngine;

namespace SGS29.ComponentBasedArchitecture.Example
{
    /// <summary>
    /// Контролер гравця, який обробляє компоненти та керує їхньою взаємодією.
    /// </summary>
    public class PlayerController : AbstractMonoController<ComponentBase, Vector2>
    {
        [SerializeReference] private ComponentFilter filter;

        [SerializeField, Tooltip("Список груп компонентів, які взаємодіють із контролером")]
        private List<GroupBase> _componentGroups;
        private List<GroupBase> filterComponentGroups;

        /// <summary>
        /// Поточна швидкість гравця.
        /// </summary>
        public Vector2 Velocity { get; private set; }

        void Start()
        {
            filterComponentGroups = filter.Apply(_componentGroups);
        }

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

        protected override IEnumerable<IComponentGroup<ComponentBase, Vector2>> GetGroups()
        {
            return filterComponentGroups;
        }
    }
}