using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Mob Drop Object", menuName = "Inventory System/Items/Mob Drop")]
public class MobDropObject : ItemObject
{
    public void Awake()
    {
        Type = ItemType.MobDrop;
    }
}
