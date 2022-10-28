using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class EndScreenButtons : MonoBehaviour, IDataPersistance
{
    int levelId;
    bool victory = false;

    private void Start()
    {
        levelId = PlayerPrefs.GetInt("CurrentLevel");

        victory = transform.GetChild(0).GetComponent<TMP_Text>().text == "Victory!";
    }

    public void Restart()
    {
        DataPersistanceManager.instance.SaveGame();
        SceneManager.LoadScene("Level");
    }

    public void LevelMap()
    {
        DataPersistanceManager.instance.SaveGame();
        SceneManager.LoadScene("WorldMap");
    }
    public void MainMenu()
    {
        DataPersistanceManager.instance.SaveGame();
        SceneManager.LoadScene("MainMenu");
    }
    public void QuitGame()
    {
        Application.Quit();
    }

    public void LoadData(GameData data)
    {

    }

    public void SaveData(GameData data)
    {
        data.playedLevelsAll.TryGetValue(levelId, out bool hasPlayed);
        if (!hasPlayed)
        {
            if (data.playedLevelsAll.ContainsKey(levelId))
            {
                data.playedLevelsAll.Remove(levelId);
            }
            data.playedLevelsAll.Add(levelId, victory);
        }

        data.playedLevelsEasy.TryGetValue(levelId, out bool onEasy);
        if (!onEasy && victory)
        {
            if (data.playedLevelsEasy.ContainsKey(levelId))
            {
                data.playedLevelsEasy.Remove(levelId);
            }
            data.playedLevelsEasy.Add(levelId, PlayerPrefs.GetInt("Difficulty") == 0);
        }

        data.playedLevelsNormal.TryGetValue(levelId, out bool onNormal);
        if (!onNormal && victory)
        {
            if (data.playedLevelsNormal.ContainsKey(levelId))
            {
                data.playedLevelsNormal.Remove(levelId);
            }
            data.playedLevelsNormal.Add(levelId, PlayerPrefs.GetInt("Difficulty") == 1);
        }

        data.playedLevelsHard.TryGetValue(levelId, out bool onHard);
        if (!onHard && victory)
        {
            if (data.playedLevelsHard.ContainsKey(levelId))
            {
                data.playedLevelsHard.Remove(levelId);
            }
            data.playedLevelsHard.Add(levelId, PlayerPrefs.GetInt("Difficulty") == 2);
        }
    }
}
