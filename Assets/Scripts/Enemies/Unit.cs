using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Unit : MonoBehaviour
{
    public Transform target;
    public float speed = 20;
    Vector2[] path;
    int targetIndex;
    public float rotationSpeed;
    
    public TowerShop shop;

    public Vector2 velocity;

    public float hp, damage;
    public int price;
    public GameObject sliderPrefab;
    GameObject sliderObj;
    Slider slider;

    float takenDamage;
    public float effectSpeed;

    public List<string> tags;    

    private void Start()
    {
        target = GameObject.Find("MainBase(Clone)").transform;
        PathRequestManager.RequestPath(transform.position, target.position, onPathFound);
        takenDamage = hp;
        shop = GameObject.FindObjectOfType<TowerShop>();
        sliderObj = Instantiate(sliderPrefab, GameObject.Find("WorldCanvas").transform);
        slider = sliderObj.GetComponent<Slider>();
        slider.maxValue = hp;
    }

    // Update is called once per frame
    void Update()
    {
        sliderObj.transform.position = new Vector2(transform.position.x, transform.position.y + 0.4f);

        if (slider.value <= 1f)
        {
            Destroy(gameObject);
        }
        slider.value = Mathf.Lerp(slider.value, takenDamage, Time.deltaTime * effectSpeed);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject go = collision.gameObject;
        if (go.CompareTag("Projectile"))
        {
            BulletBehaviour bb = go.GetComponent<BulletBehaviour>();

            takenDamage = slider.value - bb.damage;

            Destroy(go, bb.hitLifeTime);
        }
    }
    private void OnDestroy()
    {
        GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>();
        foreach (GameObject o in allObjects)
        {
            if (o.tag == "Shooter")
            {
                TowerBehaviour tB = o.GetComponent<TowerBehaviour>();
                if (tB != null && tB.enemies.Contains(transform))
                    tB.enemies.Remove(transform);
            }
        }
        EnemyController ec = FindObjectOfType<EnemyController>();
        ec.unitsInGame.Remove(gameObject);
        shop.counter.Addresources(price);
        Destroy(sliderObj);
    }

    public void onPathFound(Vector2[] newPath, bool pathSuccesful)
    {
        if(pathSuccesful)
        {
            path = newPath;
            StopCoroutine(FollowPath());
            StartCoroutine(FollowPath());
        }
    }

    IEnumerator FollowPath()
    {
        Vector3 currentWayPoint = path[0];

        while (true)
        {
            if ((currentWayPoint - transform.position).magnitude <= 0.2f)
            {
                if (targetIndex <= path.Length)
                {
                    if (targetIndex != path.Length - 1)
                    {
                        targetIndex++;
                    }
                    currentWayPoint = path[targetIndex];
                }
            }
            Vector2 moveDirection = (currentWayPoint - transform.position).normalized;
            //Rotation
            float angle = Mathf.Atan2(moveDirection.x, moveDirection.y) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.AngleAxis(-angle, transform.forward), Time.deltaTime * rotationSpeed);

            transform.position += (Vector3)moveDirection * Time.deltaTime * speed;
            velocity = moveDirection * speed;

            yield return null;
        }
    }

    public void OnDrawGizmos()
    {
        if (path != null)
        {
            for (int i = targetIndex; i < path.Length; i++)
            {
                Gizmos.color = Color.black;
                Gizmos.DrawCube(path[i], new Vector3(0.25f, 0.25f, 1));
                if (i == targetIndex)
                {
                    Gizmos.DrawLine(transform.position, path[i]);
                }
                else
                {
                    Gizmos.DrawLine(path[i - 1], path[i]);
                }
            }
        }
    }

    bool DealDamage;

    public float damageCooldown;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (tags.Contains(collision.gameObject.tag))
        {
            DealDamage = true;
            velocity = Vector2.zero;
            StartCoroutine(DealDamageOverTime(collision.gameObject));
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (tags.Contains(collision.gameObject.tag))
        {
            DealDamage = false;
        }
    }

    IEnumerator DealDamageOverTime(GameObject obj)
    {
        BuildingBehaviour bb = obj.GetComponent<BuildingBehaviour>();
        float time = damageCooldown;
        switch (obj.tag)
        {
            case "Base":
                StartCoroutine(bb.TakeDamage(damage));
                break;
            default:
                bb.hp -= damage;
                break;
        }
        while (DealDamage)
        {
            if (time <= 0)
            {
                switch (obj.tag)
                {
                    case "Base":
                        StartCoroutine(bb.TakeDamage(damage));
                        break;
                    default:
                        bb.hp -= damage;
                        break;
                }
                time = damageCooldown;
            }
            time -= Time.deltaTime;
            yield return null;
        }
    }
}
