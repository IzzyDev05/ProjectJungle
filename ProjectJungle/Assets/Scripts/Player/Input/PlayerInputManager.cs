using UnityEngine;
using UnityEngine.InputSystem;
using FMOD.Studio;

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

    // UI
    private bool isUIOpen = false;
    private bool correctTag = false;

    private bool inventoryInput;
    private bool interactInput;
    private bool miscUIInput;

    //Audio
    private EventInstance walkFootsteps;
    private EventInstance sprintFootsteps;

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
        
        walkFootsteps = AudioManager.instance.CreateEventInstance(FModEvents.instance.walkingFootsteps, this.transform);
        sprintFootsteps = AudioManager.instance.CreateEventInstance(FModEvents.instance.sprintingFootsteps, this.transform);
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

        UpdateSound();
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

    // UI Inputs
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

    /// <summary>
    /// Closes inventory UI on a click of a UI button
    /// </summary>
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

                    collider.gameObject.SetActive(false);

                    //Destroy(collider.gameObject);

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


    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "Interact_Pickup":
                {
                    Debug.Log($"Press 'F' to pick up {other.gameObject.name}.");

                    correctTag = true;

                    break;
                }
            case "Interact_Interactable":
                {
                    Debug.Log($"Press 'F' to interact with {other.gameObject.name}.");

                    correctTag = true;

                    break;
                }
            case "Crafting":
                {
                    Debug.Log($"Press 'F' to open Crafting.");

                    correctTag = true;

                    break;
                }
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (interactInput && correctTag)
        {
            HandleInteraction(other);
        }

    }
    private void OnTriggerExit(Collider other)
    {
        if (correctTag)
        {
            correctTag = false;
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
        if (miscUIInput)
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

        EnablePlayerInput();

        playerControls.MiscUI.Disable();

        interactInput = false;
    }

    /// <summary>
    /// Closes UI on a click of a UI Button
    /// </summary>
    public void ClosUIByButton()
    {
        CloseUI();
    }

    // Audio
    private void UpdateSound()
    {
        if ((verticalInput != 0 || horizontalInput != 0) && jumpInput == false)
        {
            
            PLAYBACK_STATE sprintPlaybackState, walkPlaybackState;
            walkFootsteps.getPlaybackState(out walkPlaybackState);
            sprintFootsteps.getPlaybackState(out sprintPlaybackState);

            switch (sprintInput)
            {
                case true:
                    {
                        if (sprintPlaybackState.Equals(PLAYBACK_STATE.STOPPED))
                        {
                            walkFootsteps.stop(STOP_MODE.ALLOWFADEOUT);
                            sprintFootsteps.start();
                        }

                        break;
                    }
                case false:
                    {
                        if (walkPlaybackState.Equals(PLAYBACK_STATE.STOPPED))
                        {
                            sprintFootsteps.stop(STOP_MODE.ALLOWFADEOUT);
                            walkFootsteps.start();
                        }
                        break;
                    }
            }
        }
        else
        {
            walkFootsteps.stop(STOP_MODE.ALLOWFADEOUT);
            sprintFootsteps.stop(STOP_MODE.ALLOWFADEOUT);
        }
    }
}