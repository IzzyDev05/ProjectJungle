using UnityEngine;

public class PlayerLocomotion : MonoBehaviour
{
    [Header("Movement speeds")]
    [SerializeField] private float walkingSpeed = 1.5f;
    [SerializeField] private float runningSpeed = 5f;
    [SerializeField] private float sprintingSpeed = 7f;
    [SerializeField] private float maxVelocity = 12.5f;
    [SerializeField] private float rotationSpeed = 15f;

    [Header("Air variables")]
    [SerializeField] private float gravityIntensity = -9.81f;
    [SerializeField] private float jumpHeight = 3f;
    [SerializeField] private float leapingVelocity = 3f;
    [SerializeField] private float fallingVelocity = 33f;
    [SerializeField] private float fallingSpeedMultiplier = 3f;
    [SerializeField] private float raycastHeightOffset = 0.5f;

    [Header("References")]
    [SerializeField] private LayerMask groundLayer;

    PlayerInputManager inputManager;
    private Vector3 moveDirection;
    private Transform cam;
    private Rigidbody rb;
    private Swinging swinging;
    private float inAirTimer;

    private bool isSprinting;
    private bool isGrounded = true;
    private bool isJumping;
    private bool isSwinging;

    #region PROPERTIES
    public float MaxVelocity {
        get {
            return maxVelocity;
        }
    }
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
    public bool IsSwinging {
        get {
            return isSwinging;
        }
        set {
            isSwinging = value;
        }
    }
    #endregion

    private void Start() {
        inputManager = GetComponent<PlayerInputManager>();
        cam = Camera.main.transform;
        rb = GetComponent<Rigidbody>();
    }

    public void HandleAllMovement() {
        HandleFallingAndLanding();

        HandleRotation();
        HandleMovement();
        HandleSwingingMovement();
    }

    private void HandleMovement() {
        if (isJumping) return;

        if (!isSwinging) {
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
            movementVelocity = Vector3.Lerp(movementVelocity, rb.velocity, Time.fixedDeltaTime);
            rb.velocity = movementVelocity;
        }
        else {
            rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, rb.velocity.z);
        }

        rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxVelocity);
    }

    private void HandleSwingingMovement() {
        if (!isSwinging) return;

        moveDirection = cam.forward * inputManager.VerticalInput + cam.right * inputManager.HorizontalInput;
        moveDirection.Normalize();

        rb.velocity += moveDirection;
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

        if (isSwinging) {
            rb.AddForce(transform.forward * leapingVelocity);
            rb.AddForce(Vector3.down * fallingVelocity * fallingSpeedMultiplier);
        }
        else if (!isGrounded && !isJumping) {
            inAirTimer += Time.deltaTime;

            rb.AddForce(transform.forward * leapingVelocity);
            rb.AddForce(Vector3.down * fallingVelocity * fallingSpeedMultiplier * inAirTimer);
        }

        if (Physics.SphereCast(raycastOrigin, 0.2f, Vector3.down, out hit, 0.5f, groundLayer)) {
            if (!isGrounded && !isSwinging) {
                // play landing animation
            }

            targetPosition.y = hit.point.y; // Floating capsule
            inAirTimer = 0;
            isGrounded = true;
        }
        else {
            isGrounded = false;
        }

        // Floating capsule
        if (isGrounded && !isJumping && !isSwinging) {
            if (inputManager.MoveAmount > 0) {
                // Smoothly pushes us up if we're moving
                transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime / 0.1f);
            }
            else {
                transform.position = targetPosition;
            }
        }
    }

    public void HandleJumping() {
        if (isGrounded && !isSwinging) {
            float jumpingVelocity = Mathf.Sqrt(-2 * gravityIntensity * jumpHeight);
            Vector3 playerVelocity = moveDirection;
            playerVelocity.y = jumpingVelocity;
            rb.velocity = playerVelocity;
        }
    }
}