using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{

    public static InventoryManager Instance;
    [SerializeField] GameObject inventoryUI;
    [SerializeField] GameObject slotPrefab;
    [SerializeField] Button dropItemButton;

    [SerializeField] List<GameObject> inventorySlotList = new List<GameObject>();
    [SerializeField] int unoccupiedSlotIndex = 0;

    [SerializeField] List<ItemObject> itemList = new List<ItemObject>();

    void Awake()
    {
        inventorySlotList.Clear();
        itemList.Clear();

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

        if (dropItemButton.IsActive())
        {
            dropItemButton.gameObject.SetActive(false);
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

    public void RemoveItemFromInventory(ItemObject item)
    {
        int positionInInventory = inventorySlotList.FindIndex(i => i.GetComponent<SlotManager>().MatchSlotItem(item));

        List<ItemObject> updatedList = new List<ItemObject>();
        updatedList = itemList;

        //updatedList.RemoveAt(positionInInventory);

        UpdateItemDisplayInSlot(positionInInventory);

        //itemList = updatedList;
    }

    public void RemoveStackofItemsFromInventory(ItemObject item, GameObject itemObject, int amount)
    {

    }

    void UpdateItemDisplayInSlot(int positionInInventory)
    {
        inventorySlotList[positionInInventory].GetComponent<SlotManager>().ClearSlot();

        foreach (Transform child in inventoryUI.transform)
        {
            if (child.tag == "InventorySlotContainer")
            {
                Debug.Log(child.name);

                /*Destroy(child.GetChild(positionInInventory));
                inventorySlotList.RemoveAt(positionInInventory);

                GameObject newSlot = Instantiate(slotPrefab, child);

                inventorySlotList.Add(newSlot);*/

                return;
            }
        }
    }

    public Button GetButton { get { return dropItemButton; } }
}
