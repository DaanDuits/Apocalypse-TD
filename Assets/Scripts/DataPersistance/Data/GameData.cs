using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public long lastUpdated;

    public string seed;
    public int defaultDifficulty;

    public SerializebleDictionary<int, bool> playedLevelsAll;
    public SerializebleDictionary<int, bool> playedLevelsEasy;
    public SerializebleDictionary<int, bool> playedLevelsNormal;
    public SerializebleDictionary<int, bool> playedLevelsHard;
    public GameData(string _seed, int _defaultDifficulty)
    {
        seed = _seed;
        defaultDifficulty = _defaultDifficulty;
        playedLevelsAll = new SerializebleDictionary<int, bool>();
        playedLevelsEasy = new SerializebleDictionary<int, bool>();
        playedLevelsNormal = new SerializebleDictionary<int, bool>();
        playedLevelsHard = new SerializebleDictionary<int, bool>();
    }

    public int GetPercentageComplete(PercentageMode mode)
    {
        SerializebleDictionary<int, bool> dic = null;

        switch (mode)
        {
            case PercentageMode.All:
                dic = playedLevelsAll;
                break;
            case PercentageMode.Easy:
                dic = playedLevelsEasy;
                break;
            case PercentageMode.Normal:
                dic = playedLevelsNormal;
                break;
            case PercentageMode.Hard:
                dic = playedLevelsHard;
                break;
        }
        // figure out how many coins are collected
        int totalCollected = 0;
        foreach (bool collected in dic.values)
        {
            if (collected)
                totalCollected++;
        }

        // calculate percentage and don't divide by 0
        int percentageCompleted = 0;
        if (dic.Count != 0)
        {
            percentageCompleted = (totalCollected * 100 / dic.Count);
        }
        return percentageCompleted;
    }
}
