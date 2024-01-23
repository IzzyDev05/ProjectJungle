using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] Scene newGameScene;
    [SerializeField] Scene settingsScene;

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

    }

    public void SettingButton()
    {

    }

    public void QuitGameButton()
    {
        GameManager.Instance.QuitGame();
    }
    
}
