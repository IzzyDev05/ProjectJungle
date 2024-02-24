using System;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private PlayerControls playerControls;
    private PlayerAnimatorManager animatorManager;
    private PlayerLocomotion playerLocomotion;

    private Vector2 movementInput;
    private bool sprintInput;
    private bool jumpInput;
    
    [HideInInspector] public bool leftMouse;
    [HideInInspector] public bool rightMouse;
    [HideInInspector] public bool groundSlamInput;

    public float moveAmount { get; private set; }
    public float verticalInput { get; private set; }
    public float horizontalInput { get; private set; }

    private void Start()
    {
        animatorManager = GetComponent<PlayerAnimatorManager>();
        playerLocomotion = GetComponent<PlayerLocomotion>();
    }

    private void OnEnable()
    {
        playerControls ??= new PlayerControls(); // Shorthand for: if (playerControls == null { --- }

        // Movement
        playerControls.Movement.Movement.performed += i => movementInput = i.ReadValue<Vector2>();

        // Actions
        playerControls.Actions.Sprint.performed += i => sprintInput = true;
        playerControls.Actions.Sprint.canceled += i => sprintInput = false;

        playerControls.Actions.Jump.performed += i => jumpInput = true;
        playerControls.Actions.Jump.canceled += i => jumpInput = false;

        playerControls.Actions.LeftMouse.performed += i => leftMouse = true;
        playerControls.Actions.LeftMouse.canceled += i => leftMouse = false;
        
        playerControls.Actions.RightMouse.performed += i => rightMouse = true;
        playerControls.Actions.RightMouse.canceled += i => rightMouse = false;
        
        playerControls.Actions.GroundSlam.performed += i => groundSlamInput = true;
        playerControls.Actions.GroundSlam.canceled += i => groundSlamInput = false;

        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    public void HandleAllInputs()
    {
        HandleMovementInput();
        HandleSprintingInput();
        HandleJumpingInput();
    }

    private void HandleMovementInput()
    {
        if (playerLocomotion.isAiming)
        {
            animatorManager.UpdateAnimatorMovementValues(0f, 0f, false);
            return;
        }
        
        verticalInput = movementInput.y;
        horizontalInput = movementInput.x;
        moveAmount = Mathf.Clamp01(Mathf.Abs(horizontalInput) + Mathf.Abs(verticalInput));

        if (PlayerManager.State == States.Grounded)
            animatorManager.UpdateAnimatorMovementValues(0f, moveAmount, playerLocomotion.isSprinting);
        else
            animatorManager.UpdateAnimatorMovementValues(0f, 0f, false);
    }

    private void HandleSprintingInput()
    {
        if (sprintInput && moveAmount > 0.55f) playerLocomotion.isSprinting = true;
        else playerLocomotion.isSprinting = false;
    }

    private void HandleJumpingInput()
    {
        if (!jumpInput) return;

        jumpInput = false;
        playerLocomotion.HandleJumping();
    }


    #region INPUT_LINKERS

    /// <summary>
    /// Disables the player controls
    /// </summary>
    /// <param name="reverse">Enable the player control if true. Default is false.</param>
    public void DisablePlayerControls(bool reverse = false)
    {
        if (!reverse)
        {
            playerControls.Disable();
            movementInput = Vector2.zero;
        }
        else
        {
            playerControls.Enable();
        }
    }
    #endregion
}