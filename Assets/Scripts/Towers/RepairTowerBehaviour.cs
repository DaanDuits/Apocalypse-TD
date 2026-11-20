using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TowerDefense.Health;
using TowerDefense.Utility;
using UnityEngine;

public class RepairTowerBehaviour : TowerBehaviour
{
    [SerializeField] private GameObject button;
    [SerializeField] private float repairRate;
    [SerializeField] private int repairAmount;
    float timerR;

    void Start()
    {
        audioSrc = FindObjectOfType<AudioSource>();
        GameObject[] all = FindObjectsOfType<GameObject>(true);
        foreach (GameObject b in all)
        {
            if (b.name == "StartWaveB")
            {
                button = b;
                break;
            }
        }
        timerR = repairRate;
    }

    void Update()
    {
        if (!button.activeSelf)
        {
            if (timerR <= 0)
            {
                if (GetAllBuildingsInRange().Count != 0)
                {
                    audioSrc.PlayOneShot(clip, .2f);
                }
                foreach (IHealable h in GetAllBuildingsInRange())
                {
                    h.GainHealth(repairAmount);
                }
                timerR = repairRate;
            }
            timerR -= Time.deltaTime;
        }
    }

    // TODO: Only get Healables that are a building
    HashSet<IHealable> GetAllBuildingsInRange()
    {
        return Registry<IHealable>.Items;
    }
}
