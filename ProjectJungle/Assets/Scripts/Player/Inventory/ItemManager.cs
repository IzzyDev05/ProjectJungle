using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ItemManager : MonoBehaviour
{
    TMP_Text itemCountText;

    int maxCapacity = 1;
    [SerializeField] ItemObject item;
    [SerializeField] int currentCapacity;


    // Start is called before the first frame update
    void Start()
    {
        itemCountText = GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateItemCount();
    }

    void UpdateItemCount()
    {
        if (currentCapacity <= 1)
        {
            itemCountText.text = "";
        }
        else
        {
            itemCountText.text = currentCapacity.ToString();
        }
    }

    void RemoveItem()
    {
        if (currentCapacity > 0)
        {
            currentCapacity--;
        }
        else
        {
            currentCapacity = 0;
        }
    }

    int AddItem(int amount = 1)
    {
        int remainder = 0;
        if (currentCapacity < maxCapacity)
        {
            remainder = amount - (maxCapacity - currentCapacity);
            currentCapacity += amount - remainder;
        }
        else
        {
            remainder = amount;
        }

        return remainder;
    }
}
