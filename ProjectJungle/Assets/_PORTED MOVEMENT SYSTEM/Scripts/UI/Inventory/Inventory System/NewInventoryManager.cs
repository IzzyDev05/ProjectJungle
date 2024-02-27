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
        if (eventSystem.firstSelectedGameObject == null) 
        {
            eventSystem.firstSelectedGameObject = slotContainer.transform.GetChild(0).gameObject;
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
        inventoryUI.SetActive(false);
        NewItemViewer.Instance.HideItemViewer();
    }

    /// <summary>
    /// Returns the Inventory UI Game Object
    /// </summary>
    public GameObject GetInventoryUI { get { return inventoryUI; } }

    public GameObject GetSlotContainerUI { get { return slotContainer; } }

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
}
