using UnityEngine;

public class PlayerAnimatorManager : MonoBehaviour
{
    private Animator animator;
    private int horizontal;
    private int vertical;

    public Animator Animator {
        get {
            return animator;
        }
    }

    private void Start() {
        animator = GetComponent<Animator>();

        // Caching the animator values because string references stinky
        horizontal = Animator.StringToHash("horizontal");
        vertical = Animator.StringToHash("vertical");
    }

    // Can't move or do anything else when isInteracting
    public void PlayTargetAnimation(string targetAnimation, bool isInteracting) {
        animator.SetBool("isInteracting", isInteracting);
        animator.CrossFade(targetAnimation, 0.1f);
    }

    public void UpdateAnimatorValues(float horizontalMovement, float verticalMovement, bool isSprinting) {
        // Animation snapping
        float snappedHorizontal;
        float snappedVertical;

        #region Snapped Horizontal
        if (horizontalMovement > 0 && horizontalMovement < 0.55f) {
            snappedHorizontal = 0.5f;
        }
        else if (horizontalMovement > 0.55f) {
            snappedHorizontal = 1f;
        }
        else if (horizontalMovement < 0 && horizontalMovement > -0.55f) {
            snappedHorizontal = -0.5f;
        }
        else if (horizontalMovement < -0.55f) {
            snappedHorizontal = -1f;
        }
        else {
            snappedHorizontal = 0f;
        }
        #endregion
        #region Snapped Vertical
        if (verticalMovement > 0 && verticalMovement < 0.55f) {
            snappedVertical = 0.5f;
        }
        else if (verticalMovement > 0.55f) {
            snappedVertical = 1f;
        }
        else if (verticalMovement < 0 && verticalMovement > -0.55f) {
            snappedVertical = -0.5f;
        }
        else if (verticalMovement < -0.55f) {
            snappedVertical = -1f;
        }
        else {
            snappedVertical = 0f;
        }
        #endregion

        if (isSprinting) {
            snappedHorizontal = horizontalMovement;
            snappedVertical = 2f;
        }

        // Setting animator values
        animator.SetFloat(horizontal, snappedHorizontal, 0.1f, Time.deltaTime);
        animator.SetFloat(vertical, snappedVertical, 0.1f, Time.deltaTime);
    }
}