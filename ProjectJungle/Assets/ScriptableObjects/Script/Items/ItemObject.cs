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
    [SerializeField] Sprite icon;
    [SerializeField] GameObject prefab;
    [SerializeField] string itemName;
    [SerializeField] ItemType type;
    [TextArea(15, 20)] [SerializeField] string description;
    [SerializeField] bool stackable;
    [SerializeField] int maxStackSize;
    [SerializeField] int worth;

    public Sprite GetIcon { get { return icon; } }
    public GameObject GetPrefab { get { return prefab; } }
    public string GetItemName { get { return itemName; } }

    public ItemType Type { get { return type; } set { type = value; } }
    public string GetDescription { get { return description; } }
    public bool GetStackable { get { return stackable; } }
    public int GetMaxStackSize { get { return maxStackSize; } }

    public int GetWorth { get { return worth; } }

}
