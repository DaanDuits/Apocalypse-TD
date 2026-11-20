using System.Collections.Generic;
using UnityEngine;
using TowerDefense.Utility;
using System.Linq;
using TowerDefense.Health;
using UnityEngine.Rendering;

public class TowerBehaviour : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform barrel;
    
    public float fireRate = 2f, rotationOffset, rotationSpeed, fireSpread;
    public float range;
    float timer;
    public int bulletAmount;
    public AudioSource audioSrc;
    public AudioClip clip;

    private List<Unit> _enemies = new List<Unit>();


    // Update is called once per frame
    private void Start()
    {
        audioSrc = FindObjectOfType<AudioSource>();
        timer = fireRate;
    }
    void Update()
    {
        UpdateEnemies();
        if (GetClosestEnemy() != null)
        {
            for (int i = 0; i < bulletAmount; i++)
            {
                GameObject pro = Instantiate(bulletPrefab);
                pro.transform.position = barrel.position;
                BulletBehaviour bB = pro.GetComponent<BulletBehaviour>();
                Unit eB = GetClosestEnemy();

                if (InterceptionDirection(GetClosestEnemy().transform.position, barrel.position, eB.velocity, bB.speed, out Vector2 result, out Vector2 fullResult))
                {
                    float angle = Mathf.Atan2(result.y, result.x) * Mathf.Rad2Deg - 90f + rotationOffset;
                    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.AngleAxis(angle, transform.forward), Time.deltaTime * rotationSpeed);
                    if (timer <= 0)
                    {
                        float rnd = Random.Range(-fireSpread, fireSpread) / 100;
                        bB.Setup(fullResult + new Vector2(rnd, rnd), transform.parent);
                        audioSrc.PlayOneShot(clip);
                        Destroy(pro, bB.airTime);
                        if (i == bulletAmount - 1)
                            timer = fireRate;
                    }
                    else
                        Destroy(pro);
                }
                else
                    Destroy(pro);

            }

        }

        timer -= Time.deltaTime;
    }

    private void UpdateEnemies()
    {
        _enemies = Registry<Unit>.Filter(u => Vector2.Distance(transform.position, u.transform.position) <= range).ToList();
    }

    Unit GetClosestEnemy()
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

    bool InterceptionDirection(Vector2 a, Vector2 b, Vector2 vA, float sB, out Vector2 result, out Vector2 fullResult)
    {
        Vector2 aToB = a - b;
        float dC = aToB.magnitude;
        float alpha = Vector2.Angle(aToB, vA) * Mathf.Deg2Rad;
        float sA = vA.magnitude;
        float r = sA / sB;
        if (Math.SolveQuadratic(1 - r* r, 2 * r * dC * Mathf.Cos(alpha), -(dC * dC), out float root1, out float root2) == 0)
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