using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DynamicInventorySlot : MonoBehaviour
{
    [SerializeField]Transform slotContainer;
    [SerializeField] GameObject slotPrefab;

    [SerializeField] List<GameObject> trinketPrefabList;

    private void Awake()
    {
        AddSlots(trinketPrefabList.Count);
    }

    void AddSlots(int amount)
    {
       for (int i = 0; i < amount; i++)
        {
            GameObject newSlot = Instantiate(slotPrefab, slotContainer);

            newSlot.GetComponent<NewSlotManager>().SetSlotItem = trinketPrefabList[i];
        }
    }
}
