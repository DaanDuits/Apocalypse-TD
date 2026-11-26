using UnityEngine.Events;

namespace TowerDefense.Building
{
    public class MainBase : Structure
    {
        public UnityEvent<float> OnTakeDamage
        { get; } = new UnityEvent<float>();
        public UnityEvent<float> OnGainHealth
        { get; } = new UnityEvent<float>();

        public static MainBase Instance
        {  get; private set; }

        private void Awake()
        {
            if (Instance != null)
                Destroy(gameObject);

            Instance = this;
        }

        public void ResetHealth(float value)
        {
            MaxHealth = value;
            Health = value;
        }

        public override void TakeDamage(float damage)
        {
            base.TakeDamage(damage);
            OnTakeDamage.Invoke(damage);
        }

        public override void GainHealth(float health)
        {
            base.GainHealth(health);
            OnGainHealth.Invoke(health);
        }
    }
}
