using UnityEngine;
using TowerDefense.Health;
using TowerDefense.Utility;

namespace TowerDefense.Building
{
    public class Structure : MonoBehaviour, IDamageable, IHealable
    {
        [SerializeField] private float maxHealth;

        private float _health;

        protected float MaxHealth
        { set => maxHealth = value; }
        protected float Health
        { set => _health = value; }

        private void Start()
        {
            _health = maxHealth;
        }

        private void OnEnable()
        {
            Registry<IDamageable>.Register(this);
            Registry<IHealable>.Register(this);
            Registry<Structure>.Register(this);
        }
        private void OnDisable()
        {
            Registry<IDamageable>.Unregister(this);
            Registry<IHealable>.Unregister(this);
            Registry<Structure>.Unregister(this);
        }

        private void Update()
        {
            if (_health == 0)
            {
                Destroy(gameObject);
            }
        }

        public virtual void TakeDamage(float damage)
        {
            _health -= damage;
            _health = Mathf.Clamp(_health, 0, maxHealth);
        }
        public virtual void GainHealth(float health)
        {
            _health += health;
            _health = Mathf.Clamp(_health, 0, maxHealth);
        }
    }
}
