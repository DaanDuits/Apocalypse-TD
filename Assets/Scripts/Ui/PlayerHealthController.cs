using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;
using TMPro;
using TowerDefense.Utility;
using TowerDefense;

public class PlayerHealthController : MonoBehaviour
{
    [SerializeField] private Slider slider;
    private float _playerHealth;
    private float _damageSpeed = 4f;

    private void Start()
    {
        slider = GameObject.Find("PlayerHp").GetComponent<Slider>();
        switch (PlayerPrefs.GetInt("Difficulty"))
        {
            case 0:
                _playerHealth = 1500;
                break;
            case 1:
                _playerHealth = 1000;
                break;
            case 2:
                _playerHealth = 600;
                break;
        }
        slider.maxValue = _playerHealth;
        slider.value = _playerHealth;

        MainBase.Instance.ResetHealth(_playerHealth);
        MainBase.Instance.OnGainHealth.AddListener(MainBase_OnGainHealth);
        MainBase.Instance.OnTakeDamage.AddListener(MainBase_OnTakeDamage);
    }

    public void MainBase_OnTakeDamage(float damage)
    {
        float newHealth = _playerHealth - damage;
        StartCoroutine(LerpDamage(_playerHealth, newHealth));
        _playerHealth = newHealth;
        _playerHealth = Mathf.Clamp(_playerHealth, 0f, slider.maxValue);
    }
    private IEnumerator LerpDamage(float a, float b)
    {
        float t = 0f;
        if (b == 0f && GameObject.Find("GameUI").activeSelf)
        {
            Camera.main.GetComponent<PostProcessVolume>().profile.GetSetting<Vignette>().intensity.Override(0.45f);
            List<Unit> activeUnits = Registry<Unit>.Filter(u => u != null).ToList();
            foreach (Unit u in activeUnits)
            {
                Destroy(u.gameObject);
            }

            GameObject.Find("GameUI").SetActive(false);
            GameObject ui = GameObject.Find("EndUI");
            ui.transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>().text = "You Died";
            ui.transform.GetChild(0).gameObject.SetActive(true);
        }
        while (t != 1f)
        {
            slider.value = Mathf.Lerp(a, b, t);

            t += Time.deltaTime * _damageSpeed;
            t = Mathf.Clamp01(t);
            yield return null;
        }
    }

    public void MainBase_OnGainHealth(float health)
    {
        float newHealth = _playerHealth + health;
        StartCoroutine(LerpHeal(_playerHealth, newHealth));
        _playerHealth = newHealth;
        _playerHealth = Mathf.Clamp(_playerHealth, 0f, slider.maxValue);
    }
    private IEnumerator LerpHeal(float a, float b)
    {
        float t = 0f;
        while (t != 1f)
        {
            slider.value = Mathf.Lerp(a, b, t);

            t += Time.deltaTime * _damageSpeed;
            t = Mathf.Clamp01(t);

            yield return null;
        }
    }
}
