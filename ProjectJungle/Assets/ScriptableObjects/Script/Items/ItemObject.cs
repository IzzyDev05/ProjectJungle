using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType 
{
    Food,
    Equipment,
    MobDrop,
    Default
}
public enum EquipmentType
{
    Armour,
    Shield,
    Weapon
}
public abstract class ItemObject : ScriptableObject
{
    public GameObject prefab;
    public ItemType type;
    [TextArea(15, 20)] public string description;

    public bool stackable;
    public int maxStackSize;
}
