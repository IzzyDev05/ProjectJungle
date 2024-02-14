using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] string newGameSceneName;
    [SerializeField] string settingsSceneName;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NewGameButton()
    {
        if (newGameSceneName == "")
        {
            Debug.LogError("Missing new game scene name");
        }
        else
        {
            SceneManager.LoadScene(newGameSceneName);
        }
    }

    public void SettingButton()
    {
        if (settingsSceneName == "")
        {
            Debug.Log("Settings stuff");
        }
        else
        {
            SceneManager.LoadScene(settingsSceneName);
        }
    }

    public void QuitGameButton()
    {
#if UNITY_EDITOR

        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

}
