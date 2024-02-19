using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public static GameObject Player;

    [SerializeField] GameObject settingsUI;

    private void Awake()
    {
        Instance = this;

        Player = GameObject.FindGameObjectWithTag("Player");

        settingsUI.SetActive(false);
    }

    public GameObject SettingsUI { get { return settingsUI; } }

    /// <summary>
    /// Open setting UI.
    /// </summary>
    public void OpenMenuUI()
    {
        settingsUI.SetActive(true);
    }

    /// <summary>
    /// Close the setting UI.
    /// </summary>
    public void CloseMenuUI()
    {
        settingsUI.SetActive(false);
    }

    /// <summary>
    /// Pause Game.
    /// </summary>
    public void PauseGame()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Time.timeScale = 0;

        AudioManager.Instance.PauseAmbience();
    }

    /// <summary>
    /// Unpause Game.
    /// </summary>
    public void UnpauseGame()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Time.timeScale = 1;

        AudioManager.Instance.PauseAmbience(false);
    }
}