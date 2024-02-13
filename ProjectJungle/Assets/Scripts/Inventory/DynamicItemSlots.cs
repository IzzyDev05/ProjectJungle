using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicItemSlots : MonoBehaviour
{
    [SerializeField] GameObject itemSlotPrefab;
    [SerializeField] Transform slotContainerTransform;

    [SerializeField] int slotAmount;

    [SerializeField] List<GameObject> slotItemList;

    // Start is called before the first frame update
    void Start()
    {
        slotAmount = slotItemList.Count;

        AddSlot(slotAmount);
    }

    void AddSlot(int number)
    {
        for (int i = 0; i != number; i++)
        {
            GameObject newSlot = Instantiate(itemSlotPrefab, slotContainerTransform);
            newSlot.GetComponent<NewSlotManager>().SetSlotItem = slotItemList[i];
        }
    }
}
