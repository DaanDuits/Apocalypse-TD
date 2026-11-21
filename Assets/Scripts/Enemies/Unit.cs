using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TowerDefense.Utility;
using TowerDefense.Health;
using System.Linq;

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

    [SerializeField] private float hp, damage;
    [SerializeField] private int reward;
    public GameObject sliderPrefab;
    GameObject sliderObj;
    Slider slider;

    //int changeToAgro;
    float takenDamage;
    public float effectSpeed;
    int changeToDistract;

    private bool _dealDamage;

    public float damageCooldown;

    private void Awake()
    {
        switch (PlayerPrefs.GetInt("Difficulty"))
        {
            case 0:
                hp = 75;
                damage = 10;
                reward = 20;
                changeToDistract = 5;
                //changeToAgro = 100;
                break;
            case 1:
                hp = 100;
                damage = 25;
                reward = 15;
                changeToDistract = 10;
                //changeToAgro = 60;
                break;
            case 2:
                hp = 125;
                damage = 40;
                reward = 10;
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

    private void OnEnable()
    {
        Registry<Unit>.Register(this);
    }

    private void OnDisable()
    {
        Registry<Unit>.Unregister(this);
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
            StartCoroutine(changeColors());
        }
    }

    public Color red, normal;
    IEnumerator changeColors()
    {
        bool toRed = true;
        while (true)
        {
            if (toRed)
            {
                if ((GetComponent<SpriteRenderer>().color + red * Time.deltaTime).r >= red.r)
                {
                    GetComponent<SpriteRenderer>().color = red;
                    toRed = false;
                }
                else
                {
                    GetComponent<SpriteRenderer>().color += red  * Time.deltaTime;
                }
            }
            else
            {
                if ((GetComponent<SpriteRenderer>().color - red * Time.deltaTime).r <= normal.r)
                {
                    GetComponent<SpriteRenderer>().color = normal;
                    break;
                }
                else
                {
                    GetComponent<SpriteRenderer>().color -= red * Time.deltaTime;
                }
            }
            yield return null;
        }
    }

    private void OnDestroy()
    {
        shop.counter.Addresources(reward);
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Building>() == null)
            return;
        if (_dealDamage)
            return;

        _dealDamage = true;
        velocity = Vector2.zero;
        IDamageable other = Registry<IDamageable>.Filter(d => d == collision.gameObject.GetComponent<IDamageable>()).First();
        StartCoroutine(DealDamage(other));
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Building>() == null)
            return;
        if (!_dealDamage)
            return;

        _dealDamage = false;
    }

    private IEnumerator DealDamage(IDamageable other)
    {
        while (_dealDamage)
        {
            other.TakeDamage(damage);

            yield return new WaitForSeconds(damageCooldown);

            if (other == null)
                _dealDamage = false;
        }
    }
}
