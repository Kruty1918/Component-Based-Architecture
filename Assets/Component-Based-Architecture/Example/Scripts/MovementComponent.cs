using UnityEngine;

namespace GlowspireGames.ComponentBasedArchitecture
{
    public class MovementComponent : ComponentBase
    {
        [SerializeField] private float speed = 5f;

        public override Vector2 Handle()
        {
            float moveX = Input.GetAxis("Horizontal"); // Ліво/Право (A/D або стрілки)
            float moveZ = Input.GetAxis("Vertical");   // Вперед/Назад (W/S або стрілки)

            Vector2 moveDirection = new Vector2(moveX, moveZ).normalized * speed;

            return moveDirection + Velocity;
        }
    }
}