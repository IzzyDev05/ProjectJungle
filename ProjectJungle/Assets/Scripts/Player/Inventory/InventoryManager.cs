using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.UI;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] GameObject inventoryUI;

    // Start is called before the first frame update
    void Start()
    {
        if (inventoryUI == null)
        {
            inventoryUI = GameObject.FindGameObjectWithTag("Inventory");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenInventory()
    {
        inventoryUI.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Time.timeScale = 0;
    }

    public void CloseInventory()
    {
        inventoryUI.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Time.timeScale = 1;
    }

    public GameObject GetInventoryUI
    {
        get
        {
            return inventoryUI;
        }
    }
}
