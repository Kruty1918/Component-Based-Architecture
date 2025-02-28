using System.Collections.Generic;
using UnityEngine;

namespace SGS29.CBA.Example
{
    /// <summary>
    /// Контролер гравця, який обробляє компоненти та керує їхньою взаємодією.
    /// </summary>
    public class PlayerController : AbstractMonoController<ComponentBase, Vector2>
    {

        [SerializeField, Tooltip("Список груп компонентів, які взаємодіють із контролером")]
        private List<AbstractComponentGroup<ComponentBase, Vector2>> _componentGroups;

        /// <summary>
        /// Поточна швидкість гравця.
        /// </summary>
        public Vector2 Velocity { get; private set; }

        protected override void FilterBy(CBAFilterRule filter)
        {
            filter.FilterBy(_componentGroups[0]);
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
            return _componentGroups;
        }
    }
}