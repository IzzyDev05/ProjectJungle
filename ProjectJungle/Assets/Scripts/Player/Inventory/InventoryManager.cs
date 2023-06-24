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

        inventorySlotList.Clear();
        itemList.Clear();
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

        if (inventoryUI.activeSelf == true)
        {
            inventoryUI.SetActive(false);
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
        ItemPanelManager.Instance.ClearDisplay();

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

    public void AddToInventory(ItemObject item, GameObject itemObject, bool addToNewSlot = false)
    {
        if (!addToNewSlot)
        {
            for (int i = 0; i < itemList.Count; i++)
            {
                if (itemList[i] == item && !inventorySlotList[i].GetComponent<SlotManager>().IsSlotFull())
                {
                    inventorySlotList[i].GetComponent<SlotManager>().AddItem(item, itemObject);

                    return;
                }
            }
        }


        inventorySlotList[unoccupiedSlotIndex].GetComponent<SlotManager>().AddItem(item, itemObject);
        itemList.Add(item);
        unoccupiedSlotIndex++;
    }

}
