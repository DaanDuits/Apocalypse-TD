using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Unit : MonoBehaviour
{
    public Transform target;
    Transform mainTarget;
    public float speed = 20;
    Vector2[] path;
    int targetIndex;
    public float rotationSpeed;

    [HideInInspector]
    public Rigidbody2D rb;
    
    public TowerShop shop;

    public float hp, damage;
    public int price;
    public GameObject sliderPrefab;
    GameObject sliderObj;
    Slider slider;

    float takenDamage;
    public float effectSpeed;

    private void Start()
    {
        target = GameObject.Find("MainBase(Clone)").transform;
        mainTarget = target;
        PathRequestManager.RequestPath(transform.position, target.position, onPathFound);
        takenDamage = hp;
        shop = GameObject.FindObjectOfType<TowerShop>();
        rb = gameObject.GetComponent<Rigidbody2D>();
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
        GameObject pro = collision.gameObject;
        if (pro.CompareTag("Projectile"))
        {
            BulletBehaviour bb = pro.GetComponent<BulletBehaviour>();

            takenDamage = slider.value - bb.damage;

            Destroy(pro, bb.hitLifeTime);
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
                targetIndex++;
                if (targetIndex >= path.Length)
                {
                    yield break;
                }

                currentWayPoint = path[targetIndex];
            }
            Vector2 moveDirection = (currentWayPoint - transform.position).normalized;
            //Rotation
            float angle = Mathf.Atan2(moveDirection.x, moveDirection.y) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.AngleAxis(-angle, transform.forward), Time.deltaTime * rotationSpeed);

            transform.position = Vector3.MoveTowards(transform.position, currentWayPoint, Time.deltaTime * speed);
            rb.velocity = moveDirection * speed;

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
}
