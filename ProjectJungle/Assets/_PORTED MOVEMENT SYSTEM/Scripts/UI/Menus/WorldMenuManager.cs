using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WorldMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject mainMenuCam;
    [SerializeField] private GameObject settingsCam;
    [SerializeField] private GameObject winCam;
    [SerializeField] private GameObject worldMenus;

    private void Start()
    {
        worldMenus.SetActive(true);
        mainMenuCam.SetActive(true);
        settingsCam.SetActive(false);
        winCam.SetActive(false);
        
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }

    public void StartGame()
    {
        worldMenus.SetActive(false);
        mainMenuCam.SetActive(false);
        settingsCam.SetActive(false);
        winCam.SetActive(false);
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void Settings()
    {
        mainMenuCam.SetActive(false);
        settingsCam.SetActive(true);
        winCam.SetActive(false);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        
        mainMenuCam.SetActive(true);
        settingsCam.SetActive(false);
        winCam.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}