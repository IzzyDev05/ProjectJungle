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
        GameObject item = Instantiate(slotItem.GetComponent<TrinketManager>().TrinketIcon, itemIconParent.transform);
        item.transform.localScale = item.transform.localScale * slotItem.GetComponent<TrinketManager>().ScaleMultiplier;

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

        return slotItem.name == pickedUpItem.name ? true : false;
    }

    public void AddItem(GameObject item)
    {
        AudioManager.instance.PlayOneShot(FModEvents.instance.pickupItem, GameManager.Player.transform.position);

        itemIconParent.SetActive(true);
        Destroy(item);

        isCollected = true;
    }

    public void OpenItemPanel()
    {
        if (isCollected == false)
        {
            return;
        }

        TrinketManager trinket = slotItem.GetComponent<TrinketManager>();

        NewItemViewer.Instance.OpenItemViewer(trinket.TrinketIcon, slotItem.name, trinket.TricketLore, trinket.ScaleMultiplier, trinket.Rotation);
    }

    void SetupButton()
    {
        Button button = GetComponent<Button>();

        button.onClick.RemoveAllListeners();

        button.onClick.AddListener(OpenItemPanel);
    }

    public GameObject SetSlotItem { set { slotItem = value; } }
}
