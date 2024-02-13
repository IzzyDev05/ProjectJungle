using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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
    }

    // Start is called before the first frame update
    void Start()
    {
        if (inventoryUI == null)
        {
            inventoryUI = GameObject.FindGameObjectWithTag("Inventory");
        }

        foreach (Transform slot in slotContainer.GetComponent<Transform>())
        {
            slotList.Add(slot.gameObject);
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
        inventoryUI.SetActive(false);
        NewItemViewer.Instance.HideItemViewer();
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
    public void AddToInventory(GameObject item)
    {
        foreach(GameObject slot in slotList)
        {
            NewSlotManager slotManager = slot.GetComponent<NewSlotManager>();

            if (slotManager.MatchSlotItem(item) == true)
            {
                slotManager.AddItem(item);
            }
        }

        AudioManager.instance.PlayOneShot(FModEvents.instance.pickupItem, GameManager.Player.transform.position);
    }
}
