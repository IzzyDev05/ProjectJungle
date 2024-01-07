using UnityEngine;
using UnityEngine.InputSystem;
using FMOD.Studio;

public class PlayerInputManager : MonoBehaviour
{
    private PlayerControls playerControls;
    private PlayerLocomotion playerLocomotion;
    //private PlayerAnimatorManager animatorManager;
    private UIAndInteractionManager UIAndInteraction;

    private Vector2 movementInput;
    private float moveAmount;
    private float verticalInput;
    private float horizontalInput;

    private bool sprintInput;
    private bool jumpInput;
    private bool aimInput;
    private bool swingInput;

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
    public bool AimInput {
        get {
            return aimInput;
        }
    }
    public bool SwingInput {
        get {
            return swingInput;
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
            
            playerControls.Actions.Aim.performed += i => aimInput = true;
            playerControls.Actions.Aim.canceled += i => aimInput = false;

            playerControls.Actions.Swing.started += i => swingInput = true;
            playerControls.Actions.Swing.canceled += i => swingInput = false;

        }

        playerControls.Enable();
    }

    private void OnDisable() {
        playerControls.Disable();
    }

    private void Start() {
        //animatorManager = GetComponentInChildren<PlayerAnimatorManager>();
        playerLocomotion = GetComponent<PlayerLocomotion>();
        UIAndInteraction = GetComponent<UIAndInteractionManager>();
        
        // Ensure sounds 
        walkFootsteps = AudioManager.instance.CreateEventInstance(FModEvents.instance.walkingFootsteps, this.transform);
        sprintFootsteps = AudioManager.instance.CreateEventInstance(FModEvents.instance.sprintingFootsteps, this.transform);
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
        //animatorManager.UpdateAnimatorValues(0, moveAmount, playerLocomotion.IsSpriting);

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

    // Audio
    private void UpdateSound()
    {
        if (UIAndInteraction.IsUIOpened())
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


    // PlayerInputManager to UIAndInteractionManager Helpers
    /// <summary>
    /// Sets the movement input vector to (0,0).
    /// </summary>
    public void ResetMovementInput()
    {
        movementInput = Vector2.zero;
    }

    // Enables player's movement and action input
    public void EnablePlayerInput()
    {
        playerControls.Movement.Enable();
        playerControls.Actions.Enable();
    }

    // Disables player's movement and action input
    public void DisablePlayerInput()
    {
        playerControls.Movement.Disable();
        playerControls.Actions.Disable();
    }
}