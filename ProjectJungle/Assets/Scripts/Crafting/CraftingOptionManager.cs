using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingOptionManager : MonoBehaviour
{
    [SerializeField] GameObject craftedItemPrefab;

    [SerializeField] GameObject itemListPanel;

    [SerializeField] Button optionButton;

    [SerializeField] List<CraftingItemManager> itemsNeededList = new List<CraftingItemManager>();

    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform child in itemListPanel.transform)
        {
            itemsNeededList.Add(child.GetComponent<CraftingItemManager>());
        }

        AddOnClick();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void AddOnClick()
    {
        optionButton.onClick.RemoveAllListeners();

        optionButton.onClick.AddListener(CraftItem);
    }


    void CraftItem()
    {
        bool itemRemovedSuccessfully = false;

        foreach (CraftingItemManager craftingItemNeeded in itemsNeededList)
        {
            itemRemovedSuccessfully = InventoryManager.Instance.FindItem(craftingItemNeeded.Item).RemoveMultipleItems(craftingItemNeeded.AmountNeeded);
        }

        if (itemRemovedSuccessfully == true)
        {
            InventoryManager.Instance.AddToInventory(craftedItemPrefab.GetComponent<CraftingItemManager>().Item);

            Debug.Log($"{craftedItemPrefab.name} crafted");
        }
    }
}
