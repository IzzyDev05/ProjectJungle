using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(InteractionUIManager))]
public class PlayerInteractUIManager : MonoBehaviour
{
    InteractionUIManager IUIManager;

    // Start is called before the first frame update
    void Start()
    {
        IUIManager = GetComponent<InteractionUIManager>();
    }

    // Update is called once per frame
    void Update()
    {
        IUIManager.HandleUIInputs();
    }
}
