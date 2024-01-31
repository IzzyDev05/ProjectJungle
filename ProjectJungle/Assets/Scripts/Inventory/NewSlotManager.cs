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

    private void Start()
    {
        itemImage.sprite = slotItem.GetComponent<TrinketManager>().TrinketSprite;

        this.name = slotItem.name + " Slot";
    }

    /// <summary>
    /// Check if the item picked up matches the item for this slot
    /// </summary>
    /// <param name="pickedUpItem">A Game Object of the item that was picked up</param>
    /// <returns>True if the item matched the slot item</returns>
    public bool MatchSlotItem(GameObject pickedUpItem)
    {
        if (slotItem == null)
        {
            Debug.LogError("Slot Item is NULL");
            return false;
        }

        return slotItem.name == pickedUpItem.name ? true : false;
    }

    public void AddItem(GameObject item)
    {
        AudioManager.instance.PlayOneShot(FModEvents.instance.pickupItem, GameManager.Player.transform.position);

        itemImage.gameObject.SetActive(true);
        Destroy(item);
    }
}
