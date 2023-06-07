using UnityEngine;

[RequireComponent(typeof(PlayerInputManager))]
public class PlayerLocomotion : MonoBehaviour
{
    [Header("Movement speeds")]
    [SerializeField] private float walkingSpeed = 1.5f;
    [SerializeField] private float runningSpeed = 5f;
    [SerializeField] private float sprintingSpeed = 7f;
    [SerializeField] private float rotationSpeed = 15f;

    [Header("Air variables")]
    [SerializeField] private float gravityIntensity = -9.81f;
    [SerializeField] private float jumpHeight = 3f;
    [SerializeField] private float leapingVelocity = 3f;
    [SerializeField] private float fallingVelocity = 33f;
    [SerializeField] private float raycastHeightOffset = 0.5f;

    [Header("References")]
    [SerializeField] private LayerMask groundLayer;

    PlayerInputManager inputManager;
    PlayerManager playerManager;
    PlayerAnimatorManager playerAnimatorManager;
    private Vector3 moveDirection;
    private Transform cam;
    private Rigidbody rb;

    private bool isSprinting;
    private float inAirTimer;
    private bool isGrounded = true;
    private bool isJumping;

    #region PROPERTIES
    public bool IsSpriting {
        get {
            return isSprinting;
        }
        set {
            isSprinting = value;
        }
    }
    public bool IsJumping {
        get {
            return isJumping;
        }
        set {
            isJumping = value;
        }
    }
    public bool IsGrounded {
        get {
            return isGrounded;
        }
    }
    #endregion

    private void Start() {
        inputManager = GetComponent<PlayerInputManager>();
        playerManager = GetComponent<PlayerManager>();
        playerAnimatorManager = GetComponentInChildren<PlayerAnimatorManager>();
        cam = Camera.main.transform;
        rb = GetComponent<Rigidbody>();
    }

    public void HandleAllMovement() {
        HandleFallingAndLanding();

        if (playerManager.IsInteracting) return;
        
        HandleMovement();
        HandleRotation();
    }

    private void HandleMovement() {
        if (isJumping) return;

        moveDirection = cam.forward * inputManager.VerticalInput + cam.right * inputManager.HorizontalInput;
        moveDirection.Normalize();
        moveDirection.y = 0f;

        // Checking if we are sprinting, running or walking
        if (isSprinting) {
            moveDirection *= sprintingSpeed;
        }
        else {
            if (inputManager.MoveAmount >= 0.5f) {
                moveDirection *= runningSpeed;
            }
            else {
                moveDirection *= walkingSpeed;
            }
        }
        Vector3 movementVelocity = moveDirection;
        rb.velocity = movementVelocity;
    }

    private void HandleRotation() {
        if (isJumping) return;

        Vector3 targetDirection = Vector3.zero;
        targetDirection = cam.forward * inputManager.VerticalInput + cam.right * inputManager.HorizontalInput;
        moveDirection.Normalize();
        targetDirection.y = 0f;

        if (targetDirection == Vector3.zero) targetDirection = transform.forward;

        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        Quaternion playerRotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);

        transform.rotation = playerRotation;
    }

    private void HandleFallingAndLanding() {
        RaycastHit hit;
        Vector3 raycastOrigin = transform.position;
        Vector3 targetPosition = transform.position; // Floating capsule

        raycastOrigin.y += raycastHeightOffset;

        if (!isGrounded && !isJumping) {
            if (!playerManager.IsInteracting) {
                // If we're not grounded OR jumping, and neither are we interacting, that means we should fall
                playerAnimatorManager.PlayTargetAnimation("Falling", true);
            }

            inAirTimer += Time.deltaTime;
            rb.AddForce(transform.forward * leapingVelocity);
            rb.AddForce(-Vector3.up * fallingVelocity * inAirTimer);
        }

        if (Physics.SphereCast(raycastOrigin, 0.2f, -Vector3.up, out hit, 0.5f, groundLayer)) {
            if (!isGrounded && playerManager.IsInteracting) {
                // If we just grounded and we were interacting, that means we're landing
                playerAnimatorManager.PlayTargetAnimation("Landing", true);
            }

            Vector3 rayCastHitPoint = hit.point; // Floating capsule
            targetPosition.y = rayCastHitPoint.y; // Floating capsule
            inAirTimer = 0;
            playerManager.IsInteracting = false;
            isGrounded = true;
        }
        else {
            isGrounded = false;
        }

        // Floating capsule
        if (isGrounded && !isJumping) {
            if (playerManager.IsInteracting || inputManager.MoveAmount > 0) {
                // Smoothly pushing us up (So that our feet our on the ground) if we're moving or interacting
                transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime / 0.1f);
            }
            else {
                transform.position = targetPosition;
            }
        }
    }

    public void HandleJumping() {
        if (isGrounded) {
            playerAnimatorManager.Animator.SetBool("isJumping", true);
            playerAnimatorManager.PlayTargetAnimation("Jump", false);

            float jumpingVelocity = Mathf.Sqrt(-2 * gravityIntensity * jumpHeight);
            Vector3 playerVelocity = moveDirection;
            playerVelocity.y = jumpingVelocity;
            rb.velocity = playerVelocity;
        }
    }
}