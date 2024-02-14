using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicItemSlots : MonoBehaviour
{
    [SerializeField] GameObject itemSlotPrefab;
    [SerializeField] Transform slotContainerTransform;

    [SerializeField] int slotAmount;

    [SerializeField] List<GameObject> slotItemList;

    void Awake()
    {
        slotAmount = slotItemList.Count;

        AddSlot(slotAmount);
    }

    /// <summary>
    /// Dynamically adds slots to the inventory
    /// </summary>
    /// <param name="number">The number of slots to add. Minimum of 1 slot.</param>
    void AddSlot(int number)
    {
        if (number < 1)
        {
            number = 1;
        }

        for (int i = 0; i != number; i++)
        {
            GameObject newSlot = Instantiate(itemSlotPrefab, slotContainerTransform);
            newSlot.GetComponent<NewSlotManager>().SetSlotItem = slotItemList[i];
        }
    }
}
