using System.Collections.Generic;
using UnityEngine;

namespace SGS29.ComponentBasedArchitecture.Example
{
    /// <summary>
    /// Базова група компонентів, що обробляє їх логіку та агрегує результати.
    /// </summary>
    public sealed class GroupBase : AbstractComponentGroup<ComponentBase, Vector2>
    {
        [Tooltip("Контролер гравця, до якого належить ця група.")]
        [SerializeField] private PlayerController playerController;

        [Tooltip("Список компонентів, що входять до цієї групи.")]
        [SerializeField] private List<ComponentBase> components = new();

        public override IEnumerable<ComponentBase> GetEnumerable()
        {
            return components;
        }

        /// <summary>
        /// Обробляє всі компоненти групи, передаючи результати обчислень між ними.
        /// </summary>
        /// <returns>Агрегований вектор швидкості після обробки компонентів.</returns>
        public override Vector2 Handle()
        {
            Vector2 accumulatedVelocity = playerController.Velocity;

            foreach (var component in components)
            {
                component.Velocity = accumulatedVelocity;
                accumulatedVelocity = component.Handle();
            }

            return accumulatedVelocity;
        }
    }
}