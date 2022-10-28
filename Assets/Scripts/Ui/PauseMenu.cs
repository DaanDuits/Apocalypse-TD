using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseUI;

    public GameObject EndUI;

    public MenuMode mode;
    void Update()
    {
        if (EndUI != null)
        {
            if (EndUI.activeSelf)
            {
                mode = MenuMode.Disabled;
            }
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (LevelBehaviour.instance != null)
            {
                LevelBehaviour.instance.onExit();
            }
            switch (mode)
            {
                case MenuMode.Off:
                    pauseUI.SetActive(false);
                    mode = MenuMode.On;
                    break;
                case MenuMode.On:
                    pauseUI.SetActive(true);
                    mode = MenuMode.Off;
                    break;
                case MenuMode.Disabled:
                    mode = MenuMode.On;
                    break;
            }
        }
    }

    public void OnWorldMap()
    {
        SceneManager.LoadSceneAsync("WorldMap");
    }

    public void OnMainMenu()
    {
        SceneManager.LoadSceneAsync("MainMenu");
    }

    public void OnQuit()
    {
        Application.Quit();
    }
}
public enum MenuMode
{
    On,
    Off,
    Disabled
}
