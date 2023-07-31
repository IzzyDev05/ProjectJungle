using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    [SerializeField] EquipmentObject eqipmentObjectScript;
    [SerializeField] FoodObject foodObjectScript;
    [SerializeField] MobDropObject mobDropObjectScript;
    [SerializeField] ResourceObject resourceObjectScript;

    [SerializeField] ItemObject itemObject;
    [SerializeField] int pickupAmount = 1;

    private void Awake()
    {
        if (eqipmentObjectScript != null)
        {
            itemObject = GetEquipment;
        }
        else if (foodObjectScript != null)
        {
            itemObject = GetFood;
        }
        else if (mobDropObjectScript != null)
        {
            itemObject = GetMobDrop;
        }
    }

    public ItemManager PickupItem() { return this; }

    /// <summary>
    /// Returns the parent scriptable object for every item
    /// </summary>
    public ItemObject GetItemObject { get { return itemObject; } }

    /// <summary>
    /// Returns the number of items to pick up
    /// </summary>
    public int GetAmountPickedUp { get { return pickupAmount; } }

    /// <summary>
    /// Returns the scriptable object for every equipment item
    /// </summary>
    public EquipmentObject GetEquipment { get { return eqipmentObjectScript != null ? eqipmentObjectScript : null; } }
    /// <summary>
    /// Returns the scriptable object for every food item
    /// </summary>
    public FoodObject GetFood { get { return foodObjectScript != null ? foodObjectScript : null; } }
    /// <summary>
    /// Returns the scriptable object for every mob drop item
    /// </summary>
    public MobDropObject GetMobDrop { get { return mobDropObjectScript != null ? mobDropObjectScript : null; } }
    /// <summary>
    /// Returns the scriptable object for every resource item
    /// </summary>
    public ResourceObject GetResource { get { return resourceObjectScript != null ? resourceObjectScript : null; } }

}
