using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TowerDefense.Utility;
using TowerDefense.Health;
using TowerDefense.Building;

[RequireComponent(typeof(SpriteRenderer))]
public class Unit : MonoBehaviour, IDamageable
{
    public Transform mainTarget;
    public Transform target;

    [SerializeField] private int reward;
    [SerializeField] private float speed = 20f;
    [SerializeField] private float damageCooldown = 2.5f;
    [SerializeField] private Slider sliderPrefab;

    [SerializeField] private Color red, normal;
    [SerializeField] private float colorLerpSpeed = 5f;

    Vector2[] path;
    int targetIndex;
    public float rotationSpeed;
    
    public TowerShop shop;

    public Vector2 velocity;
    private Slider _slider;

    public float effectSpeed;
    int changeToDistract;

    private bool _dealDamage;
    private float _damage;

    private float _maxHealth;
    private float _health;

    private IEnumerator _currentDamageLoop;

    private SpriteRenderer _renderer;

    private void Awake()
    {
        switch (PlayerPrefs.GetInt("Difficulty"))
        {
            case 0:
                _maxHealth = 75;
                _damage = 10;
                reward = 20;
                changeToDistract = 5;
                break;
            case 1:
                _maxHealth = 100;
                _damage = 25;
                reward = 15;
                changeToDistract = 10;
                break;
            case 2:
                _maxHealth = 125;
                _damage = 40;
                reward = 10;
                changeToDistract = 20;
                break;
        }

        _renderer = GetComponent<SpriteRenderer>();
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

        shop = FindAnyObjectByType<TowerShop>();

        _slider = Instantiate(sliderPrefab, GameObject.Find("WorldCanvas").transform);
        _slider.maxValue = _maxHealth;
        _slider.value = _maxHealth;
        _health = _maxHealth;
    }

    private void OnEnable()
    {
        Registry<Unit>.Register(this);
        Registry<IDamageable>.Register(this);
    }

    private void OnDisable()
    {
        Registry<Unit>.Unregister(this);
        Registry<IDamageable>.Unregister(this);
    }

    // Update is called once per frame
    void Update()
    {
        _slider.transform.position = new Vector2(transform.position.x, transform.position.y + 0.4f);
    }

    public void TakeDamage(float damage)
    {
        float newHealth = _health - damage;

        StartCoroutine(LerpDamage(_health, newHealth));
        StartCoroutine(LerpColors());
        _health = newHealth;
        _health = Mathf.Clamp(_health, 0f, _maxHealth);

        if (_health == 0f)
            Destroy(gameObject);
    }

    private IEnumerator LerpDamage(float a, float b)
    {
        float t = 0f;

        while (t != 1f)
        {
            _slider.value = Mathf.Lerp(a, b, t);

            t += Time.deltaTime * colorLerpSpeed;
            t = Mathf.Clamp01(t); 

            yield return null;
        }
    }
    private IEnumerator LerpColors()
    {
        float t = 0f;
        while ( t != 1f)
        {
            Debug.Log(t);

            _renderer.color = Color.Lerp(normal, red, -4f * Mathf.Pow(t, 2) + 4f * t);

            t += Time.deltaTime * colorLerpSpeed;
            t = Mathf.Clamp01(t);
            yield return null;
        }
    }

    private void OnDestroy()
    {
        shop.counter.Addresources(reward);
        Destroy(_slider.gameObject);
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
        if (collision.gameObject.GetComponent<Structure>() == null)
            return;
        if (_dealDamage)
            return;

        _dealDamage = true;
        velocity = Vector2.zero;
        IDamageable other = Registry<IDamageable>.Filter(d => d == collision.gameObject.GetComponent<IDamageable>()).First();

        _currentDamageLoop = DealDamage(other);
        StartCoroutine(_currentDamageLoop);
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Structure>() == null)
            return;
        if (!_dealDamage)
            return;

        _dealDamage = false;
        StopCoroutine(_currentDamageLoop);
    }

    private IEnumerator DealDamage(IDamageable other)
    {
        while (_dealDamage)
        {
            other.TakeDamage(_damage);

            yield return new WaitForSeconds(damageCooldown);

            if (other == null)
                _dealDamage = false;
        }
    }
}
