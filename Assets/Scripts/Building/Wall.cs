using TowerDefense.Utility;
using UnityEngine;

namespace TowerDefense.Building
{
    public class Wall : Structure
    {
        public Vector3 Position
        { get => transform.position - transform.rotation * new Vector3(0.5f, 0f); }

        private new void OnEnable()
        {
            base.OnEnable();
            Registry<Wall>.Register(this);
        }

        private new void OnDisable()
        {
            base.OnDisable();
            Registry<Wall>.Unregister(this);
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawSphere(Center, 0.1f);
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(Position, 0.1f);
        }
    }
}
