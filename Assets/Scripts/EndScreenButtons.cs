using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class EndScreenButtons : MonoBehaviour, IDataPersistance
{
    int levelId;
    bool victory;

    private void Start()
    {
        levelId = PlayerPrefs.GetInt("CurrentLevel");

        victory = transform.GetChild(0).GetComponent<TMP_Text>().text == "Victory!";
    }

    public void Restart()
    {
        SceneManager.LoadScene("Level");
    }

    public void LevelMap()
    {
        SceneManager.LoadScene("WorldMap");
    }
    public void LoadData(GameData data)
    {

    }

    public void SaveData(GameData data)
    {
        if (!data.playedLevelsAll[levelId])
        {
            if (data.playedLevelsAll.ContainsKey(levelId))
            {
                data.playedLevelsAll.Remove(levelId);
            }
            data.playedLevelsAll.Add(levelId, victory);
        }
        if (!data.playedLevelsEasy[levelId])
        {
            if (data.playedLevelsEasy.ContainsKey(levelId))
            {
                data.playedLevelsEasy.Remove(levelId);
            }
            data.playedLevelsEasy.Add(levelId, PlayerPrefs.GetInt("Difficulty") == 0);
        }
        if (!data.playedLevelsNormal[levelId])
        {
            if (data.playedLevelsNormal.ContainsKey(levelId))
            {
                data.playedLevelsNormal.Remove(levelId);
            }
            data.playedLevelsNormal.Add(levelId, PlayerPrefs.GetInt("Difficulty") == 1);
        }
        if (!data.playedLevelsHard[levelId])
        {
            if (data.playedLevelsHard.ContainsKey(levelId))
            {
                data.playedLevelsHard.Remove(levelId);
            }
            data.playedLevelsHard.Add(levelId, PlayerPrefs.GetInt("Difficulty") == 2);
        }
    }
}
