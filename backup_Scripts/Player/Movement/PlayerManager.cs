using UnityEngine;

[RequireComponent(typeof(PlayerInputManager))]
[RequireComponent(typeof(PlayerLocomotion))]
public class PlayerManager : MonoBehaviour
{
    private PlayerInputManager inputManager;
    private PlayerLocomotion playerLocomotion;
    private Animator animator;
    private bool isInteracting;

    #region PROPERTIES
    public bool IsInteracting {
        get {
            return isInteracting;
        }
        set {
            isInteracting = value;
        }
    }
    #endregion

    private void Start() {
        inputManager = GetComponent<PlayerInputManager>();
        playerLocomotion = GetComponent<PlayerLocomotion>();
        animator = GetComponentInChildren<Animator>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update() {
        inputManager.HandleAllInputs();
    }

    private void FixedUpdate() {
        playerLocomotion.HandleAllMovement();
    }

    private void LateUpdate() {
        isInteracting = animator.GetBool("isInteracting");
        playerLocomotion.IsJumping = animator.GetBool("isJumping");
        animator.SetBool("isGrounded", playerLocomotion.IsGrounded);
    }
}