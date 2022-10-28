using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RepairTowerBehaviour : TowerBehaviour
{
    public float repairRate;
    public int repairAmount;
    float timerR;
    public GameObject button;
    // Start is called before the first frame update
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

    // Update is called once per frame
    void Update()
    {
        if (!button.activeSelf)
        {
            if (timerR <= 0)
            {
                Debug.Log("Repair");
                if (GetAllBuildingsInRange().Length != 0)
                {
                    audioSrc.PlayOneShot(clip, .2f);
                }
                foreach (GameObject o in GetAllBuildingsInRange())
                {
                    switch (o.tag)
                    {
                        case "Base":
                            StartCoroutine(FindObjectOfType<PlayerHpController>().GainHp(repairAmount));
                            break;
                        default:
                            o.GetComponent<BuildingBehaviour>().hp += repairAmount;
                            break;
                    }
                }
                timerR = repairRate;
            }
            timerR -= Time.deltaTime;
        }
    }

    GameObject[] GetAllBuildingsInRange()
    {
        GameObject[] objects = FindObjectsOfType<GameObject>();
        List<GameObject> inRange = new List<GameObject>();

        foreach (GameObject o in objects)
        {
            if ((o.CompareTag("Base") || o.CompareTag("Wall") || o.CompareTag("Tower")) && Vector3.Distance(transform.position, o.transform.position) <= range)
            {
                inRange.Add(o);
            }
        }
        return inRange.ToArray();
    }
}
