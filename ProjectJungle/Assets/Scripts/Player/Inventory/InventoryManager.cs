using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.UI;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] GameObject inventoryUI;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public GameObject GetInventoryUI()
    {
        return inventoryUI;
    }
}
