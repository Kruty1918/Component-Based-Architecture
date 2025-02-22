using UnityEngine;

namespace GlowspireGames.ComponentBasedArchitecture
{
    // Реалізація базового компонента, що обробляє Vector2
    public abstract class ComponentBase : MonoBehaviour, IComponentHandler<Vector2>
    {
        public Vector2 Velocity { protected get; set; }
        public abstract Vector2 Handle();
    }
}
