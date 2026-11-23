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
    private float _timer;

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
        _timer = repairRate;
    }

    void Update()
    {
        if (!button.activeSelf)
        {
            if (_timer <= 0)
            {
                if (GetAllBuildingsInRange().Count != 0)
                {
                    audioSrc.PlayOneShot(clip, .2f);
                }
                foreach (IHealable h in GetAllBuildingsInRange())
                {
                    h.GainHealth(repairAmount);
                }
                _timer = repairRate;
            }
            _timer -= Time.deltaTime;
        }
    }

    // TODO: Only get Healables that are a building
    HashSet<IHealable> GetAllBuildingsInRange()
    {
        return Registry<IHealable>.Items;
    }
}
