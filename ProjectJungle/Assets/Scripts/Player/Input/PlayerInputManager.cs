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
    private bool inventoryInput;

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
    #endregion

    public void OnEnable() {
        if (playerControls == null) {
            playerControls = new PlayerControls();

            playerControls.Movement.Movement.performed += i => movementInput = i.ReadValue<Vector2>();
            playerControls.Actions.Sprint.performed += i => sprintInput = true;
            playerControls.Actions.Sprint.canceled += i => sprintInput = false;
            playerControls.Actions.Jump.performed += i => jumpInput = true;
            playerControls.Actions.OpenInventory.performed += i => inventoryInput = true;
            playerControls.Inventory.CloseInventory.performed += i => inventoryInput = false;
        }

        playerControls.Enable();
    }

    public void OnDisable() {
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
        HandleInventoryInput();
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

    private void HandleInventoryInput()
    {
        if (inventoryInput)
        {
            playerControls.Inventory.Enable();

            playerControls.Movement.Disable();
            playerControls.Actions.Disable();

            GetComponent<InventoryManager>().GetInventoryUI().SetActive(true);
        }
        else
        {
            GetComponent<InventoryManager>().GetInventoryUI().SetActive(false);

            playerControls.Movement.Enable();
            playerControls.Actions.Enable();

            playerControls.Inventory.Disable();
        }
    }
}