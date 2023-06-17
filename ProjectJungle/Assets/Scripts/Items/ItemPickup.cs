using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    [SerializeField] ItemObject itemObject;

    public static ItemPickup Instance;

    private void Awake()
    {
        Instance = this;
    }

    public ItemObject PickupItem()
    {
        return itemObject;
    }
}
