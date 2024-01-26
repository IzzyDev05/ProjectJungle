using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public static GameObject Player;

    [SerializeField] GameObject settingsUI;

    //[SerializeField] List<ItemManager> startingItems = new List<ItemManager>();

    private void Awake()
    {
        Instance = this;

        Player = GameObject.FindGameObjectWithTag("Player");

        settingsUI.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        /*if (startingItems.Count > 0)
        {
            foreach (ItemManager startingItem in startingItems)
            {
                InventoryManager.Instance.AddToInventory(startingItem);
            }
        }*/
    }

    public GameObject SettingsUI { get { return settingsUI; } }

    public void PauseGame()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Time.timeScale = 0;

        AudioManager.instance.PauseAmbience();
    }

    public void UnpauseGame()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Time.timeScale = 1;

        AudioManager.instance.PauseAmbience(false);
    }
}
