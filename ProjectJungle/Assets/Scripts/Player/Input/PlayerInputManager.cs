using UnityEngine;
using FMODUnity;
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

    private bool inventoryInput;
    private bool menuInput;
    private bool closeAllUI;
    private bool interactInput;

    //Audio
    private EventInstance walkFootsteps;
    private EventInstance sprintFootsteps;

    private bool playOneShot = false;

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
        if (playerControls == null)
        {
            playerControls = new PlayerControls();

            playerControls.Movement.Movement.performed += Move => movementInput = Move.ReadValue<Vector2>();

            playerControls.Actions.Sprint.performed += Sprint => sprintInput = true;
            playerControls.Actions.Sprint.canceled += Sprint => sprintInput = false;
            playerControls.Actions.Jump.performed += Jump => jumpInput = true;
            playerControls.Actions.Interact.performed += Interact => interactInput = true;
            playerControls.Actions.OpenInventory.performed += OpenInventory => { inventoryInput = true; playOneShot = true; };
            playerControls.Actions.Menu.performed += Menu => menuInput = true;

            playerControls.UI.CloseInventory.performed += CloseInventory => { inventoryInput = false; playOneShot = true; };
            playerControls.UI.ExitUI.performed += CloseUI => closeAllUI = true;

        }

        playerControls.Enable();
    }

    private void OnDisable() {
        playerControls.Disable();
    }

    private void Start() {
        animatorManager = GetComponentInChildren<PlayerAnimatorManager>();
        playerLocomotion = GetComponent<PlayerLocomotion>();
        
        // Ensure sounds 
        walkFootsteps = AudioManager.instance.CreateEventInstance(FModEvents.instance.walkingFootsteps, this.transform);
        sprintFootsteps = AudioManager.instance.CreateEventInstance(FModEvents.instance.sprintingFootsteps, this.transform);

        // Disable all UI inputs
        playerControls.UI.Disable();

        closeAllUI = false;
    }

    public void HandleAllInputs() {
        HandleMovementInput();
        HandleSprintingInput();
        HandleJumpingInput();
        HandleUIInputs();
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


    // Enables player's movement and action input
    private void EnablePlayerInput()
    {
        playerControls.Movement.Enable();
        playerControls.Actions.Enable();
    }

    // Disables player's movement and action input
    private void DisablePlayerInput()
    {
        playerControls.Movement.Disable();
        playerControls.Actions.Disable();
    }


    /// UI Inputs ///
    private void SwitchInputs(bool toUIInputs)
    {
        if (toUIInputs)
        {
            playerControls.UI.Enable();

            DisablePlayerInput();
        }
        else
        {
            EnablePlayerInput();

            playerControls.UI.Disable();
        }
    }

    /// <summary>
    /// Closes inventory UI via other means like a button press
    /// </summary>
    public void ManuallyCloseInventory()
    {
        if (inventoryInput)
        {
            inventoryInput = false;
            PlayOneShotSound(FModEvents.instance.backpack, transform.position);
        }      
    }

    private void HandleUIInputs()
    {
        CloseUI();

        if (menuInput)
        {
            OpenSettings();

            return;
        }

        if (playOneShot)
        {
            PlayOneShotSound(FModEvents.instance.backpack, transform.position);
        }

        if (inventoryInput)
        {
            InventoryManager.Instance.OpenInventory();

            SwitchInputs(true);

            GameManager.Instance.PauseGame();
        }
    }

    private void OpenSettings()
    {
        SwitchInputs(true);

        GameManager.Instance.PauseGame();

        GameManager.Instance.SettingsUI.SetActive(true);
    }

    private void CloseSettings()
    {
        GameManager.Instance.SettingsUI.SetActive(false);

        menuInput = false;
    }

    private void CloseUI()
    {
        if (!playerControls.UI.enabled)
        {
            return;
        }

        if (closeAllUI)
        {
            ManuallyCloseInventory();
            CloseSettings();
            closeAllUI = false;
        }

        if (!inventoryInput)
        {
            InventoryManager.Instance.CloseInventory();

            SwitchInputs(false);

            inventoryInput = false;
        }

        movementInput = Vector2.zero;

        SwitchInputs(false);
        GameManager.Instance.UnpauseGame();
    }

    // Audio
    private void PlayOneShotSound(EventReference oneshot, Vector3 position)
    {
        AudioManager.instance.PlayOneShot(oneshot, position);
        playOneShot = false;
    }

    private void UpdateSound()
    {
        if (inventoryInput || menuInput)
        {
            walkFootsteps.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            sprintFootsteps.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);

            return;
        }

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
                            walkFootsteps.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                            sprintFootsteps.start();
                        }

                        break;
                    }
                case false:
                    {
                        if (walkPlaybackState.Equals(PLAYBACK_STATE.STOPPED))
                        {
                            sprintFootsteps.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                            walkFootsteps.start();
                        }
                        break;
                    }
            }
        }
        else
        {
            walkFootsteps.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            sprintFootsteps.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        }
    }

    // Interactions
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Interact_Pickup"))
        {
            Debug.Log($"Press 'F' to pickup {other.gameObject.name}");
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (!other.gameObject.CompareTag("Interact_Pickup"))
        {
            return;
        }

        if (interactInput)
        {
            ItemManager itemPickedUp = other.gameObject.GetComponent<ItemManager>();
            InventoryManager.Instance.AddToInventory(itemPickedUp.PickupItem(), itemPickedUp.GetAmountPickedUp);
            interactInput = false;
        }
    }
}