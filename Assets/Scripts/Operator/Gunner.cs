using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TowerDefense.Utility;

namespace TowerDefense.Operator
{
    public class Gunner : MonoBehaviour, ITowerOperator
    {
        [SerializeField] private Projectile bulletPrefab;
        [SerializeField] private Transform barrel;

        [SerializeField] private float fireRate = 2f, rotationSpeed, fireSpread;
        [SerializeField] private float range;
        [SerializeField] private int bulletAmount;
        [SerializeField] protected AudioClip clip;
        protected AudioSource _audioSrc;

        private IEnumerable<Unit> _enemies;

        public float Range
        { get => range; }

        // Update is called once per frame
        private void Start()
        {
            _audioSrc = FindAnyObjectByType<AudioSource>();
            UpdateEnemies();
            StartCoroutine(ShootLoop());
        }

        private void FixedUpdate()
        {
            UpdateEnemies();
        }

        private IEnumerator ShootLoop()
        {
            float timer = 0f;
            while (true)
            {
                timer += Time.deltaTime * (1f / fireRate);
                timer = Mathf.Clamp01(timer);

                yield return null;

                if (timer == 1f)
                {
                    HandleProjectiles();
                    timer = 0f;
                }
            }
        }

        private void HandleProjectiles()
        {
            if (GetClosestEnemy() == null)
                return;

            for (int i = 0; i < bulletAmount; i++)
            {
                Projectile bB = Instantiate(bulletPrefab);
                bB.transform.position = barrel.position;
                Unit eB = GetClosestEnemy();

                if (InterceptionDirection(GetClosestEnemy().transform.position, barrel.position, eB.velocity, bulletPrefab.Speed, out Vector2 result, out Vector2 fullResult))
                {
                    float angle = Mathf.Atan2(result.y, result.x) * Mathf.Rad2Deg - 90f;
                    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.AngleAxis(angle, transform.forward), 1f);

                    float rnd = Random.Range(-fireSpread, fireSpread) / 100;
                    bB.Setup(fullResult + new Vector2(rnd, rnd));
                    _audioSrc.PlayOneShot(clip);
                }
            }
        }

        private void UpdateEnemies()
        {
            _enemies = Registry<Unit>.Filter(u => Vector2.Distance(transform.position, u.transform.position) <= range);
        }

        private Unit GetClosestEnemy()
        {
            Unit tMin = null;
            float minDist = Mathf.Infinity;
            Vector3 currentPos = transform.position;
            foreach (Unit t in _enemies)
            {
                float dist = Vector3.Distance(t.transform.position, currentPos);
                if (dist < minDist)
                {
                    tMin = t;
                    minDist = dist;
                }
            }
            return tMin;
        }

        private bool InterceptionDirection(Vector2 a, Vector2 b, Vector2 vA, float sB, out Vector2 result, out Vector2 fullResult)
        {
            Vector2 aToB = a - b;
            float dC = aToB.magnitude;
            float alpha = Vector2.Angle(aToB, vA) * Mathf.Deg2Rad;
            float sA = vA.magnitude;
            float r = sA / sB;
            if (Math.SolveQuadratic(1 - r * r, 2 * r * dC * Mathf.Cos(alpha), -(dC * dC), out float root1, out float root2) == 0)
            {
                result = Vector2.zero;
                fullResult = result;

                return false;
            }
            float dA = Mathf.Max(root1, root2);
            float t = dA / sB;
            Vector2 c = a + vA * t;
            result = (c - b).normalized;
            fullResult = c - b;
            return true;
        }
    }
}