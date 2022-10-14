using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBehaviour : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform barrel;
    public List<Transform> enemies = new List<Transform>();
    
    public float fireRate = 2f, rotationOffset, rotationSpeed, fireSpread;
    public float range;
    float timer;
    public int bulletAmount;

    // Update is called once per frame
    private void Start()
    {   
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
                Unit eB = GetClosestEnemy().gameObject.GetComponent<Unit>();

                if (InterceptionDirection(GetClosestEnemy().position, barrel.position, eB.rb.velocity, bB.speed, out Vector2 result, out Vector2 fullResult))
                {
                    float angle = Mathf.Atan2(result.y, result.x) * Mathf.Rad2Deg - 90f + rotationOffset;
                    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.AngleAxis(angle, transform.forward), Time.deltaTime * rotationSpeed);
                    if (timer <= 0)
                    {
                        float rnd = Random.Range(-fireSpread, fireSpread) / 450;
                        bB.Setup(fullResult + new Vector2(rnd, rnd), transform.parent);
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

    void UpdateEnemies()
    {
        if (enemies.Count > 0)
        {
            for (int i = 0; i < enemies.Count; i++)
            {
                if (Vector2.Distance(enemies[i].position, transform.position) > range)
                {
                    enemies.RemoveAt(i);
                }
            }
        }

        GameObject[] objects = GameObject.FindObjectsOfType<GameObject>();
        foreach (GameObject c in objects)
        {
            if (c.tag == "Enemy" && Vector2.Distance(transform.position, c.transform.position) <= range && !enemies.Contains(c.transform))
            {
                enemies.Add(c.transform);
            }
        }
    }

    Transform GetClosestEnemy()
    {
        Transform tMin = null;
        float minDist = Mathf.Infinity;
        Vector3 currentPos = transform.position;
        foreach (Transform t in enemies)
        {
            float dist = Vector3.Distance(t.position, currentPos);
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
        if (Meth.SolveQuadratic(1 - r* r, 2 * r * dC * Mathf.Cos(alpha), -(dC * dC), out float root1, out float root2) == 0)
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

public class Meth
{
    public static int SolveQuadratic(float a, float b, float c, out float root1, out float root2)
    {
        float discriminant = b * b - 4 * a * c;
        if (discriminant < 0)
        {
            root1 = Mathf.Infinity;
            root2 = -root1;
            return 0;
        }
        root1 = (-b + Mathf.Sqrt(discriminant)) / (2 * a);
        root2 = (-b - Mathf.Sqrt(discriminant)) / (2 * a);
        return discriminant > 0 ? 2 : 1;
    }
}