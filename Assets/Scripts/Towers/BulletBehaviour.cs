using System.Linq;
using TowerDefense.Health;
using TowerDefense.Utility;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float airTime;
    [SerializeField] private float hitLifeTime;
    [SerializeField] private float damage;

    private Transform _shooter;
    private Vector3 _dir;

    public float Speed
    { get => speed; }

    private void Update()
    {
        //transform.Translate(speed * Time.deltaTime * dir.normalized);
        transform.position += speed * Time.deltaTime * _dir.normalized;
    }

    public void Setup(Vector3 uDir, Transform _shooter)
    {
        _dir = uDir;
        this._shooter = _shooter;
        Destroy(gameObject, airTime);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Unit>() == null)
            return;

        IDamageable damageable = Registry<IDamageable>.Filter(d => d == collision.GetComponent<IDamageable>()).First();
        damageable.TakeDamage(damage);
        Destroy(gameObject, hitLifeTime);
    }
}
