using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class ButtonBehaviour : MonoBehaviour, IPointerDownHandler
{
    public LevelBehaviour currentLevel;
    public TMP_Dropdown dropdown;
    public PlayLevel play;

    public void OnPointerDown(PointerEventData pointerEventData)
    {
        if (currentLevel != null)
        {
            currentLevel.onExit();
            currentLevel = null;
        }
    }

}
