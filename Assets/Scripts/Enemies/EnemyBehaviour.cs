using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyBehaviour : MonoBehaviour
{
    [HideInInspector]
    public Rigidbody2D rb;
    [SerializeField]
    TowerShop shop;
    [SerializeField]
    public float speed;

    [SerializeField]
    float hp, damage;
    [SerializeField]
    int price;
    [SerializeField]
    GameObject sliderPrefab;
    GameObject sliderObj;
    Slider slider;

    float takenDamage;
    [SerializeField]
    float effectSpeed;
    [SerializeField]
    float rotationSpeed;
    GridController gridController;

    bool knockBack = false;
    [SerializeField]
    float knockBackTime, knockBackForce;
    float time;

    // Start is called before the first frame update
    void Start()
    {
        takenDamage = hp;
        shop = GameObject.FindObjectOfType<TowerShop>();
        rb = gameObject.GetComponent<Rigidbody2D>();
        gridController = GameObject.Find("GridController").GetComponent<GridController>();
        sliderObj = Instantiate(sliderPrefab, GameObject.Find("WorldCanvas").transform);
        slider = sliderObj.GetComponent<Slider>();
        slider.maxValue = hp;
        time = knockBackTime;
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
        if (knockBack)
        {
            rb.velocity = -transform.up * knockBackForce;
            time -= Time.deltaTime;
        }
        if (time <= 0f && knockBack)
        {
            knockBack = false;
            time = knockBackTime;
        }
    }
    private void FixedUpdate()
    {
        if (gridController.curFlowField == null)
            return;
        //Get Directoion to move/rotate to
        Cell cellBelow = gridController.curFlowField.GetCellFromWorldPos(transform.position);
        Vector2 moveDirection = new Vector2(cellBelow.bestDirection.vector.x, cellBelow.bestDirection.vector.y);

        if (!knockBack)
        {
            //Movement
            rb.velocity = moveDirection * speed;
        }

        //Rotation
        float angle = Mathf.Atan2(moveDirection.x, moveDirection.y) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.AngleAxis(-angle, transform.forward), Time.deltaTime * rotationSpeed);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject pro = collision.gameObject;
        if (pro.tag == "Projectile")
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
        EnemyController.unitsInGame.Remove(gameObject);
        shop.counter.Addresources(price);
        Destroy(sliderObj);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject obj = collision.gameObject;
        if (obj.tag == "Tower" || obj.tag == "Wall")
        {
            knockBack = true;
            BuildingBehaviour BB = obj.GetComponent<BuildingBehaviour>();
            BB.hp -= damage;
        }
    }
}
