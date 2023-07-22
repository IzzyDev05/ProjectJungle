using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{

    public static InventoryManager Instance;

    [SerializeField] GameObject inventoryUI;
    [SerializeField] GameObject slotContainer;
    [SerializeField] GameObject slotPrefab;
    [SerializeField] Button dropItemButton;

    [SerializeField] List<GameObject> inventorySlotList = new List<GameObject>();
    [SerializeField] int t_slotCount = 0;
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
                t_slotCount++;
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

    public void AddToInventory(ItemManager item, bool addToNewSlot = false, int amount = 1)
    {
        if (!addToNewSlot)
        {
            for (int i = 0; i < itemList.Count; i++)
            {
                if (itemList[i] == item.GetItemObject && inventorySlotList[i].GetComponent<SlotManager>().IsSlotFull() == false)
                {
                    inventorySlotList[i].GetComponent<SlotManager>().AddItem(item, amount);

                    return;
                }
            }
        }

        FindNextUnoccupiedSlot();

        inventorySlotList[GetUnoccupiedSlotIndex].GetComponent<SlotManager>().AddItem(item, amount);
        itemList.Add(item.GetItemObject);
        unoccupiedSlotIndex++;
    }

    void FindNextUnoccupiedSlot()
    {
        unoccupiedSlotIndex = 0;

        foreach (GameObject slotObject in inventorySlotList)
        {
            if (slotObject.GetComponent<SlotManager>().GetItem == null)
            {
                return;
            }

            unoccupiedSlotIndex++;
        }
    }

    public int GetUnoccupiedSlotIndex { get { return unoccupiedSlotIndex; } }

    public void RemoveItemFromInventory(GameObject currentSlot)
    {
        int positionInInventory = inventorySlotList.FindIndex(slot => slot.name == currentSlot.name);

        List<ItemObject> updatedList = new List<ItemObject>();
        updatedList = itemList;

        UpdateItemDisplayInSlot(positionInInventory);

        updatedList.RemoveAt(positionInInventory);

        itemList = updatedList;
    }

    void UpdateItemDisplayInSlot(int positionInInventory)
    {
        inventorySlotList[positionInInventory].GetComponent<SlotManager>().ClearSlot();

        Destroy(slotContainer.transform.GetChild(positionInInventory).gameObject);
        inventorySlotList.RemoveAt(positionInInventory);

        GameObject newSlot = Instantiate(slotPrefab, slotContainer.transform);
        newSlot.name = $"ItemSlot ({t_slotCount})";

        ItemPanelManager.Instance.ClearDisplay();
    }

    public Button GetButton { get { return dropItemButton; } }

    public List<ItemObject> GetInventoryItems { get { return itemList; } }

    public List<GameObject> GetInventorySlots { get { return inventorySlotList; } }

}
