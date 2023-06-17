using UnityEngine;
using UnityEngine.InputSystem;

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
    private bool interactInput;

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

    private void OnEnable() {
        if (playerControls == null) {
            playerControls = new PlayerControls();

            playerControls.Movement.Movement.performed += Move => movementInput = Move.ReadValue<Vector2>();
            playerControls.Actions.Sprint.performed += Sprint => sprintInput = true;
            playerControls.Actions.Sprint.canceled += Sprint => sprintInput = false;
            playerControls.Actions.Jump.performed += Jump => jumpInput = true;
            playerControls.Actions.OpenInventory.performed += OpenInventory => inventoryInput = true;
            playerControls.Inventory.CloseInventory.performed += CloseInventory => inventoryInput = false;
            playerControls.Actions.Interact.performed += Interact => interactInput = true;
            playerControls.Actions.Interact.canceled += Interact => interactInput = false;

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

            InventoryManager.Instance.OpenInventory();
            
        }
        else
        {
            InventoryManager.Instance.CloseInventory();

            playerControls.Movement.Enable();
            playerControls.Actions.Enable();

            playerControls.Inventory.Disable();
        }
    }

    public void CloseInventoryByButton()
    {
        if (inventoryInput)
        {
            inventoryInput = false;
        }      
    }

    private void HandleInteraction(Collider collider)
    {
        switch (collider.tag)
        {
            case "Interact_Pickup":
                {
                    InventoryManager.Instance.AddToInventory(collider.GetComponent<ItemPickup>().PickupItem());

                    collider.gameObject.SetActive(false);

                    // Destroy(collider.GetComponent<GameObject>());

                    interactInput = false;

                    break;
                }
            case "Interact_Interactable":
                {
                    // To Be Added
                    break;
                }
        }
    }

    void OnTriggerStay(Collider other)
    {
        Debug.Log($"Press 'F' to pick up {other.gameObject.name}.");

        if ((other.CompareTag("Interact_Pickup")|| other.CompareTag("Interact_Interactable")) && interactInput)
        {
            HandleInteraction(other);
        }
    }
}