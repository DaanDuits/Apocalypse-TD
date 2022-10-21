using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayLevel : MonoBehaviour
{
    public void Onclick()
    {
        SceneManager.LoadScene("Level");
    }

    public void GetDifficulty()
    {
        TMPro.TMP_Dropdown dropdown = GameObject.Find("Dropdown").GetComponent<TMPro.TMP_Dropdown>();

        switch(dropdown.value)
        {
            case 0:
                PlayerPrefs.SetInt("Difficulty", 0);
                break;
            case 1:
                PlayerPrefs.SetInt("Difficulty", 1);
                break;
            case 2:
                PlayerPrefs.SetInt("Difficulty", 2);
                break;
        }
    }

    public void SetDifficulty(int dif)
    {
        PlayerPrefs.SetInt("Difficulty", dif);
    }
}
