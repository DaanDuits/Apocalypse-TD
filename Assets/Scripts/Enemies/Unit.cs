using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Unit : MonoBehaviour
{
    public Transform mainTarget;
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

    //int changeToAgro;
    float takenDamage;
    public float effectSpeed;

    public List<string> tags;
    int changeToDistract;

    private void Awake()
    {
        switch (PlayerPrefs.GetInt("Difficulty"))
        {
            case 0:
                hp = 75;
                damage = 10;
                price = 20;
                changeToDistract = 5;
                //changeToAgro = 100;
                break;
            case 1:
                hp = 100;
                damage = 25;
                price = 15;
                changeToDistract = 10;
                //changeToAgro = 60;
                break;
            case 2:
                hp = 125;
                damage = 40;
                price = 10;
                changeToDistract = 20;
                //changeToAgro = 30;
                break;
        }
    }

    private void Start()
    {
        System.Random rng = new System.Random();
        GameObject[] targets = GameObject.FindGameObjectsWithTag("Decoy");
        if (targets.Length != 0)
            mainTarget = rng.Next(0, changeToDistract) == 1 ? targets[rng.Next(0, targets.Length)].transform : GameObject.Find("MainBase(Clone)").transform;
        else
            mainTarget = GameObject.Find("MainBase(Clone)").transform;
        PathRequestManager.RequestPath(transform.position, mainTarget.position, onPathFound, mainTarget == GameObject.Find("MainBase(Clone)").transform);
        takenDamage = hp;
        shop = FindObjectOfType<TowerShop>();
        sliderObj = Instantiate(sliderPrefab, GameObject.Find("WorldCanvas").transform);
        slider = sliderObj.GetComponent<Slider>();
        slider.maxValue = hp;
        slider.value = hp;
    }

    // Update is called once per frame
    void Update()
    {
        sliderObj.transform.position = new Vector2(transform.position.x, transform.position.y + 0.4f);

        if (slider.value <= 1f)
        {
            Destroy(gameObject);
        }
        if (slider.value > takenDamage)
        {
            slider.value -= 3 * takeDamage * Time.deltaTime;
        }
    }

    float takeDamage;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject go = collision.gameObject;
        if (go.CompareTag("Projectile"))
        {
            BulletBehaviour bb = go.GetComponent<BulletBehaviour>();

            takenDamage = slider.value - bb.damage;
            takeDamage = bb.damage;
            System.Random rng = new System.Random();

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
        if (ec != null)
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

    bool stopFollowingPath;
    IEnumerator FollowPath()
    {
        stopFollowingPath = false;
        Vector3 currentWayPoint = path[0];

        while (!stopFollowingPath)
        {
            if ((currentWayPoint - transform.position).magnitude <= 0.4f)
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
        if (tags.Contains(collision.gameObject.tag) && !DealDamage)
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
        PlayerHpController ph = FindObjectOfType<PlayerHpController>();
        switch (obj.tag)
        {
            case "Base":
                StartCoroutine(ph.TakeDamage(damage));
                break;
            default:
                bb.hp -= damage;
                break;
        }
        while (DealDamage)
        {
            yield return new WaitForSeconds(damageCooldown); 
            switch (obj.tag)
            {
                case "Base":
                    StartCoroutine(ph.TakeDamage(damage));
                    break;
                case "Decoy":
                    if (bb.hp - damage <= 0)
                    {
                        bb.hp -= damage;
                        stopFollowingPath = true;
                        yield return null;
                        mainTarget = GameObject.Find("MainBase(Clone)").transform;
                        PathRequestManager.RequestPath(transform.position, mainTarget.position, onPathFound, mainTarget == GameObject.Find("MainBase(Clone)").transform);
                        yield break;
                    }
                    bb.hp -= damage;
                    break;
                default:
                    if (bb.hp - damage <= 0)
                    {
                        bb.hp -= damage;
                        yield break;
                    }
                    bb.hp -= damage;
                    break;
            }
        }
    }
}
