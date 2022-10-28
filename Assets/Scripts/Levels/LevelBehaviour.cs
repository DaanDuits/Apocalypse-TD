using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelBehaviour : MonoBehaviour, IDataPersistance
{
    public static bool canClick;
    public float scaleSpeed;

    bool hasCompleted, onEasy, onHard, onNormal;
    int levelId, mapId;

    Vector3 ogScale, toScale;

    Transform flags;

    ButtonBehaviour button;
    CameraBehaviour cam;

    public static LevelBehaviour instance;

    private void Awake()
    {
        flags = transform.GetChild(0);
        for (int i = 0; i < transform.parent.childCount; i++)
        {
            levelId = i;
        }
        canClick = true;
        button = GameObject.Find("Image").GetComponent<ButtonBehaviour>();

        int seedHash = PlayerPrefs.GetString("Seed").GetHashCode();
        System.Random prng = new System.Random(seedHash + levelId);
        mapId = prng.Next(0, 4);
    }

    private void Start()
    {
        cam = Camera.main.gameObject.GetComponent<CameraBehaviour>();
        ogScale = transform.localScale;
        toScale = ogScale;
        button.gameObject.transform.parent.gameObject.SetActive(false);
    }
    public void LoadData(GameData data)
    {
        data.playedLevelsAll.TryGetValue(levelId, out hasCompleted);
        data.playedLevelsEasy.TryGetValue(levelId, out onEasy);
        if (onEasy)
            flags.GetChild(1).gameObject.SetActive(true);

        data.playedLevelsNormal.TryGetValue(levelId, out onNormal);
        if (onNormal)
            flags.GetChild(2).gameObject.SetActive(true);

        data.playedLevelsHard.TryGetValue(levelId, out onHard);
        if (onHard)
            flags.GetChild(0).gameObject.SetActive(true);
    }

    public void SaveData(GameData data)
    {
        if (data.playedLevelsAll.ContainsKey(levelId))
        {
            data.playedLevelsAll.Remove(levelId);
        }
        data.playedLevelsAll.Add(levelId, hasCompleted);
        if (data.playedLevelsEasy.ContainsKey(levelId))
        {
            data.playedLevelsEasy.Remove(levelId);
        }
        data.playedLevelsEasy.Add(levelId, onEasy);
        if (data.playedLevelsNormal.ContainsKey(levelId))
        {
            data.playedLevelsNormal.Remove(levelId);
        }
        data.playedLevelsNormal.Add(levelId, onNormal);
        if (data.playedLevelsHard.ContainsKey(levelId))
        {
            data.playedLevelsHard.Remove(levelId);
        }
        data.playedLevelsHard.Add(levelId, onHard);
    }

    private void Update()
    {
        if (transform.localScale != toScale)
        {
            Vector3 difference = toScale - transform.localScale;
            transform.localScale += difference * scaleSpeed * Time.deltaTime;

            Vector3 next = transform.localScale + difference * scaleSpeed * Time.deltaTime;
            if (ogScale.magnitude < toScale.magnitude)
            {
                if (MagnitudeAndRoundVector(next) > toScale.magnitude)
                {
                    transform.localScale = toScale;
                }
            }
            else if (MagnitudeAndRoundVector(next) < toScale.magnitude)
            {
                transform.localScale = toScale;
            }
        }
    }

    public static float MagnitudeAndRoundVector(Vector3 a)
    {
        float i = RoundFloat(a.magnitude);
        return i;
    }

    public static float RoundFloat(float a)
    {
        float i = Mathf.Round(a * 1000) * 0.001f;
        return i;
    }


    private void OnMouseEnter()
    {
        if (canClick)
        {
            toScale = ogScale * 2;
        }
    }
    private void OnMouseExit()
    {
        if (canClick)
        {
            toScale = ogScale;
        }
    }

    private void OnMouseDown()
    {
        if (canClick)
        {
            transform.localScale = ogScale * 2;
            cam.toCamScale = .5f;
            cam.toPos = new Vector3(transform.position.x, transform.position.y, -10);
            canClick = false;
            button.gameObject.transform.parent.gameObject.SetActive(true);
            button.GetComponent<ButtonBehaviour>().dropdown.value = PlayerPrefs.GetInt("DefaultDifficulty");
            button.GetComponent<ButtonBehaviour>().play.SetDifficulty(PlayerPrefs.GetInt("DefaultDifficulty"));
            instance = this;
            PlayerPrefs.SetInt("CurrentLevel", levelId);
            Debug.Log(mapId);
            PlayerPrefs.SetInt("MapIndex", mapId);
            button.currentLevel = this;
        }
    }

    public void onExit()
    {
        instance = null;
        toScale = ogScale;
        cam.toCamScale = 5;
        cam.toPos = new Vector3(0, 0, -10);
        button.gameObject.transform.parent.gameObject.SetActive(false);
        canClick = true;
    }
}

