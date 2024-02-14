using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewSlotManager : MonoBehaviour
{
    [SerializeField] GameObject slotItem;
    [SerializeField] GameObject itemIconParent;
    [SerializeField] Image emptyImage;

    bool isCollected = false; 

    private void Awake()
    {
        if (itemIconParent.activeSelf == true)
        {
            itemIconParent.SetActive(false);
        }

        if (isCollected == true)
        {
            isCollected = false;
        }
    }

    private void Start()
    {
        Transform item = Instantiate(slotItem.GetComponent<TrinketManager>().TrinketIcon, itemIconParent.transform).transform;
        item.localScale *= slotItem.GetComponent<TrinketManager>().ScaleMultiplier;
        item.rotation *= slotItem.GetComponent<TrinketManager>().RotationMultiplier;

        this.name = slotItem.name + " Slot";

        SetupButton();
    }

    /// <summary>
    /// Check if the item picked up matches the item for this slot
    /// </summary>
    /// <param name="pickedUpItem">A Game Object of the item that was picked up</param>
    /// <returns>True if the item matched the slot item</returns>
    public bool MatchSlotItem(GameObject pickedUpItem)
    {
        if (slotItem == null)
        {
            Debug.LogError("Slot Item is NULL");
            return false;
        }

        return slotItem.GetComponent<TrinketManager>().TrinketIcon.name == pickedUpItem.GetComponent<TrinketManager>().TrinketIcon.name ? true : false;
    }

    /// <summary>
    /// Add the item to the slot
    /// </summary>
    /// <param name="item"></param>
    public void AddItem(GameObject item)
    {
        AudioManager.instance.PlayOneShot(FModEvents.instance.pickupItem, GameManager.Player.transform.position);

        itemIconParent.SetActive(true);
        Destroy(item);

        isCollected = itemIconParent.activeSelf;
    }

    /// <summary>
    /// Display the information of the item when click on if the item is collected
    /// </summary>
    public void OpenItemPanel()
    {
        if (isCollected == false)
        {
            return;
        }

        NewItemViewer.Instance.OpenItemViewer(slotItem.GetComponent<TrinketManager>());
    }

    /// <summary>
    /// Set up the slots so the item can be displayed when clicked on
    /// </summary>
    void SetupButton()
    {
        Button button = GetComponent<Button>();

        button.onClick.RemoveAllListeners();

        button.onClick.AddListener(OpenItemPanel);
    }

    public GameObject SetSlotItem { set { slotItem = value; } }
}
