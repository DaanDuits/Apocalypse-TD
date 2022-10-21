using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveSlotMenu : MonoBehaviour
{
    public MainMenu mainMenu;
    public NewGameMenu newGameMenu;
    bool isLoadingGame;

    SaveSlot[] saveSlots;

    private void Awake()
    {
        saveSlots = this.GetComponentsInChildren<SaveSlot>();
    }

    public void OnBackClicked()
    {
        mainMenu.ActivateMenu();
        DeactivateMenu();
    }

    public void OnSaveSlotClicked(SaveSlot saveSlot)
    {
        // update the selected profile id to be used for data persistance
        DataPersistanceManager.instance.ChangeSelectedProfileId(saveSlot.profileId);

        // create a new game - which will initialize our data to a clean slate
        if (!isLoadingGame)
        {
            newGameMenu.gameObject.SetActive(true);
            DeactivateMenu();
            return;
        }

        Dictionary<string, GameData> profilesGameData = DataPersistanceManager.instance.GetAllProfilesGameData();
        GameData profileData = null;
        profilesGameData.TryGetValue(saveSlot.profileId, out profileData);
        PlayerPrefs.SetString("Seed", profileData.seed);
        PlayerPrefs.SetInt("DefaultDifficulty", profileData.defaultDifficulty);

        DataPersistanceManager.instance.SaveGame();
        // load the scene - which will in turn save the game because of OnSceneUnloaded() in the DataPersistanceManager
        SceneManager.LoadSceneAsync("WorldMap");
    }

    public void ActivateMenu(bool _isLoadingGame)
    {
        gameObject.SetActive(true);

        isLoadingGame = _isLoadingGame;

        // Load all of the profiles that exist
        Dictionary<string, GameData> profilesGameData = DataPersistanceManager.instance.GetAllProfilesGameData();

        // loop throug each of the save slot in the UI and set the content appropriately
        foreach (SaveSlot saveSlot in saveSlots)
        {
            GameData profileData = null;
            profilesGameData.TryGetValue(saveSlot.profileId, out profileData);
            saveSlot.SetData(profileData);
            if (profileData == null && _isLoadingGame)
                saveSlot.SetInteractable(false);
            else
                saveSlot.SetInteractable(true);
        }
    }

    public void DeactivateMenu()
    {
        gameObject.SetActive(false);
    }
}
