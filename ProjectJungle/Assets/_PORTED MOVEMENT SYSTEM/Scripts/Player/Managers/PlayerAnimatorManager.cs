using System;
using UnityEngine;

public class PlayerAnimatorManager : MonoBehaviour
{
    public Animator Animator { get; private set; }
    private int hashedHorizontal;
    private int hashedVertical;
    private int hashedIsLockedInAnim;

    private void Start()
    {
        Animator = GetComponentInChildren<Animator>();

        hashedHorizontal = Animator.StringToHash("horizontal");
        hashedVertical = Animator.StringToHash("vertical");
        hashedIsLockedInAnim = Animator.StringToHash("isLockedInAnim");
    }

    public void PlayTargetAnimation(string targetAnimation, bool isLockedInAnim)
    {
        Animator.SetBool(hashedIsLockedInAnim, isLockedInAnim);
        Animator.CrossFade(targetAnimation, 0.2f);
    }

    public void UpdateAnimatorMovementValues(float horizontalMovement, float verticalMovement, bool isSprinting)
    {
        SnapAnimationValues(horizontalMovement, verticalMovement, out float snappedHorizontal,
            out float snappedVertical);

        if (isSprinting)
        {
            snappedHorizontal = horizontalMovement;
            snappedVertical = 2f;
        }
        
        Animator.SetFloat(hashedHorizontal, snappedHorizontal, 0.1f, Time.deltaTime);
        Animator.SetFloat(hashedVertical, snappedVertical, 0.1f, Time.deltaTime);
    }

    private void SnapAnimationValues(float horizontalMovement, float verticalMovement, out float snappedHorizontal,
        out float snappedVertical)
    {
        if (horizontalMovement is > 0f and < 0.55f) snappedHorizontal = 0.5f;
        else if (horizontalMovement > 0.55f) snappedHorizontal = 1f;
        else if (horizontalMovement is < 0f and > -0.55f) snappedHorizontal = -0.5f;
        else if (horizontalMovement < -0.55f) snappedHorizontal = -1f;
        else snappedHorizontal = 0f;

        if (verticalMovement is > 0f and < 0.55f) snappedVertical = 0.5f;
        else if (verticalMovement > 0.55f) snappedVertical = 1f;
        else if (verticalMovement is < 0f and > -0.55f) snappedVertical = -0.5f;
        else if (verticalMovement < -0.55f) snappedVertical = -1f;
        else snappedVertical = 0f;
    }
}