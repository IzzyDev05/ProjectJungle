using UnityEngine;

public class ItemManager : MonoBehaviour
{
    [SerializeField] ItemObject itemObject;
    [SerializeField] int pickupAmount = 1;

    public ItemManager PickupItem()
    {
        AudioManager.instance.PlayOneShot(FModEvents.instance.pickupItem, transform.position);

        gameObject.SetActive(false);

        return this;
    }

    /// <summary>
    /// Returns the parent scriptable object for every item
    /// </summary>
    public ItemObject GetItemObject { get { return itemObject; } }

    /// <summary>
    /// Returns the item as an Equipment item
    /// </summary>
    public EquipmentObject GetEquipmentObject { get { return itemObject as EquipmentObject; } }

    /// <summary>
    /// Returns the number of items to pick up
    /// </summary>
    public int AmountPickedUp { get { return pickupAmount; } }

    /// <summary>
    /// Returns true if the item to compare is the same item, otherwise return false
    /// </summary>
    /// <param name="itemToCompare"></param>
    public bool CompareItem(ItemManager itemToCompare)
    {
        if (itemToCompare == null)
        {
            return false;
        }

        if (itemObject.ItemType != itemToCompare.GetItemObject.ItemType)
        {
            return false;
        }

        if (itemObject.ItemName == itemToCompare.GetItemObject.ItemName)
        {
            return true;
        }

        /*switch (itemObject.ItemType)
        {
            case ItemType.Equipment:
                {
                    if (GetEquipmentObject == itemToCompare.GetEquipmentObject)
                    {
                        return true;
                    }

                    break;
                }
        }*/

        return false;
    }
}

