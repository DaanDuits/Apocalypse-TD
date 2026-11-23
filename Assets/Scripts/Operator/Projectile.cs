using System.Linq;
using UnityEngine;
using TowerDefense.Health;
using TowerDefense.Utility;

namespace TowerDefense.Operator
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private float speed;
        [SerializeField] private float airTime;
        [SerializeField] private float hitLifeTime;
        [SerializeField] private float damage;

        private Vector3 _dir;

        public float Speed
        { get => speed; }

        private void Update()
        {
            //transform.Translate(speed * Time.deltaTime * dir.normalized);
            transform.position += speed * Time.deltaTime * _dir.normalized;
        }

        public void Setup(Vector3 uDir)
        {
            _dir = uDir;
            Destroy(gameObject, airTime);
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.GetComponent<Unit>() == null)
                return;

            IDamageable damageable = Registry<IDamageable>.Filter(d => d == collision.GetComponent<IDamageable>()).First();
            damageable.TakeDamage(damage);
            Destroy(gameObject, hitLifeTime);
        }
    }
}
