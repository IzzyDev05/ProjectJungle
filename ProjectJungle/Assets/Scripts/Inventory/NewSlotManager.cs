using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewSlotManager : MonoBehaviour
{
    [SerializeField] GameObject slotItem;
    [SerializeField] Image itemImage;
    [SerializeField] Image emptyImage;


    private void Awake()
    {
        if (itemImage.gameObject.activeSelf == true)
        {
            itemImage.gameObject.SetActive(false);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Check if the item picked up matches the item for this slot
    /// </summary>
    /// <param name="pickedUpItem">A Game Object of the item that was picked up</param>
    /// <returns>True if the item matched the slot item</returns>
    public bool MatchSlotItem(GameObject pickedUpItem)
    {
        return slotItem == pickedUpItem ? true : false;
    }

    public void AddItem()
    {
        itemImage.gameObject.SetActive(true);
    }
}
