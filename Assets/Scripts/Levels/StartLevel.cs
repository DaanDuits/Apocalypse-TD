using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartLevel : MonoBehaviour
{
    public List<enemy> enemies;

    public List<Wave> waves;

    private void Awake()
    {
        GameObject.Find("GameUI").SetActive(true);
        switch (PlayerPrefs.GetInt("Difficulty"))
        {
            case 0:
                for (int i = 0; i < 5; i++)
                {
                    CreateWave(enemies, 0);
                }
                break;
            case 1:
                for (int i = 0; i < 8; i++)
                {
                    CreateWave(enemies, 1);
                }
                break;
            case 2:
                for (int i = 0; i < 10; i++)
                {
                    CreateWave(enemies, 2);
                }
                break;
        }
    }

    void CreateWave(List<enemy> e, int dif)
    {
        List<GameObject> es = new List<GameObject>();
        System.Random prng = new System.Random();
        for (int i = 0; i < e.Count; i++)
        {

            for (int j = 0; j < prng.Next(e[i].min[dif], e[i].max[dif]); j++)
            {
                es.Add(e[i].enemyObj);
            }
            e[i].min[dif] += prng.Next(e[i].minWM[dif], e[i].maxWM[dif]);
            e[i].max[dif] += prng.Next(e[i].minWM[dif], e[i].maxWM[dif]);
            if (e[i].min[dif] > e[i].max[dif])
            {
                int oldMin = e[i].min[dif];
                e[i].min[dif] = e[i].max[dif];
                e[i].max[dif] = oldMin;
            }
        }
        Wave newWave = new Wave(es);
        waves.Add(newWave);
    }

    [System.Serializable]
    public class Wave
    {
        // the class that holds the enemies that should be spawned
        public List<GameObject> enemies;

        public Wave(List<GameObject> _enemies)
        {
            enemies = _enemies;
        }
    }

    [System.Serializable]
    public struct enemy
    {
        // the minimum and maximum of enemies in a wave for each difficulty
        public int[] min, max;
        // the minimum and maximum of enemies to add for each new wave for each difficulty
        public int[] minWM, maxWM;
        // the enemy object
        public GameObject enemyObj;
    }
}
