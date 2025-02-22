using System.Collections.Generic;
using UnityEngine;

namespace SGS29.ComponentBasedArchitecture.Example
{
    /// <summary>
    /// Базова група компонентів, що обробляє їх логіку та агрегує результати.
    /// </summary>
    public sealed class GroupBase : MonoBehaviour, IComponentGroup<ComponentBase, Vector2>
    {
        [Tooltip("Назва групи компонентів.")]
        [SerializeField] private string _name;

        [Tooltip("Контролер гравця, до якого належить ця група.")]
        [SerializeField] private PlayerController playerController;

        [Tooltip("Список компонентів, що входять до цієї групи.")]
        [SerializeField] private List<ComponentBase> components = new();

        /// <summary>
        /// Назва групи компонентів.
        /// </summary>
        public string Name => _name;

        /// <summary>
        /// Колекція компонентів у цій групі.
        /// </summary>
        IEnumerable<ComponentBase> IComponentGroup<ComponentBase, Vector2>.Components => components;

        /// <summary>
        /// Обробляє всі компоненти групи, передаючи результати обчислень між ними.
        /// </summary>
        /// <returns>Агрегований вектор швидкості після обробки компонентів.</returns>
        public Vector2 Handle()
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