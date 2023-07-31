using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Food Object", menuName = "Inventory System/Items/Food")]
public class FoodObject : ItemObject
{
    public int restoreHealthValue;

    public void Awake()
    {
        Type = ItemType.Food;
    }

    /// <summary>
    /// Returns the amount of health the food item restores
    /// </summary>
    public int HealthRestored { get { return restoreHealthValue; } }
}
