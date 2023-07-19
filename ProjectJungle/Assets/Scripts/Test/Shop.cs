using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    [SerializeField] ItemObject stealItem;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /*private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            foreach (GameObject slot in InventoryManager.Instance.GetInventorySlots)
            {
                if (slot.GetComponent<SlotManager>().MatchSlotItem(stealItem))
                {
                    slot.GetComponent<SlotManager>().RemoveMultipleItems(10);

                    Debug.Log($"Robin hood stole 10 arrows");

                    return;
                }
            }
        }
    }*/
}
