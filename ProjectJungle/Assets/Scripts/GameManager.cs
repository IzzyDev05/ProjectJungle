using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public static GameObject Player;

    [SerializeField] List<ItemManager> startingItems = new List<ItemManager>();

    private void Awake()
    {
        Instance = this;

        Player = GameObject.FindGameObjectWithTag("Player");
    }

    // Start is called before the first frame update
    void Start()
    {
        if (startingItems.Count > 0)
        {
            foreach (ItemManager startingItem in startingItems)
            {
                InventoryManager.Instance.AddToInventory(startingItem);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PauseGame()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Time.timeScale = 0;
    }

    public void UnpauseGame()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Time.timeScale = 1;
    }
}
