using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.UI;

public class InventoryManager : MonoBehaviour
{

    public static InventoryManager Instance;
    [SerializeField] GameObject inventoryUI;

    [SerializeField] List<GameObject> inventorySlotList = new List<GameObject>();
    [SerializeField] int unoccupiedSlotIndex = 0;

    [SerializeField] List<ItemObject> itemList = new List<ItemObject>();

    void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (inventoryUI == null)
        {
            inventoryUI = GameObject.FindGameObjectWithTag("Inventory");
        }

        foreach (Transform child in inventoryUI.GetComponentsInChildren<Transform>())
        {
            if (child.gameObject.CompareTag("InventorySlot"))
            {
                inventorySlotList.Add(child.gameObject);
            }    
        }
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

    public void AddToInventory(ItemObject item, bool addToNewSlot = false)
    {
        if (!addToNewSlot)
        {
            foreach (ItemObject savedItem in itemList)
            {
                if (savedItem == item)
                {
                    inventorySlotList[itemList.IndexOf(item)].GetComponent<SlotManager>().AddItem(item);

                    return;
                }
            }
        }

        inventorySlotList[unoccupiedSlotIndex].GetComponent<SlotManager>().AddItem(item);
        itemList.Add(item);
        unoccupiedSlotIndex++;
    }
}
