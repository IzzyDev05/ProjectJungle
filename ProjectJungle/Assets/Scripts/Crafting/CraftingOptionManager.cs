using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingOptionManager : MonoBehaviour
{
    [SerializeField] GameObject craftingItemPrefab;

    [SerializeField] GameObject itemListPanel;

    [SerializeField] List<ItemObject> itemsNeededList = new List<ItemObject>();
 
    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform child in itemListPanel.transform)
        {
            itemsNeededList.Add(child.GetComponent<CraftingItemManager>().Item);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
