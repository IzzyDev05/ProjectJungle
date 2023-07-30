using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingOptionManager : MonoBehaviour
{
    [SerializeField] GameObject craftedItemPrefab;

    [SerializeField] GameObject itemListPanel;

    [SerializeField] Button optionButton;

    [SerializeField] List<ItemManager> itemsNeededList = new List<ItemManager>();
 
    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform child in itemListPanel.transform)
        {
            itemsNeededList.Add(child.GetComponent<CraftingItemManager>().Item);
        }

        AddOnClick();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void CraftItem()
    {
        InventoryManager.Instance.AddToInventory(craftedItemPrefab.GetComponent<CraftingItemManager>().Item);

    }

    void AddOnClick()
    {
        optionButton.onClick.RemoveAllListeners();

        optionButton.onClick.AddListener(CraftItem);
    }
}
