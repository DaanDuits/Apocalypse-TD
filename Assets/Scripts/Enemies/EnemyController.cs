using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField]
    GameObject unitPrefab;
    [SerializeField]
    int numUnitsPerSpawn;
    [SerializeField]
    List<Transform> possibleSpawns;
    [SerializeField]
    float spawnSpeed;
    float time;
    int i = 0;
    bool spawn;

    public static List<GameObject> unitsInGame;

    

    private void Awake()
    {
        time = spawnSpeed + Random.Range(0, 0.3f);
        unitsInGame = new List<GameObject>();
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

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            DestroyUnits();
        }
    }


    private void SpawnUnits()
    {
        //Spawn 1 unit on a random spawn position
        Vector3 newPos;
        GameObject newUnit = Instantiate(unitPrefab);
        newUnit.GetComponent<CircleCollider2D>().radius = Random.Range(0.3f, 0.6f);
        newUnit.transform.parent = transform;
        unitsInGame.Add(newUnit);

        Transform spawnPoint = possibleSpawns[Random.Range(0, possibleSpawns.Count)];
        newPos = spawnPoint.position;
        newUnit.transform.position = newPos;
    }

    private void DestroyUnits()
    {
        //Destroy all units on screen
        foreach (GameObject go in unitsInGame)
        {
            Destroy(go);
        }
        unitsInGame.Clear();
    }
}
