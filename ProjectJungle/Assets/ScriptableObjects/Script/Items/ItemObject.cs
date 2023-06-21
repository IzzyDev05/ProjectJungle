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
    Weapon,
    Ammo
}
public abstract class ItemObject : ScriptableObject
{
    public Sprite icon;
    public GameObject prefab;
    public string itemName;
    public ItemType type;
    [TextArea(15, 20)] public string description;
    public bool stackable;
    public int maxStackSize;

    public string GetDescription { get { return description; } }

    public ItemType GetItemType { get { return type; } }
}
