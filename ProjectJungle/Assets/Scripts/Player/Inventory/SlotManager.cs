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

    [SerializeField] ItemObject slotItem;
    [SerializeField] GameObject slotGameObject;

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

    public void RemoveItem()
    {
        if (currentCapacity > 1)
        {
            currentCapacity--;

            return;
        }

        InventoryManager.Instance.RemoveItemFromInventory(slotItem);

        currentCapacity = 0;
        maxCapacity = 1;
    }

    public void ClearSlot()
    {
        slotItem = null;
        slotGameObject = null;
        itemImage.sprite = null;
    }

    public void AddItem(ItemObject item, GameObject itemObject)
    {
        if (slotItem == null)
        {
            SetMaxCapacity = item.GetMaxStackSize;
            currentCapacity = 1;

            slotItem = item;
            slotGameObject = itemObject;

            itemImage.sprite = item.GetIcon;
            itemImage.color = Color.white;

            return;
        }

        if (slotItem == item && item.GetStackable && IsSlotFull() == false)
        {
            currentCapacity++;

        }
        else if (IsSlotFull() == true)
        {
            InventoryManager.Instance.AddToInventory(item, itemObject, true);
        }

    }

    public int GetCurrentCapacity { get { return currentCapacity; } }

    int SetMaxCapacity { set { maxCapacity = value; } }

    public int GetMaxCapacity { get { return maxCapacity; } }

    public ItemObject GetItem { get { return slotItem; } }

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
        SetUpDropButton();

        switch (slotItem.Type)
        {
            case ItemType.Equipment:
                {
                    ItemPanelManager.Instance.DisplaySelectedItem(slotGameObject.GetComponent<ItemManager>().GetEquipment);

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

        dropButton.onClick.AddListener(delegate { this.RemoveItem(); });

    }

}
