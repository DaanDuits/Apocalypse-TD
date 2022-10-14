using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    public float speed;
    public float airTime;
    public float hitLifeTime;
    public Transform shooter;

    Vector3 dir;
    public float damage;

    private void Update()
    {
        //transform.Translate(speed * Time.deltaTime * dir.normalized);
        transform.position += speed * Time.deltaTime * dir.normalized;
    }

    public void Setup(Vector3 uDir, Transform _shooter)
    {
        dir = uDir;
        shooter = _shooter;
    }
}
