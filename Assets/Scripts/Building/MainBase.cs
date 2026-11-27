using TowerDefense.Construction;
using UnityEngine;
using UnityEngine.Events;

namespace TowerDefense.Building
{
    public struct TileMatrix2x2
    {
        public Vector3 TopLeft, TopRight;
        public Vector3 BottomLeft, BottomRight;

        public TileMatrix2x2(Vector3 topLeft)
        {
            TopLeft = topLeft;
            TopRight = new Vector3(topLeft.x - 1f, topLeft.y);
            BottomLeft = new Vector3(topLeft.x, topLeft.y - 1f);
            BottomRight = new Vector3(topLeft.x - 1f, topLeft.y - 1f);
        }
    }
    public class MainBase : Structure
    {
        public UnityEvent<float> OnTakeDamage
        { get; } = new UnityEvent<float>();
        public UnityEvent<float> OnGainHealth
        { get; } = new UnityEvent<float>();
        
        public TileMatrix2x2 TileMatrix
        { get; set; }

        public static MainBase Instance
        {  get; private set; }

        private void Awake()
        {
            if (Instance != null)
                Destroy(gameObject);

            Instance = this;
        }

        protected override void OnDie()
        {
            TileZoneBank.Get(TileZoneType.Occupied).RemoveTileMatrix2x2(TileMatrix);
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
