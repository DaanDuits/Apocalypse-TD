using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;
using TMPro;

public class BuildingBehaviour : MonoBehaviour
{
    float playerHp;
    public Slider slider;
    public float hp;
    float speed = 5;

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

    // Update is called once per frame
    void Update()
    {
        if (hp <= 0)
        {
            Destroy(gameObject);
        }
    }

    public IEnumerator TakeDamage(float damage)
    {
        float newHp = playerHp - damage; 
        if (newHp < 1f && GameObject.Find("GameUI").activeSelf)
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
            playerHp = Mathf.Lerp(playerHp, newHp, speed * Time.deltaTime);
            slider.value = playerHp;

            yield return null;
        }
        
    }
}
