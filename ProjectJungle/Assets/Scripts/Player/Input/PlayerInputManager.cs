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

    private bool isUIOpen = false;

    private bool inventoryInput;
    private bool interactInput;
    private bool miscUIInput;

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
            playerControls.MiscUI.CloseUI.performed += CloseUI => miscUIInput = true;

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
        HandleUIInput();
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

    private void EnablePlayerInput()
    {
        playerControls.Movement.Enable();
        playerControls.Actions.Enable();
    }

    private void DisablePlayerInput()
    {
        playerControls.Movement.Disable();
        playerControls.Actions.Disable();
    }

    private void HandleInventoryInput()
    {
        if (inventoryInput && !isUIOpen)
        {
            playerControls.Inventory.Enable();

            DisablePlayerInput();

            InventoryManager.Instance.OpenInventory();
            
        }
        else if (!inventoryInput && !isUIOpen)
        {
            InventoryManager.Instance.CloseInventory();

            EnablePlayerInput();

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
                    ItemManager itemManager = collider.GetComponent<ItemManager>();

                    InventoryManager.Instance.AddToInventory(itemManager.PickupItem(), false, itemManager.GetAmountPickedUp);

                    //collider.gameObject.SetActive(false);

                    Destroy(collider.gameObject);

                    interactInput = false;

                    break;
                }
            case "Interact_Interactable":
                {
                    // To Be Added
                    break;
                }
            case "Crafting":
                {
                    HandleUI();

                    break;
                }
            case "Merchant":
                {
                    // To Be Added
                    break;
                }
            default:
                {
                    return;
                }
        }
    }

    void OnTriggerStay(Collider other)
    {
        switch (other.tag)
        {
            case "Interact_Pickup":
                {
                    Debug.Log($"Press 'F' to pick up {other.gameObject.name}.");
                    break;
                }
            case "Interact_Interactable":
                {
                    Debug.Log($"Press 'F' to interact with {other.gameObject.name}.");
                    break;
                }
            case "Crafting":
                {
                    Debug.Log($"Press 'F' to open Crafting.");
                    break;
                }
        }

        if (interactInput)
        {
            HandleInteraction(other);
        }

    }

    private void HandleUI()
    {
        if (!CraftingSystemManager.Instance.MenuActive && !isUIOpen)
        {
            OpenUI();
        }
        else if (CraftingSystemManager.Instance.MenuActive && isUIOpen)
        {
            CloseUI();
        }
    }

    private void HandleUIInput()
    {
        if (miscUIInput && CraftingSystemManager.Instance.MenuActive)
        {
            CloseUI();
        }

        miscUIInput = false;
    }

    private void OpenUI()
    {
        isUIOpen = true;

        playerControls.MiscUI.Enable();

        DisablePlayerInput();

        CraftingSystemManager.Instance.ActivateMenu();

        interactInput = false;
    }

    private void CloseUI()
    {
        isUIOpen = false;

        CraftingSystemManager.Instance.DeactivateMenu();

        EnablePlayerInput();

        playerControls.MiscUI.Disable();

        interactInput = false;
    }

    public void ClosUIByButton()
    {
        CloseUI();
    }
}