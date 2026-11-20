using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;
using TMPro;
using TowerDefense.Utilities;
using TowerDefense.Health;

public class PlayerHpController : MonoBehaviour, IDamageable
{
    [SerializeField] private Slider slider;
    private float _playerHP;
    private float _damageSpeed = 4f;

    private void Start()
    {
        slider = GameObject.Find("PlayerHp").GetComponent<Slider>();
        switch (PlayerPrefs.GetInt("Difficulty"))
        {
            case 0:
                slider.maxValue = 1500;
                slider.value = 1500;
                _playerHP = 1500;
                break;
            case 1:
                slider.maxValue = 1000;
                slider.value = 1000;
                _playerHP = 1000;
                break;
            case 2:
                slider.maxValue = 600;
                slider.value = 600;
                _playerHP = 600;
                break;
        }
    }

    public void TakeDamage(float damage)
    {
        float newHP = _playerHP - damage;
        StartCoroutine(LerpDamage(_playerHP, newHP));
        _playerHP = newHP;
        _playerHP = Mathf.Clamp(_playerHP, 0f, slider.maxValue);
    }
    private IEnumerator LerpDamage(float a, float b)
    {
        float t = 0f;
        if (_playerHP == 0f && GameObject.Find("GameUI").activeSelf)
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
        while (slider.value != b)
        {
            slider.value = Mathf.Lerp(a, b, t);

            t += Time.deltaTime * _damageSpeed;
            t = Mathf.Clamp01(t);
            yield return null;
        }
    }
    public IEnumerator GainHp(float amount)
    {
        float newHP = _playerHP + amount;
        _playerHP = Mathf.Clamp(newHP, 0f, slider.maxValue);
        float oldHP = _playerHP;

        float t = 0f;
        while (slider.value != newHP)
        {
            slider.value = Mathf.Lerp(oldHP, newHP, t);

            t += Time.deltaTime * _damageSpeed;
            t = Mathf.Clamp01(t);

            yield return null;
        }

    }
}
