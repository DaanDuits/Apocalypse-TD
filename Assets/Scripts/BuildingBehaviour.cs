using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingBehaviour : MonoBehaviour
{
    public float hp;
    float maxHp;

    private void Start()
    {
        maxHp = hp;
    }
    // Update is called once per frame
    void Update()
    {
        Mathf.Clamp(hp, 0, maxHp);
        if (hp <= 0)
        {
            Destroy(gameObject);
        }
    }

    
}
