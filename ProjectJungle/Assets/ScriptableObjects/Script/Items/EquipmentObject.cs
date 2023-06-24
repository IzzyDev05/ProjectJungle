using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Equipement Object", menuName = "Inventory System/Items/Equipment")]
public class EquipmentObject : ItemObject
{
    public EquipmentType equipmentType;
    public int damage;
    public int defense;
    public float attackSpeed;
    public float range;

    public void Awake()
    {
        Type = ItemType.Equipment;

    }

    public int GetDamage { get { return damage; } }
    public int GetDefense { get { return defense; } }
    public float GetAttackSpeed { get { return attackSpeed; } }
    public float GetRange { get { return range; } }
}
