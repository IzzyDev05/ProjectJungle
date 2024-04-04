using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject pauseCanvas;
    [SerializeField] private GameObject pauseDefaultButton;
    
    [Header("Settings")]
    [SerializeField] private GameObject settingsCanvas;
    [SerializeField] private GameObject settingsDefaultButton;

    private PlayerManager playerManager;
    private Timer timer;
    private bool isPaused;
    private int defaultX;

    private void Start()
    {
        playerManager = FindObjectOfType<PlayerManager>();
        timer = FindObjectOfType<Timer>();
        
        pauseCanvas.SetActive(false);
        
        settingsCanvas.SetActive(false);
        timer.ShouldAddTime = true;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
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
        OpenPauseMenu();

        Cursor.visible = true;
        playerManager.DisableMovement = true;
    }

    private void Unpause()
    {
        isPaused = false;
        timer.ShouldAddTime = true;
        CloseAllMenus();
        
        Cursor.visible = false;
        playerManager.DisableMovement = false;
    }

    private void OpenPauseMenu()
    {
        pauseCanvas.SetActive(true);
        print(pauseCanvas.activeInHierarchy);
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

    public void OnMainMenu()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OnQuitPress()
    {
        Application.Quit();
    }
}