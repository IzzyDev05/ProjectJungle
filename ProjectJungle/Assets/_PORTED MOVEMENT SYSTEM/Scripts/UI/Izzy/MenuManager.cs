using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject pauseCanvas;
    [SerializeField] private GameObject pauseDefaultButton;
    [SerializeField] private GameObject settingsCanvas;
    [SerializeField] private GameObject settingsDefaultButton;

    private bool isPaused;

    private void Start()
    {
        pauseCanvas.SetActive(false);
        settingsCanvas.SetActive(false);
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
        //Time.timeScale = 0f;
        OpenPauseMenu();

        Cursor.visible = true;
    }

    private void Unpause()
    {
        isPaused = false;
        //Time.timeScale = 1f;
        CloseAllMenus();
        
        Cursor.visible = false;
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

    public void OnQuitPress()
    {
        Application.Quit();
    }
}