using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject pauseCanvas;
    [SerializeField] private GameObject pauseDefaultButton;
    
    [Header("Settings")]
    [SerializeField] private GameObject settingsCanvas;
    [SerializeField] private GameObject settingsDefaultButton;
    /*[SerializeField] private GameObject baseSettings;
    [SerializeField] private GameObject audioSettings;
    [SerializeField] private GameObject videoSettings;
    [SerializeField] private GameObject gameplaySettings;*/

    private PlayerManager playerManager;
    private Timer timer;
    private bool isPaused;

    private void Start()
    {
        playerManager = FindObjectOfType<PlayerManager>();
        timer = FindObjectOfType<Timer>();
        
        pauseCanvas.SetActive(false);
        
        settingsCanvas.SetActive(false);
        /*baseSettings.SetActive(true);
        audioSettings.SetActive(false);
        videoSettings.SetActive(false);
        gameplaySettings.SetActive(false);*/

        timer.ShouldAddTime = true;
    }

    private void Update()
    {
        if (InputManagerUI.Instance.MenuOpenCloseInput)
        {
            if (!isPaused) Pause();
            else Unpause();
        }
    }

    private void Pause()
    {
        isPaused = true;
        timer.ShouldAddTime = false;
        Time.timeScale = 0.5f;
        OpenPauseMenu();

        Cursor.visible = true;
        playerManager.DisableMovement = true;
    }

    private void Unpause()
    {
        isPaused = false;
        timer.ShouldAddTime = true;
        Time.timeScale = 1f;
        CloseAllMenus();
        
        Cursor.visible = false;
        playerManager.DisableMovement = false;
    }

    private void OpenPauseMenu()
    {
        pauseCanvas.SetActive(true);
        settingsCanvas.SetActive(false);
        
        EventSystem.current.SetSelectedGameObject(pauseDefaultButton);
    }
    
    private void OpenSettingsMenu()
    {
        settingsCanvas.SetActive(true);
        pauseCanvas.SetActive(false);
        
        EventSystem.current.SetSelectedGameObject(settingsDefaultButton);
    }

    private void CloseAllMenus()
    {
        pauseCanvas.SetActive(false);
        settingsCanvas.SetActive(false);
        
        EventSystem.current.SetSelectedGameObject(null);
    }
    
    // === BUTTON FUNCTIONS === //
    public void OnSettingsPress()
    {
        OpenSettingsMenu();
    }

    public void OnResumePress()
    {
        Unpause();
    }

    public void OnSettingsBack()
    {
        OpenPauseMenu();
    }

    /*public void OnSubSettingsBack()
    {
        baseSettings.SetActive(true);
        audioSettings.SetActive(false);
        videoSettings.SetActive(false);
        gameplaySettings.SetActive(false);
    }

    public void OnSubSetting(string type)
    {
        switch (type)
        {
            case ("Audio"):
                baseSettings.SetActive(false);
                audioSettings.SetActive(true);
                videoSettings.SetActive(false);
                gameplaySettings.SetActive(false);
                break;
            case ("Video"):
                baseSettings.SetActive(false);
                audioSettings.SetActive(false);
                videoSettings.SetActive(true);
                gameplaySettings.SetActive(false);
                break;
            case ("Gameplay"):
                baseSettings.SetActive(false);
                audioSettings.SetActive(false);
                videoSettings.SetActive(false);
                gameplaySettings.SetActive(true);
                break;
        }
    }*/

    public void OnQuitPress()
    {
        Application.Quit();
    }
}