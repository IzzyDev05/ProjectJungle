using System;
using UnityEngine;

public class TrebuchetController : MonoBehaviour
{
    [SerializeField] private Rigidbody weight;
    [SerializeField] private GameObject ammo;

    private InputManager inputManager;

    private void Start()
    {
        inputManager = FindObjectOfType<InputManager>();
    }

    private void Update()
    {
        if (inputManager.leftMouse)
        {
            weight.isKinematic = false;
        }
        
        if (inputManager.rightMouse)
        {
            HingeJoint hinge = ammo.GetComponent<HingeJoint>();
            Destroy(hinge);
        }
    }
}