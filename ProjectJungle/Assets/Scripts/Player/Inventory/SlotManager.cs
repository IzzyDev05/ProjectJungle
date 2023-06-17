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
    [SerializeField] ItemObject slotItem;
    [SerializeField] int currentCapacity;

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
        if (currentCapacity > 0)
        {
            currentCapacity--;
        }
        else
        {
            currentCapacity = 0;
        }
    }

    public void AddItem(ItemObject item)
    {
        if (slotItem == null)
        {
            SetMaxCapacity = item.maxStackSize;
            currentCapacity = 1;

            slotItem = item;

            itemImage.sprite = item.icon;
            itemImage.color = Color.white;

            return;
        }

        if (item.stackable && currentCapacity != maxCapacity)
        {
            currentCapacity++;

        }
        else if (currentCapacity == maxCapacity)
        {
            InventoryManager.Instance.AddToInventory(item, true);
        }

    }

    int SetMaxCapacity
    {
        set
        {
            maxCapacity = value;
        }
    }

    int GetMaxCapacity {
        get
        {
            return maxCapacity;
        }
    }
}
