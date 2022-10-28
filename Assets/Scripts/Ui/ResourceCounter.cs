using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResourceCounter : MonoBehaviour
{

    public float resources;

    [SerializeField]
    TMP_Text text;

    int sign = 0;

    [SerializeField]
    string[] letters;

    private void Start()
    {
        switch (PlayerPrefs.GetInt("Difficulty"))
        {
            case 0:
                resources = 500;
                break;
            case 1:
                resources = 250;
                break;
            case 2:
                resources = 150;
                break;
        }
    }

    private void Update()
    {
        Updateresources();
    }

    void Updateresources()
    {
        if (sign == 0)
        {
            resources = (int)resources;
            text.text = resources.ToString("0") + " " + letters[sign];
        }
        else
        {
            text.text = resources.ToString("0.00") + " " + letters[sign];
        }
        if (resources > 999)
        {
            resources /= 1000;
            sign++;
        }
        if (resources < -999)
        {
            resources /= 1000;
            sign++;
        }
        if (sign != 0 && resources < 1 && resources > 0)
        {
            sign--;
            resources *= 1000;
        }
        if (sign != 0 && resources < 0 && resources > -1)
        {
            sign--;
            resources *= 1000;
        }
    }

    public void Addresources(int addedresources)
    {
        resources += addedresources / Mathf.Pow(1000, sign);
    }
    public void Removeresources(int removedresources)
    {
        resources -= removedresources / Mathf.Pow(1000, sign);
    }


    public bool CheckRemovedResources(int removedresources)
    {
        if (resources - removedresources / Mathf.Pow(1000, sign) < 0)
        {
            return false;
        }
        return true;
    }
}
