using System;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private PlayerInputManager inputManager;
    private PlayerLocomotion playerLocomotion;
    private Animator anim;

    public bool isInteracting;

    private void Start()
    {
        inputManager = GetComponent<PlayerInputManager>();
        playerLocomotion = GetComponent<PlayerLocomotion>();
        anim = GetComponent<Animator>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        inputManager.HandleAllInputs();
    }

    private void FixedUpdate()
    {
        playerLocomotion.HandleAllMovement();
    }

    private void LateUpdate()
    {
        isInteracting = anim.GetBool("isInteracting");
        playerLocomotion.IsJumping = anim.GetBool("isJumping");
    }
}