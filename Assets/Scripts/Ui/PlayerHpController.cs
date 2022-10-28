using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;
using TMPro;

public class PlayerHpController : MonoBehaviour
{
    public Slider slider;
    float playerHp;
    float speed = 5;
    bool takeDamage;

    private void Start()
    {
        slider = GameObject.Find("PlayerHp").GetComponent<Slider>();
        switch (PlayerPrefs.GetInt("Difficulty"))
        {
            case 0:
                slider.maxValue = 1500;
                slider.value = 1500;
                playerHp = 1500;
                break;
            case 1:
                slider.maxValue = 1000;
                slider.value = 1000;
                playerHp = 1000;
                break;
            case 2:
                slider.maxValue = 600;
                slider.value = 600;
                playerHp = 600;
                break;
        }
    }
    public IEnumerator TakeDamage(float damage)
    {
        float newHp = playerHp - damage;
        takeDamage = true;
        if (playerHp < .1f && GameObject.Find("GameUI").activeSelf)
        {
            Camera.main.GetComponent<PostProcessVolume>().profile.GetSetting<Vignette>().intensity.Override(0.45f);
            EnemyController ec = FindObjectOfType<EnemyController>();
            for (int i = 0; i < ec.unitsInGame.Count; i++)
            {
                Destroy(ec.unitsInGame[i]);
            }

            GameObject.Find("GameUI").SetActive(false);
            GameObject ui = GameObject.Find("EndUI");
            ui.transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>().text = "You Died";
            ui.transform.GetChild(0).gameObject.SetActive(true);
        }
        while (newHp < playerHp)
        {
            playerHp -= 8 * speed * Time.deltaTime;
            slider.value = playerHp;

            yield return null;
        }
        takeDamage = false;
    }
    public IEnumerator GainHp(float amount)
    {
        float newHp = playerHp + amount;
        Mathf.Clamp(newHp, 0, slider.maxValue);
        while (newHp > playerHp)
        {
            if (takeDamage)
            {
                newHp = playerHp + amount;
            }
            playerHp += 8 * speed * Time.deltaTime;
            slider.value = playerHp;

            yield return null;
        }

    }
}
