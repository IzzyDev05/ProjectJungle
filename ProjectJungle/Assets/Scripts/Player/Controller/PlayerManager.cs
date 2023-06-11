using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private PlayerInputManager inputManager;
    private PlayerLocomotion playerLocomotion;
    private PlayerAnimationManager animationManager;

    [SerializeField] private bool isInteracting;

    public bool IsInteracting {
        get {
            return isInteracting;
        }
        set {
            isInteracting = value;
        }
    }

    private void Start() {
        inputManager = GetComponent<PlayerInputManager>();
        playerLocomotion = GetComponent<PlayerLocomotion>();
        animationManager = GetComponentInChildren<PlayerAnimationManager>();
    }

    private void Update() {
        inputManager.HandleAllInputs();
        playerLocomotion.HandleAllMovement();
    }

    private void LateUpdate() {
        isInteracting = animationManager.GetBool("isInteracting");
        
        if (!isInteracting) animationManager.UpdateAnimatorValues(0, inputManager.MoveAmount, playerLocomotion.IsSprinting);

        playerLocomotion.IsJumping = animationManager.GetBool("isJumping");
        animationManager.SetBool("isGrounded", playerLocomotion.IsGrounded);
    }
}