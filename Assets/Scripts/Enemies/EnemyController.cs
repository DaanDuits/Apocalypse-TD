using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public GameObject unitPrefab;
    public int numUnitsPerSpawn;
    public Sprite[] textures;
    public List<Transform> possibleSpawns;
    public float spawnSpeed;
    float time;
    int i = 0;
    bool spawn;

    

    private void Awake()
    {
        time = spawnSpeed + Random.Range(0, 0.3f);
    }

    void Update()
    {
        //Spawn enemies over random time when spawn true
        if (time <= 0 && i < numUnitsPerSpawn && spawn)
        {
            time = spawnSpeed + Random.Range(0, 0.3f);
            SpawnUnits();
            i++;
        }
        if (i == numUnitsPerSpawn)
        {
            spawn = false;
            time = spawnSpeed + Random.Range(0, 0.3f);
            i = 0;
        }
        if (spawn)
        {
            time -= Time.deltaTime;
        }

        //Set spawn true when 1 is pressed and destroy all when 2 pressed
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            spawn = true;
        }
    }


    private void SpawnUnits()
    {
        //Spawn 1 unit on a random spawn position
        Vector3 newPos;
        GameObject newUnit = Instantiate(unitPrefab);
        System.Random rng = new System.Random();
        newUnit.GetComponent<SpriteRenderer>().sprite = textures[rng.Next(0, textures.Length)];
        //newUnit.GetComponent<CircleCollider2D>().radius = Random.Range(0.3f, 0.6f);
        newUnit.transform.parent = transform;

        Transform spawnPoint = possibleSpawns[Random.Range(0, possibleSpawns.Count)];
        newPos = spawnPoint.position;
        newUnit.transform.position = newPos;
    }
}
