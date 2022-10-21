using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class SaveSlot : MonoBehaviour
{
    [Header("Profile")]
    public string profileId = "";

    [Header("Content")]
    public GameObject noDataContent, hasDataContent;
    public TMP_Text nameText, lastPlayedText;
    public TMP_Text percentageAllText, percentageEasyText, percentageNormalText, percentageHardText;

    Button saveSlotButton;

    private void Awake()
    {
        saveSlotButton = GetComponent<Button>();
    }

    public void SetData(GameData data)
    {
        // there is no data for this profile
        if (data == null)
        {
            noDataContent.SetActive(true);
            hasDataContent.SetActive(false);
        }
        // there is data for this profile
        else
        {
            noDataContent.SetActive(false);
            hasDataContent.SetActive(true);

            percentageAllText.text = data.GetPercentageComplete(PercentageMode.All) + "% Complete";
            percentageEasyText.text = data.GetPercentageComplete(PercentageMode.Easy) + "% On Easy";
            percentageNormalText.text = data.GetPercentageComplete(PercentageMode.Normal) + "% On Normal";
            percentageHardText.text = data.GetPercentageComplete(PercentageMode.Hard) + "% On Hard";

            nameText.text = profileId;
            lastPlayedText.text = "Last played on: " + DateTime.FromBinary(data.lastUpdated).ToString();
        }
    }

    public void SetInteractable(bool interactable)
    {
        saveSlotButton.interactable = interactable;
    }
}
public enum PercentageMode
{
    All,
    Easy,
    Normal,
    Hard
}
