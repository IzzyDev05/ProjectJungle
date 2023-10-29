using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SlotManager : MonoBehaviour
{
    [SerializeField] TMP_Text itemCountText;
    [SerializeField] Image itemImage;

    int maxCapacity = 1;
    [SerializeField] int currentCapacity = 0;

    [SerializeField] ItemManager slotItem;

    Color defaultColor;

    // Start is called before the first frame update
    void Start()
    {
        itemCountText = GetComponentInChildren<TextMeshProUGUI>();

        itemImage = GetComponent<Transform>().GetChild(0).GetComponentInChildren<Image>();

        defaultColor = itemImage.color;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateItemCount();
    }

    public int GetCurrentCapacity { get { return currentCapacity; } }

    int SetMaxCapacity { set { maxCapacity = value; } }

    public int GetMaxCapacity { get { return maxCapacity; } }

    /// <summary>
    /// Returns the Item Manager of the item in the slot
    /// </summary>
    public ItemManager GetSlotItemManager { get { return slotItem; } }

    public void ClearSlot()
    {
        slotItem = null;
        itemImage.sprite = null;

        Destroy(gameObject);
    }

    public void AddItemToSlot(ItemManager item, int amount = 1)
    {
        // If the slot is empty
        if (slotItem == null)
        {
            SetMaxCapacity = item.GetItemObject.GetMaxStackSize;
            currentCapacity = amount;

            slotItem = item;

            itemImage.sprite = item.GetItemObject.GetIcon;
            itemImage.color = Color.white;

            return;
        }

        if (slotItem.GetItemObject == item.GetItemObject && IsSlotFull() == false)
        {
            if (amount == 1)
            {
                currentCapacity++;

                return;
            }

            if (maxCapacity - currentCapacity >= amount)
            {
                currentCapacity += amount;
            }
            else
            {
                int overflowAmount = amount - (maxCapacity - currentCapacity);

                currentCapacity = maxCapacity;

                InventoryManager.Instance.AddToInventory(item, overflowAmount);
            }
            

        }
        else if (IsSlotFull() == true)
        {
            InventoryManager.Instance.AddToInventory(item);
        }

    }

    void UpdateItemCount()
    {
        if (currentCapacity <= 1)
        {
            itemCountText.text = "";
        }
        else
        {
            itemCountText.text = currentCapacity.ToString();
        }
    }

    public void DropItem()
    {
        if (currentCapacity > 1)
        {
            currentCapacity--;

            return;
        }

        RemoveItem();
    }

    public void RemoveItem()
    {
        InventoryManager.Instance.RemoveItemFromInventory(gameObject);

        currentCapacity = 0;
        maxCapacity = 1;
    }

    /// <summary>
    /// Removes a certain amount of a single item. If the item has been removed returns true. By default it removes 1 item.
    /// </summary>
    /// <param name="amount"></param>
    /// <returns></returns>
    public bool RemoveMultipleItems(int amount = 1)
    {
        if (currentCapacity - amount >= 1)
        {
            currentCapacity -= amount;

            return true;
        }
        else if (currentCapacity - amount >= 0)
        {
            RemoveItem();

            return true;
        }

        return false;
    }

    public bool MatchSlotItem(ItemObject item)
    {
        return item == slotItem ? true : false;
    }

    public bool IsSlotFull()
    {
        if (GetCurrentCapacity == GetMaxCapacity)
        {
            return true;
        }

        return false;
    }

    public void SelectSlot()
    {
        if (slotItem == null)
        {
            return;
        }

        SetUpDropButton();

        ItemPanelManager.Instance.ClearDisplay();

        switch (slotItem.GetItemObject.Type)
        {
            case ItemType.Equipment:
                {
                    ItemPanelManager.Instance.DisplaySelectedItem(slotItem.GetChildItem<EquipmentObject>());

                    break;
                }
        }

    }

    void SetUpDropButton()
    {
        Button dropButton = InventoryManager.Instance.GetButton;

        if (dropButton.IsActive() == false)
        {
            dropButton.gameObject.SetActive(true);

        }

        dropButton.onClick.RemoveAllListeners();

        dropButton.onClick.AddListener(DropItem); 
    }

}
