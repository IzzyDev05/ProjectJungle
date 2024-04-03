using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DynamicInventorySlot : MonoBehaviour
{
    [SerializeField] Transform slotContainer;
    [SerializeField] GameObject slotPrefab;

    [SerializeField] List<GameObject> trinketPrefabList;

    private void Awake()
    {
        slotContainer = NewInventoryManager.Instance.GetSlotContainerUI.transform;

        AddSlots(trinketPrefabList.Count);
    }

    /// <summary>
    /// Dynamically adds slots to the inventory UI.
    /// </summary>
    /// <param name="amount">The number of slots to add to the inventory.</param>
    void AddSlots(int amount)
    {
       for (int i = 0; i < amount; i++)
        {
            GameObject newSlot = Instantiate(slotPrefab, slotContainer);

            newSlot.GetComponent<NewSlotManager>().SetSlotItem = trinketPrefabList[i];
        }
    }

    public int TrinketCount { get {  return trinketPrefabList.Count; } }
}
