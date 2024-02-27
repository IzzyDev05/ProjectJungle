using UnityEngine;

public class PlayerInputManager : MonoBehaviour
{
    private PlayerControls playerControls;
    private PlayerLocomotion playerLocomotion;
    private PlayerAnimatorManager animatorManager;

    private Vector2 movementInput;
    private float moveAmount;
    private float verticalInput;
    private float horizontalInput;

    private bool sprintInput;
    private bool jumpInput;
    private bool aimInput;

    #region PROPERTIES
    public float MoveAmount {
        get {
            return moveAmount;
        }
    }
    public float VerticalInput {
        get {
            return verticalInput;
        }
    }
    public float HorizontalInput {
        get {
            return horizontalInput;
        }
    }
    public bool AimInput {
        get {
            return aimInput;
        }
    }
    #endregion

    private void OnEnable() {
        if (playerControls == null) {
            playerControls = new PlayerControls();

            playerControls.Movement.Movement.performed += i => movementInput = i.ReadValue<Vector2>();
            
            playerControls.Actions.Sprint.performed += i => sprintInput = true;
            playerControls.Actions.Sprint.canceled += i => sprintInput = false;
            playerControls.Actions.Jump.performed += i => jumpInput = true;

            playerControls.Actions.Aim.performed += i => aimInput = true;
            playerControls.Actions.Aim.canceled += i => aimInput = false;
        }

        playerControls.Enable();
    }

    private void OnDisable() {
        playerControls.Disable();
    }

    private void Start() {
        animatorManager = GetComponentInChildren<PlayerAnimatorManager>();
        playerLocomotion = GetComponent<PlayerLocomotion>();
    }

    public void HandleAllInputs() {
        HandleMovementInput();
        HandleSprintingInput();
        HandleJumpingInput();
    }

    private void HandleMovementInput() {
        verticalInput = movementInput.y;
        horizontalInput = movementInput.x;

        moveAmount = Mathf.Clamp01(Mathf.Abs(horizontalInput) + Mathf.Abs(verticalInput));
        animatorManager.UpdateAnimatorValues(0, moveAmount, playerLocomotion.IsSpriting);
    }

    private void HandleSprintingInput() {
        if (sprintInput && moveAmount > 0.5f) {
            playerLocomotion.IsSpriting = true;
        }
        else {
            playerLocomotion.IsSpriting = false;
        }
    }

    private void HandleJumpingInput() {
        if (jumpInput) {
            jumpInput = false;
            playerLocomotion.HandleJumping();
        }
    }
}