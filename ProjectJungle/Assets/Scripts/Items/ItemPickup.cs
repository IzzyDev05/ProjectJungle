using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    [SerializeField] ItemObject itemObject;
    [SerializeField] int pickupAmount = 1;

    public ItemObject PickupItem() { return itemObject; }

    public int GetAmountPickedUp { get { return pickupAmount; } }
}
