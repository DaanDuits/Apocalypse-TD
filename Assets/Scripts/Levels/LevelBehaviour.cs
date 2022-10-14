using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelBehaviour : MonoBehaviour
{
    public static bool canClick = true;
    public float scaleSpeed;
    
    Vector3 ogScale, toScale;

    ButtonBehaviour button;
    CameraBehaviour cam;

    private void Awake()
    {
        button = GameObject.Find("Image").GetComponent<ButtonBehaviour>();
    }

    private void Start()
    {
        cam = Camera.main.gameObject.GetComponent<CameraBehaviour>();
        ogScale = transform.localScale;
        toScale = ogScale;
        button.gameObject.transform.parent.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (transform.localScale != toScale)
        {
            Vector3 difference = toScale - transform.localScale;
            transform.localScale += difference * scaleSpeed * Time.deltaTime;

            Vector3 next = transform.localScale + difference * scaleSpeed * Time.deltaTime;
            if (ogScale.magnitude < toScale.magnitude)
            {
                if (MagnitudeAndRoundVector(next) > toScale.magnitude)
                {
                    transform.localScale = toScale;
                }
            }
            else if (MagnitudeAndRoundVector(next) < toScale.magnitude)
            {
                transform.localScale = toScale;
            }
        }
    }

    public static float MagnitudeAndRoundVector(Vector3 a)
    {
        float i = RoundFloat(a.magnitude);
        return i;
    }

    public static float RoundFloat(float a)
    {
        float i = Mathf.Round(a * 1000) * 0.001f;
        return i;
    }


    private void OnMouseEnter()
    {
        if (canClick)
        {
            toScale = ogScale * 2;
        }
    }
    private void OnMouseExit()
    {
        if (canClick)
        {
            toScale = ogScale;
        }
    }

    private void OnMouseDown()
    {
        if (canClick)
        {
            transform.localScale = ogScale * 2;
            cam.toCamScale = .5f;
            cam.toPos = new Vector3(transform.position.x, transform.position.y, -10);
            canClick = false;
            button.gameObject.transform.parent.gameObject.SetActive(true);
            button.GetComponent<ButtonBehaviour>().dropdown.value = PlayerPrefs.GetInt("DefaultDifficulty");
            button.GetComponent<ButtonBehaviour>().play.SetDifficulty(PlayerPrefs.GetInt("DefaultDifficulty"));
            button.currentLevel = this;
        }
    }

    public void onExit()
    {
        toScale = ogScale;
        cam.toCamScale = 5;
        cam.toPos = new Vector3(0, 0, -10);
        button.gameObject.transform.parent.gameObject.SetActive(false);
        canClick = true;
    }
}

