using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SlotManager : MonoBehaviour
{
    [SerializeField] TMP_Text itemCountText;
    [SerializeField] Image itemImage;

    int maxCapacity = 1;
    [SerializeField] int currentCapacity;

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

    public ItemManager GetItem { get { return slotItem; } }

    public void ClearSlot()
    {
        slotItem = null;
        itemImage.sprite = null;
    }

    public void AddItem(ItemManager item, int amount = 1)
    {
        if (slotItem == null)
        {
            SetMaxCapacity = item.GetItemObject.GetMaxStackSize;
            currentCapacity = amount;

            slotItem = item;

            itemImage.sprite = item.GetItemObject.GetIcon;
            itemImage.color = Color.white;

            return;
        }

        if (slotItem.GetItemObject == item.GetItemObject && item.GetItemObject.GetStackable && IsSlotFull() == false)
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
                int overflow = amount - (maxCapacity - currentCapacity);

                currentCapacity = maxCapacity;

                InventoryManager.Instance.AddToInventory(item, true, overflow);
            }
            

        }
        else if (IsSlotFull() == true)
        {
            InventoryManager.Instance.AddToInventory(item, true);
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

    public void RemoveMultipleItems(int amount = 1)
    {
        if (currentCapacity - amount >= 1)
        {
            currentCapacity -= amount;
        }
        else if (currentCapacity - amount >= 0)
        {
            RemoveItem();
        }
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

        switch (slotItem.GetItemObject.Type)
        {
            case ItemType.Equipment:
                {
                    ItemPanelManager.Instance.DisplaySelectedItem(slotItem.GetEquipment);

                    break;
                }
            case ItemType.Food:
                {
                    ItemPanelManager.Instance.DisplaySelectedItem(slotItem.GetFood);

                    break;
                }
            case ItemType.MobDrop:
                {
                    ItemPanelManager.Instance.DisplaySelectedItem(slotItem.GetMobDrop);

                    break;
                }
            case ItemType.Resource:
                {
                    ItemPanelManager.Instance.DisplaySelectedItem(slotItem.GetResource);

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
