using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


[RequireComponent(typeof(DynamicInventorySlot))]
public class NewInventoryManager : MonoBehaviour
{
    public static NewInventoryManager Instance;

    [SerializeField] GameObject inventoryUI;
    [SerializeField] GameObject slotContainer;
    [SerializeField] DynamicInventorySlot slotAdder;

    [SerializeField] List<GameObject> slotList;

    private void Awake()
    {
        #region SINGLETON
        if (Instance != null)
        {
            Debug.LogError("Multiple Inventory Manager Instances found.");
        }

        Instance = this;
        #endregion

        inventoryUI = GameObject.FindGameObjectWithTag("Inventory");
        slotContainer = GameObject.FindGameObjectWithTag("InventorySlotContainer");
        slotAdder = GetComponent<DynamicInventorySlot>();
    }

    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform slot in slotContainer.GetComponent<Transform>())
        {
            slotList.Add(slot.gameObject);
        }

        if (inventoryUI.activeSelf == true)
        {
            inventoryUI.SetActive(false);
        }

        EventSystem eventSystem = EventSystem.current;
        if (eventSystem.firstSelectedGameObject == null && slotAdder.TrinketCount > 0) 
        {
            eventSystem.firstSelectedGameObject = slotContainer.transform.GetChild(0).gameObject;
        }
    }

    #region UI_Controls
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
        inventoryUI.SetActive(false);
        NewItemViewer.Instance.HideItemViewer();
    }
    #endregion

    #region Getters
    /// <summary>
    /// Returns the Inventory UI Game Object
    /// </summary>
    public GameObject GetInventoryUI { get { return inventoryUI; } }

    public GameObject GetSlotContainerUI { get { return slotContainer; } }
    #endregion

    /// <summary>
    /// Adds an Item into the inventory
    /// </summary>
    /// <param name="itemToAdd"></param>
    /// <param name="amount"></param>
    public void AddToInventory(GameObject item)
    {
        bool pickedUp = false;

        foreach(GameObject slot in slotList)
        {
            NewSlotManager slotManager = slot.GetComponent<NewSlotManager>();

            if (slotManager.MatchSlotItem(item) == true)
            {
                slotManager.AddItem(item);
                pickedUp = true;
            }
        }

        if (pickedUp)
        {
            AudioManager.Instance.PlayOneShot(FModEvents.Instance.pickupItem, GameManager.Player.transform.position);
        }
    }

    /// <summary>
    /// Checks if the inventory has all the slot items
    /// </summary>
    /// <returns>Returns true if all trinkets are collected. Otherwise return false.</returns>
    public bool AllTrinketsCollected()
    {
        bool allCollected = false;

        if (slotList.Count == 0)
        {
            return true;
        }

        foreach (GameObject slot in slotList) 
        {
            NewSlotManager slotManager = slot.GetComponent<NewSlotManager>();

            if (slotManager.IsCollected)
            {
                allCollected = true;
            }
            else
            {
                return false;
            }
        }

        return allCollected;
    }
}
