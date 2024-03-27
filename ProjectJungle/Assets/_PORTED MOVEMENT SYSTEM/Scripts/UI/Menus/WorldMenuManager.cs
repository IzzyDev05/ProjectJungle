using System;
using UnityEngine;

public class WorldMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject mainMenuCam;
    [SerializeField] private GameObject settingsCam;
    [SerializeField] private GameObject worldMenus;

    private void Start()
    {
        worldMenus.SetActive(true);
        mainMenuCam.SetActive(true);
        settingsCam.SetActive(false);

        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }

    public void StartGame()
    {
        worldMenus.SetActive(false);
        mainMenuCam.SetActive(false);
        settingsCam.SetActive(false);
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void Settings()
    {
        mainMenuCam.SetActive(false);
        settingsCam.SetActive(true);
    }

    public void MainMenu()
    {
        mainMenuCam.SetActive(true);
        settingsCam.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}