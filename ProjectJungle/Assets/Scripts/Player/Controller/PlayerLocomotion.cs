using UnityEngine;

[RequireComponent (typeof(PlayerAnimatorManager))]
public class PlayerLocomotion : MonoBehaviour
{
    [Header("Movement speeds")]
    [SerializeField] private float walkingSpeed = 1.5f;
    [SerializeField] private float runningSpeed = 5f;
    [SerializeField] private float sprintingSpeed = 10f;
    [SerializeField] private float rotationSpeed = 15f;
    [SerializeField] private float maxVelocity = 25f;

    [Header("Swinging variables")]
    [SerializeField] private float swingingSpeed = 12.5f;
    [SerializeField] private float airMomentumVelocity = 20f;

    [Header("Air variables")]
    [SerializeField] private float gravityIntensity = -9.81f;
    [SerializeField] private float jumpForce = 3f;
    [SerializeField] private float fallingVelocity = 33f;

    [Header("References & Other Values")]
    [SerializeField] private float maxSlopeAngle = 55f;
    [SerializeField] private float raycastHeightOffset = 0.5f;
    [SerializeField] private LayerMask groundLayer;

    private PlayerInputManager inputManager;
    private PlayerManager playerManager;
    private PlayerAnimatorManager animatorManager;
    private Vector3 moveDirection;
    private Transform cam;
    private Rigidbody rb;

    private float inAirTimer = 1;
    private Vector3 swingDirection;

    private bool isOnSlope;

    private bool isSprinting;
    private bool isGrounded = true;
    private bool isJumping;
    private bool isSwinging;
    private bool isGrappling;
    private bool shouldHaveAirMomentum;
    private bool hasAddedForwardMomentum;

    public bool shouldMove;

    #region PROPERTIES
    public float MaxVelocity
    {
        get
        {
            return maxVelocity;
        }
    }
    public float InAirTimer
    {
        get
        {
            return inAirTimer;
        }
        set
        {
            inAirTimer = value;
        }
    }
    public Vector3 SwingDirection
    {
        get
        {
            return swingDirection;
        }
        set
        {
            swingDirection = value;
        }
    }
    public bool IsSpriting
    {
        get
        {
            return isSprinting;
        }
        set
        {
            isSprinting = value;
        }
    }
    public bool IsJumping
    {
        get
        {
            return isJumping;
        }
        set
        {
            isJumping = value;
        }
    }
    public bool IsGrappling
    {
        get
        {
            return isGrappling;
        }
        set
        {
            isGrappling = value;
        }
    }
    public bool IsSwinging
    {
        get
        {
            return isSwinging;
        }
        set
        {
            isSwinging = value;
        }
    }
    public bool ShouldHaveAirMomentum
    {
        get
        {
            return shouldHaveAirMomentum;
        }
        set
        {
            shouldHaveAirMomentum = value;
        }
    }
    public bool HasAddedForwardMomentum
    {
        get
        {
            return hasAddedForwardMomentum;
        }
        set
        {
            hasAddedForwardMomentum = value;
        }
    }
    #endregion

    private void Start()
    {
        inputManager = GetComponent<PlayerInputManager>();
        playerManager = GetComponent<PlayerManager>();
        animatorManager = GetComponent<PlayerAnimatorManager>();
        cam = Camera.main.transform;
        rb = GetComponent<Rigidbody>();
    }

    public void HandleAllMovement()
    {
        HandleFallingAndLanding();

        if (playerManager.isInteracting) return;

        HandleRotation();
        HandleMovement();
        HandleSwingingMovement();
    }

