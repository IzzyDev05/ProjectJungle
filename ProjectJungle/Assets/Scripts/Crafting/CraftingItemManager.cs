using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CraftingItemManager : MonoBehaviour
{
    [SerializeField] Image itemImage;
    [SerializeField] TMP_Text amountText;

    [SerializeField] ItemManager item;

    [SerializeField] int amount = 1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (item != null)
        {
            itemImage.sprite = item.GetItemObject.GetIcon;
        }

        if (amount > 1)
        {
            amountText.text = amount.ToString();
        }
        else
        {
            amountText.text = "";
        }
    }

    /// <summary>
    /// Returns the item that is part of the crafting
    /// </summary>
    public ItemManager Item { get { return item; } }

    /// <summary>
    /// Returns the amount of the item needed for the crafting
    /// </summary>
    public int AmountNeeded { get { return amount; } }
}
