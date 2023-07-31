using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType 
{
    Food,
    Equipment,
    MobDrop,
    Resource,
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

    private void Awake()
    {
        if (stackable == false)
        {
            maxStackSize = 1;
        }
    }

    /// <summary>
    /// Returns the icon that will be displayed in the inventory
    /// </summary>
    public Sprite GetIcon { get { return icon; } }
    /// <summary>
    /// Returns the game object the player can interact with
    /// </summary>
    public GameObject GetPrefab { get { return prefab; } }
    /// <summary>
    /// Returns the name of the item
    /// </summary>
    public string GetItemName { get { return itemName; } }
    /// <summary>
    /// Returns the type the item is
    /// </summary>
    public ItemType Type { get { return type; } set { type = value; } }
    /// <summary>
    /// Returns the description of the item
    /// </summary>
    public string GetDescription { get { return description; } }
    /// <summary>
    ///  Returns true if the item is stackable
    /// </summary>
    public bool GetStackable { get { return stackable; } }
    /// <summary>
    ///  Returns the maximum size of the stack. If the item is not stackable, returns 1.
    /// </summary>
    public int GetMaxStackSize { get { return stackable == true? maxStackSize : 1; } }
    /// <summary>
    /// Returns the worth of the item
    /// </summary>
    public int GetWorth { get { return worth; } }

    
}
