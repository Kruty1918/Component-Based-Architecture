using UnityEngine;

namespace GlowspireGames.ComponentBasedArchitecture
{
    public class MovementClamp : ComponentBase
    {
        [SerializeField] private float maxSpeed = 5f;

        public override Vector2 Handle()
        {
            Vector2 result = new();

            result = Vector2.ClampMagnitude(Velocity, maxSpeed);

            return result;
        }
    }
}