using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Equipement Object", menuName = "Inventory System/Items/Equipment")]
public class EquipmentObject : ItemObject
{
    public EquipmentType equipmentType;

    public void Awake()
    {
        type = ItemType.Equipment;
    }
}
