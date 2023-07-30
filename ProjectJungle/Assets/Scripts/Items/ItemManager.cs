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

    public ItemObject GetItemObject { get { return itemObject; } }

    public int GetAmountPickedUp { get { return pickupAmount; } }

    public EquipmentObject GetEquipment { get { return eqipmentObjectScript != null ? eqipmentObjectScript : null; } }
    public FoodObject GetFood { get { return foodObjectScript != null ? foodObjectScript : null; } }
    public MobDropObject GetMobDrop { get { return mobDropObjectScript != null ? mobDropObjectScript : null; } }
    public ResourceObject GetResource { get { return resourceObjectScript != null ? resourceObjectScript : null; } }

}
