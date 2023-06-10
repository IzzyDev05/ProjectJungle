using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private PlayerInputManager inputManager;
    private PlayerLocomotion playerLocomotion;
    private PlayerAnimationManager animationManager;

    private void Start() {
        inputManager = GetComponent<PlayerInputManager>();
        playerLocomotion = GetComponent<PlayerLocomotion>();
        animationManager = GetComponentInChildren<PlayerAnimationManager>();
    }

    private void Update() {
        playerLocomotion.HandleAllInputs();
    }

    private void LateUpdate() {
        animationManager.UpdateAnimatorValues(0, inputManager.MoveAmount, playerLocomotion.IsSprinting);
        playerLocomotion.IsJumping = animationManager.GetBool("isJumping");
        animationManager.SetBool("isGrounded", playerLocomotion.IsGrounded);
    }
}