using System.Collections.Generic;
using UnityEngine;

namespace GlowspireGames.ComponentBasedArchitecture
{
    public sealed class GroupBase : MonoBehaviour, IComponentGroup<ComponentBase, Vector2>
    {
        [SerializeField] private PlayerController playerController;
        [SerializeField] private List<ComponentBase> components;

        // Властивість, що повертає компоненти групи
        IEnumerable<ComponentBase> IComponentGroup<ComponentBase, Vector2>.Components => components;

        // Обробка компонентів і повернення результату
        public Vector2 Handle()
        {
            Vector2 result = playerController.Velocity;

            // Підсумовування результатів усіх компонентів
            foreach (var component in components)
            {
                component.Velocity = result;
                result = component.Handle();
            }

            return result;
        }
    }
}
