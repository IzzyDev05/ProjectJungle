using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingOptionManager : MonoBehaviour
{
    [SerializeField] GameObject craftedItemPrefab;

    [SerializeField] GameObject itemListPanel;

    [SerializeField] Button optionButton;

    [SerializeField] List<GameObject> itemsNeededList = new List<GameObject>();
 
    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform child in itemListPanel.transform)
        {
            itemsNeededList.Add(child.GetComponent<CraftingItemManager>().gameObject);
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
        if (CheckForItemsNeeded())
        {
            

            InventoryManager.Instance.AddToInventory(craftedItemPrefab.GetComponent<CraftingItemManager>().Item);
        }
    }

    bool CheckForItemsNeeded()
    {
        foreach (ItemObject item in InventoryManager.Instance.GetInventoryItems)
        {
            foreach (GameObject itemNeededObject in itemsNeededList)
            {
                CraftingItemManager craftingItemNeeded = itemNeededObject.GetComponent<CraftingItemManager>();

                if (item == craftingItemNeeded.Item.GetItemObject)
                {
                    return true;
                }
            }
        }
       

        return false;
    }

    void RemoveNeededItems()
    {

    }
}
