using System.Collections.Generic;
using UnityEngine;

namespace GlowspireGames.ComponentBasedArchitecture
{
    public class PlayerController : MonoBehaviour, IController<ComponentBase, Vector2>
    {
        [SerializeField] private List<GroupBase> componentGroups;
        public Vector2 Velocity;

        private IEnumerable<IComponentGroup<ComponentBase, Vector2>> Groups
        {
            get
            {
                foreach (var group in componentGroups)
                {
                    yield return group;
                }
            }
        }

        IEnumerable<IComponentGroup<ComponentBase, Vector2>> IController<ComponentBase, Vector2>.Groups => Groups;

        void FixedUpdate()
        {
            Velocity = Vector2.zero;
            foreach (var group in Groups)
            {
                Velocity += group.Handle();
            }

            transform.position += new Vector3(Velocity.x, 0, Velocity.y) * Time.fixedDeltaTime;
        }
    }
}
