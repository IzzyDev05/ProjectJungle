using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SortInventory : MonoBehaviour
{
    [SerializeField] TMP_Dropdown sortBy;
    [SerializeField] GameObject inventory;

    // Start is called before the first frame update
    void Start()
    {
        sortBy = GetComponent<TMP_Dropdown>();
    }

    // Update is called once per frame
    void Update()
    {
        string option = sortBy.options[sortBy.value].ToString();

        switch (option) 
        {
            case "Name":
                {

                    break;
                }
            case "Rarity":
                {

                    break;
                }
            case "Amount":
                {

                    break;
                }
        }

    }
}
