using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using TMPro;

public class EnemyController : MonoBehaviour
{
    public Level level;
    public int numUnitsPerSpawn;
    public Sprite[] textures;
    public List<Vector2> possibleSpawns;
    public float spawnSpeed;
    float spawnOffset;
    float time;
    int i = 0;
    bool spawn;

    public List<GameObject> unitsInGame;
    int wave = -1;
    StartLevel startLevel;

    public Transform grid;
    public GameObject[] maps;

    GameObject button;
    public TMP_Text text;

    private void Awake()
    {
        Camera.main.GetComponent<PostProcessVolume>().profile.GetSetting<Vignette>().intensity.Override(0f);
        button = GameObject.Find("StartWaveB");
        startLevel = FindObjectOfType<StartLevel>();

        GameObject map = Instantiate(maps[PlayerPrefs.GetInt("MapIndex")], grid);
        map.name = "Level";
        level = map.GetComponent<Level>();
        time = spawnSpeed + Random.Range(0, 0.3f);
        GameObject[] spawnAreas = GameObject.FindGameObjectsWithTag("SpawnArea");
        for (int i = 0; i < spawnAreas.Length; i++)
        {
            Transform spawnArea = spawnAreas[i].transform;
            for (float x = spawnArea.position.x - (spawnArea.localScale.x / 2) + .5f; x <= spawnArea.position.x + (spawnArea.localScale.x / 2) - .5f; x++)
            {
                for (float y = spawnArea.position.y - (spawnArea.localScale.y / 2) + .5f; y <= spawnArea.position.y + (spawnArea.localScale.y / 2) - .5f; y++)
                {
                    possibleSpawns.Add(new Vector2(x, y));
                }
            }
        }
        switch (PlayerPrefs.GetInt("Difficulty"))
        {
            case 0:
                spawnSpeed = 0.8f;
                spawnOffset = 0.5f;
                break;
            case 1:
                spawnSpeed = 0.5f;
                spawnOffset = 0.3f;
                break;
            case 2:
                spawnSpeed = 0.2f;
                spawnOffset = 0.1f;
                break;
        }
    }

    public GameObject walls;
    public void StartWave()
    {
            spawn = true;
            wave++;
            button.SetActive(false);
            walls.SetActive(false);
            text.text = "Wave: " + (wave + 1).ToString() + "/" + startLevel.waves.Count.ToString();
    }

    void Update()
    {

        //Spawn enemies over random time when spawn true
        if (time <= 0 && i < startLevel.waves[wave].enemies.Count && spawn)
        {
            time = spawnSpeed + Random.Range(-spawnOffset, spawnOffset);
            SpawnUnit(startLevel.waves[wave].enemies[i]);
            i++;
        }
        if (wave != -1 && i == startLevel.waves[wave].enemies.Count )
        {
            spawn = false;
            time = spawnSpeed + Random.Range(-spawnOffset, spawnOffset);
            i = 0;
        }
        if (spawn)
        {
            time -= Time.deltaTime;
        }

        if (unitsInGame.Count == 0 && !spawn && wave < startLevel.waves.Count - 1 && wave != -1 && !button.activeSelf)
        {
            walls.SetActive(true);
            button.SetActive(true);
        }
        else if (unitsInGame.Count == 0 && !spawn &&  wave == startLevel.waves.Count - 1 && GameObject.Find("GameUI") != null)
        {
            Camera.main.GetComponent<PostProcessVolume>().profile.GetSetting<Vignette>().intensity.Override(0.45f);
            GameObject.Find("GameUI").SetActive(false);
            GameObject ui = GameObject.Find("EndUI");
            ui.transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>().text = "Victory!";
            ui.transform.GetChild(0).gameObject.SetActive(true);
        }
    }

    private void OnDestroy()
    {
        unitsInGame.Clear();
    }

    private void SpawnUnit(GameObject unit)
    {
        GameObject newUnit = Instantiate(unit);
        System.Random rng = new System.Random();
        newUnit.GetComponent<SpriteRenderer>().sprite = textures[rng.Next(0, textures.Length)];
        //newUnit.GetComponent<CircleCollider2D>().radius = Random.Range(0.3f, 0.6f);
        newUnit.transform.parent = transform;

        Vector2 spawnPoint = possibleSpawns[Random.Range(0, possibleSpawns.Count)];
        newUnit.transform.position = spawnPoint;

        unitsInGame.Add(newUnit);
    }
}
