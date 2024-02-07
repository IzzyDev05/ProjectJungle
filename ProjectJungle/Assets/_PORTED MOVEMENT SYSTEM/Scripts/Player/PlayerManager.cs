using UnityEngine;

public enum States
{
    Grappling,
    Swinging,
    Aerial,
    Grounded
}

[RequireComponent(typeof(InputManager))]
public class PlayerManager : MonoBehaviour
{
    public static States State = States.Grounded;
    public static States PreviousState = State;

    public States currentState;
    
    public bool isLockedInAnim { get; private set; }
    
    private InputManager inputManager;
    private PlayerLocomotion playerLocomotion;
    private PlayerAnimatorManager animatorManager;
    
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
        inputManager = GetComponent<InputManager>();
        playerLocomotion = GetComponent<PlayerLocomotion>();
        animatorManager = GetComponent<PlayerAnimatorManager>();
    }

    private void Update()
    {
        currentState = State;
        inputManager.HandleAllInputs();
    }

    private void FixedUpdate()
    {
        playerLocomotion.HandleAllMovement();
    }

    private void LateUpdate()
    {
        HandleAnimatorValues();
    }

    private void HandleAnimatorValues()
    {
        var animator = animatorManager.Animator;
        animator.SetBool("isGrounded", playerLocomotion.isGrounded);
        animator.SetBool("isSwinging", playerLocomotion.isSwinging);
        animator.SetBool("isGrappling", playerLocomotion.isGrappling);
        
        isLockedInAnim = animator.GetBool("isLockedInAnim");
        playerLocomotion.isJumping = animator.GetBool("isJumping");
    }

    public static void UpdateState(States newState)
    {
        if (State != newState) PreviousState = State;
        State = newState;
    }
}