    private void HandleMovement()
    {
        if (isJumping || isSwinging || isGrappling) return;

        if (!shouldHaveAirMomentum)
        {
            moveDirection = cam.forward * inputManager.VerticalInput + cam.right * inputManager.HorizontalInput;
            moveDirection.Normalize();

            if (isSprinting)
            {
                moveDirection *= sprintingSpeed;
            }
            else
            {
                if (inputManager.MoveAmount >= 0.5f)
                {
                    moveDirection *= runningSpeed;
                }
                else
                {
                    moveDirection *= walkingSpeed;
                }
            }

            if (!isOnSlope) moveDirection.y = 0f;

            Vector3 movementVelocity = moveDirection;
            rb.velocity = movementVelocity;
        }
        else
        {
            moveDirection = cam.forward * inputManager.VerticalInput + cam.right * inputManager.HorizontalInput;
            moveDirection.Normalize();
            moveDirection.y = 0f;

            moveDirection *= swingingSpeed;

            Vector3 movementVelocity = moveDirection;
            Vector3 newVelocity = new Vector3(movementVelocity.x, rb.velocity.y, movementVelocity.z);
            // rb.velocity = Vector3.Lerp(rb.velocity, newVelocity, Time.fixedDeltaTime);
            rb.velocity = newVelocity;
            rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxVelocity);
        }
    }

    private void HandleSwingingMovement()
    {
        if (!isSwinging) return;

        rb.useGravity = true;

        moveDirection = cam.forward * inputManager.VerticalInput + cam.right * inputManager.HorizontalInput;
        moveDirection.Normalize();

        rb.velocity += moveDirection * swingingSpeed;
        rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxVelocity);
    }

    private void HandleRotation()
    {
        if (isJumping || !shouldMove) return;

        Vector3 targetDirection = Vector3.zero;
        targetDirection = cam.forward * inputManager.VerticalInput + cam.right * inputManager.HorizontalInput;
        targetDirection.Normalize();
        targetDirection.y = 0f;

        if (targetDirection == Vector3.zero) targetDirection = transform.forward;

        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        Quaternion playerRotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);

        transform.rotation = playerRotation;
    }

    private void HandleFallingAndLanding()
    {
        if (isGrappling) return;

        RaycastHit hit;
        Vector3 raycastOrigin = transform.position;
        raycastOrigin.y += raycastHeightOffset;

        if (shouldHaveAirMomentum)
        {
            inAirTimer += Time.deltaTime;
            if (!hasAddedForwardMomentum)
            {
                rb.AddForce((transform.forward + swingDirection) * airMomentumVelocity, ForceMode.Impulse);
                hasAddedForwardMomentum = true;
            }
            rb.AddForce(Vector3.down * fallingVelocity * inAirTimer, ForceMode.Force);
        }
        else
        {
            if (!isGrounded && !isJumping && !isSwinging && !shouldHaveAirMomentum)
            {
                inAirTimer += Time.deltaTime;
                rb.AddForce(Vector3.down * fallingVelocity / 4 * inAirTimer * -gravityIntensity);

                if (!playerManager.isInteracting)
                {
                    animatorManager.PlayTargetAnimation("Falling", true);
                }
            }
        }

        if (Physics.SphereCast(raycastOrigin, 0.2f, Vector3.down, out hit, 0.5f, groundLayer))
        {
            if (!isGrounded)
            {
                animatorManager.PlayTargetAnimation("Landing", true);
            }

            inAirTimer = 1;
            isGrounded = true;
            shouldHaveAirMomentum = false;
            hasAddedForwardMomentum = false;
            rb.useGravity = false;
            swingDirection = Vector3.zero;

            AdjustVelocityBasedOnSlope(hit);
        }
        else
        {
            isGrounded = false;
            isOnSlope = false;
        }

        animatorManager.Animator.SetBool("isGrounded", isGrounded);
    }

    private void AdjustVelocityBasedOnSlope(RaycastHit hit)
    {
        // Adjusting the player's velocity based on the surface normal (This fixes the jitter when going down slopes)

        if (Vector3.Angle(hit.normal, Vector3.up) < maxSlopeAngle)
        {
            Vector3 slopeDirection = Vector3.Cross(Vector3.Cross(Vector3.up, hit.normal), hit.normal).normalized;
            Vector3 movementDirection = rb.velocity.normalized;
            float slopeDot = Vector3.Dot(movementDirection, slopeDirection);

            if (slopeDot > 0f)
            {
                isOnSlope = true;
                rb.velocity = slopeDirection * rb.velocity.magnitude;
            }
            else
            {
                isOnSlope = false;
            }
        }
    }

    public void HandleJumping()
    {
        if (!isGrounded) return;

        animatorManager.Animator.SetBool("isJumping", true);
        animatorManager.PlayTargetAnimation("Jump", false);

        float jumpingVelocity = Mathf.Sqrt(-2 * gravityIntensity * jumpForce);
        rb.AddForce(Vector3.up * jumpingVelocity, ForceMode.Impulse);
    }
}