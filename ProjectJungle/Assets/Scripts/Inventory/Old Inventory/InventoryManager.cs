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

        #region SINGLETON
        if (Instance != null)
        {
            Debug.LogError("Multiple Inventory Manager Instances found.");
        }

        Instance = this;
        #endregion
    }

    // Start is called before the first frame update
    void Start()
    {
        if (inventoryUI == null)
        {
            inventoryUI = GameObject.FindGameObjectWithTag("Inventory");
        }

        // Gets the each inventory slot
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

    /// <summary>
    /// Opens inventory UI
    /// </summary>
    public void OpenInventory()
    {
        inventoryUI.SetActive(true);
    }

    /// <summary>
    /// Close Inventory UI
    /// </summary>
    public void CloseInventory()
    {
        ItemPanelManager.Instance.ClearDisplay();

        inventoryUI.SetActive(false);
    }

    /// <summary>
    /// Returns the Inventory UI Game Object
    /// </summary>
    public GameObject GetInventoryUI
    {
        get
        {
            return inventoryUI;
        }
    }

    /// <summary>
    /// Adds an Item into the inventory
    /// </summary>
    /// <param name="itemToAdd"></param>
    /// <param name="amount"></param>
    public void AddToInventory(ItemManager itemToAdd, int amount = 1)
    {
        SlotManager foundSlot = FindIteminInventory(itemToAdd, true);
        // If the item already exists in the inventory and the item is stackable
        if (foundSlot.GetSlotItemManager != null && itemToAdd.GetItemObject.Stackable == true)
        {
            foreach (GameObject slotGameObject in inventorySlotList)
            {
                if (slotGameObject.GetComponent<SlotManager>() == foundSlot)
                {
                    foundSlot.AddItemToSlot(itemToAdd, amount);

                    return;
                }
            }
        }
        
        FindNextUnoccupiedSlot();

        inventorySlotList[GetUnoccupiedSlotIndex].GetComponent<SlotManager>().AddItemToSlot(itemToAdd, amount);
        itemList.Add(itemToAdd.GetItemObject);
        unoccupiedSlotIndex++;
    }

    /// <summary>
    /// Find the first unoccuiped slot in the inventory
    /// </summary>
    void FindNextUnoccupiedSlot()
    {
        unoccupiedSlotIndex = 0;

        foreach (GameObject slotObject in inventorySlotList)
        {
            if (slotObject.GetComponent<SlotManager>().GetSlotItemManager == null)
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

        AudioManager.instance.PlayOneShot(FModEvents.instance.dropItem, GameManager.Player.transform.position);
    }

    void UpdateItemDisplayInSlot(int positionInInventory)
    {
        inventorySlotList[positionInInventory].GetComponent<SlotManager>().ClearSlot();

        inventorySlotList.RemoveAt(positionInInventory);

        GameObject newSlot = Instantiate(slotPrefab, slotContainer.transform);
        newSlot.name = $"ItemSlot ({t_slotCount})";

        ItemPanelManager.Instance.ClearDisplay();
    }

    /// <summary>
    /// Returns the first slot with the item. If FindUnfilledStack is true, it will return the first slot with the item that is not full
    /// </summary>
    /// <param name="itemToFind"></param>
    /// <param name="FindUnfilledStack"></param>
    public SlotManager FindIteminInventory(ItemManager itemToFind, bool FindUnfilledStack = false)
    {
        foreach (GameObject slotGameObject in inventorySlotList)
        {
            SlotManager slot = slotGameObject.GetComponent<SlotManager>();
            ItemManager itemInSlot = slot.GetSlotItemManager;

            if (itemToFind.CompareItem(itemInSlot) == false)
            {
                continue; // Skipp current iteration
            }

            if (FindUnfilledStack == true && slot.IsSlotFull() == false)
            {
                return slot;
            }
            else if (FindUnfilledStack == false)
            {
                return slot;
            }
        }

        return slotPrefab.GetComponent<SlotManager>();
    }

    /// <summary>
    /// Returns the button object for dropping items
    /// </summary>
    public Button GetButton { get { return dropItemButton; } }

    /// <summary>
    /// Returns the list of items in the inventory
    /// </summary>
    public List<ItemObject> GetInventoryItems { get { return itemList; } }

    /// <summary>
    /// Returns the list of slots in the inventory
    /// </summary>
    public List<GameObject> GetInventorySlots { get { return inventorySlotList; } }

}
