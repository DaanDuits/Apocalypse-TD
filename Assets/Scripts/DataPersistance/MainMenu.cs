using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public SaveSlotMenu saveSlotsMenu;

    public Button continueButton, loadGameButton;

    private void Start()
    {
        if (!DataPersistanceManager.instance.HasGameData())
        {
            continueButton.interactable = false;
            loadGameButton.interactable = false;
        }
    }
    public void OnNewGameClicked()
    {
        saveSlotsMenu.ActivateMenu(false);
        DeactivateMenu();
    }

    public void OnLoadGameClicked()
    {
        saveSlotsMenu.ActivateMenu(true);
        DeactivateMenu();
    }

    public void OnContinueGameClicked()
    {
        DataPersistanceManager.instance.SaveGame();
        // Load the next scene - which will in turn Load the game because of
        // OnSceneLoaded() in the DataPersistanceManager
        SceneManager.LoadSceneAsync("WorldMap");
    }

    public void ActivateMenu()
    {
        gameObject.SetActive(true);
    }

    public void DeactivateMenu()
    {
        gameObject.SetActive(false);
    }
}
