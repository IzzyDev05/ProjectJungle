using System.Collections;
using Cinemachine;
using UnityEngine;

[RequireComponent(typeof(PlayerManager))]
public class PlayerLocomotion : MonoBehaviour
{
    [Header("Movement Speeds")] 
    [SerializeField] private float walkingSpeed = 3f;
    [SerializeField] private float runningSpeed = 6f;
    [SerializeField] private float sprintingSpeed = 12f;
    [SerializeField] private float rotationSpeed = 15f;
    [SerializeField] private float regularDrag = 1.5f;

    [Header("Aerial Speeds")] 
    [SerializeField] private float aerialMovementSpeed = 10f;
    [SerializeField] private float aerialDrag = 3.5f;
    [SerializeField] private float leapingVelocity = 1.5f;
    [SerializeField] private float fallingVelocity = 33f;
    [SerializeField] private float jumpHeight = 5f;
    [SerializeField] private float jumpCooldown = 0.1f;
    [SerializeField] private int totalJumps = 2;
    [SerializeField] private float gravityIntensity = -9.8f;
    [SerializeField] private float groundSlamInitialForce = 5f;
    [SerializeField] private float groundSlamForce = 20f;
    [SerializeField] private float groundSlamWait = 0.5f;
    
    [Header("Ground Check")] 
    [SerializeField] private float groundCheckDistance = 0.5f;
    [SerializeField] private float raycastHeightOffset = 0.5f; // We want our raycast to begin a bit above our feet
    [SerializeField] private LayerMask groundLayer;

    [Header("Juice")] 
    public float lowRumbleFrequency = 0.25f;
    public float highRumbleFrequency = 1f;
    public float rumbleDuration = 0.25f;
    [SerializeField] private float fallingVelocityThreshold = 20f;
    [SerializeField] private CinemachineImpulseSource groundShakeImpulseSource;

    [Header("Others")] 
    [SerializeField] private float regularFOV = 45f;
    [SerializeField] private float swingingFOV = 75f;
    [SerializeField] private float fovChangeTime = 12.5f;
    [SerializeField] private Transform ledgeCheckTransform;
    [SerializeField] private float ledgeCheckLength;

    [HideInInspector] public bool isSprinting;
    [HideInInspector] public bool isJumping;
    [HideInInspector] public bool isGrounded = true;
    [HideInInspector] public bool isSwinging;
    [HideInInspector] public bool isAiming;
    [HideInInspector] public bool isGrappling;
    [HideInInspector] public bool isGroundSlamming;
    [HideInInspector] public float inAirTimer = 0.75f;

    private InputManager inputManager;
    private PlayerManager playerManager;
    private PlayerAnimatorManager animatorManager;
    private ScreenShakeManager screenShakeManager;
    private Rigidbody rb;
    private Transform cam;
    private CinemachineFreeLook freeLook;

    private PlayerVFX playerFX;

    private float hitDistance; // For visualization
    private Vector3 moveDirection;
    private bool canJump = true;
    private bool shouldHaveGravity = true;
    private int maxJumpCount;
    private int jumpCount;
    
    private void Start()
    {
        inputManager = GetComponent<InputManager>();
        playerManager = GetComponent<PlayerManager>();
        animatorManager = GetComponent<PlayerAnimatorManager>();
        screenShakeManager = FindObjectOfType<ScreenShakeManager>();

        playerFX = GetComponent<PlayerVFX>();

        rb = GetComponent<Rigidbody>();
        cam = Camera.main.transform;
        freeLook = GameObject.FindWithTag("CinemachineCamera").GetComponent<CinemachineFreeLook>();
        
        freeLook.m_Lens.FieldOfView = regularFOV;
        maxJumpCount = totalJumps;
    }

    public void HandleAllMovement()
    {
        switch (PlayerManager.State)
        {
            case (States.Grappling):
                rb.useGravity = false;
                isSwinging = false;
                maxJumpCount = 0;
                playerFX.SprintVFX(false);
                break;
            
            case (States.Swinging):
                rb.useGravity = true;
                isSwinging = true;
                maxJumpCount = 0;
                playerFX.SprintVFX(false);
                
                freeLook.m_Lens.FieldOfView =
                    Mathf.Lerp(freeLook.m_Lens.FieldOfView, swingingFOV, fovChangeTime * Time.deltaTime);

                HandleRotation();
                HandleAirMovement();
                break;
            
            case (States.Aerial):
                rb.useGravity = false;
                isSwinging = false;

                LedgeGrab();
                HandleGroundSlamming();
                HandleRotation();
                HandleFallingAndLanding();
                HandleAirMovement();
                break;
            
            case (States.Grounded):
                rb.useGravity = false;
                isSwinging = false;
                maxJumpCount = totalJumps;
                
                freeLook.m_Lens.FieldOfView =
                    Mathf.Lerp(freeLook.m_Lens.FieldOfView, regularFOV, fovChangeTime * Time.deltaTime);

                HandleFallingAndLanding();
                
                if (playerManager.isLockedInAnim || isJumping || isAiming) return;
                
                HandleRotation();
                HandleMovement();
                break;

            default:
                Debug.LogError("Something went wrong with the state machine!");
                break;
        }
    }

    private void HandleMovement()
    {
        rb.drag = regularDrag;
        
        moveDirection = (cam.forward * inputManager.verticalInput + cam.right * inputManager.horizontalInput)
            .normalized;

        moveDirection.y = 0;

        if (isSprinting) moveDirection *= sprintingSpeed;
        else
        {
            if (inputManager.moveAmount >= 0.5f) moveDirection *= runningSpeed;
            else moveDirection *= walkingSpeed;
        }
        
        Vector3 movementVelocity = moveDirection;
        rb.velocity = movementVelocity;
        playerFX.SprintVFX(isSprinting && isGrounded);
    }

    private void HandleAirMovement()
    {
        rb.drag = aerialDrag;
        
        moveDirection = (cam.forward * inputManager.verticalInput + cam.right * inputManager.horizontalInput)
            .normalized;

        if (inputManager.moveAmount > 0.5f)
            rb.velocity = new Vector3(moveDirection.x * aerialMovementSpeed, rb.velocity.y,
                moveDirection.z * aerialMovementSpeed);
    }

    private void HandleRotation()
    {
        if (isAiming) return;
        
        Vector3 targetDirection = Vector3.zero;

        targetDirection = (cam.forward * inputManager.verticalInput + cam.right * inputManager.horizontalInput)
            .normalized;
        targetDirection.y = 0;

        if (targetDirection == Vector3.zero) targetDirection = transform.forward;

        Quaternion targetRot = Quaternion.LookRotation(targetDirection);
        Quaternion playerRot = Quaternion.Slerp(transform.rotation, targetRot, rotationSpeed * Time.deltaTime);

        transform.rotation = playerRot;
    }

    public void HandleJumping()
    {
        // TODO: Fix jumping
        if (isGrounded || jumpCount < maxJumpCount)
        {
            if (!canJump || isSwinging) return;
            jumpCount++;
            playerFX.SprintVFX(false);

            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
            if (jumpCount == 1)
            {
                animatorManager.Animator.SetBool("isJumping", true);
                animatorManager.PlayTargetAnimation("Jump", false);
                
                float jumpForce = Mathf.Sqrt(-2 * gravityIntensity * jumpHeight);
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            }
            else
            {
                animatorManager.Animator.SetBool("isJumping", true);
                playerFX.DoubleJumpVFX();
                animatorManager.PlayTargetAnimation("JumpFlip", false);
                
                float jumpForce = Mathf.Sqrt(-2 * gravityIntensity * (jumpHeight + 2f));
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            }

            GameManager.Player.GetComponentInChildren<PlayerSounds>().PlayJumping(jumpCount == totalJumps ? true : false);
        }
    }

    // Seems like this function doesn't need much changing
    private void HandleFallingAndLanding()
    {
        bool isLockedInAnim = playerManager.isLockedInAnim;

        Vector3 raycastOrigin = transform.position;
        raycastOrigin.y += raycastHeightOffset;

        Vector3 targetPos = transform.position;

        // Gravity
        if (!isGrounded && !isJumping && !isSwinging && !isGrappling && shouldHaveGravity)
        {
            if (!isLockedInAnim) animatorManager.PlayTargetAnimation("Falling", true);

            inAirTimer += Time.deltaTime;

            rb.AddForce(transform.forward * leapingVelocity);
            rb.AddForce(Vector3.down * (fallingVelocity * inAirTimer));
            
            // This might potentially cause issues
            PlayerManager.UpdateState(States.Aerial);
        }

        // Grounding
        if (Physics.SphereCast(raycastOrigin, 0.2f, Vector3.down, out var hit, groundCheckDistance, groundLayer))
        {
            if (!isGrounded) StartCoroutine(JumpCooldown());
            
            // If we're near the ground but not grounded AND are locked in an animation (Falling), play the landing animation
            if (!isGrounded && isLockedInAnim)
            {
                float rumbleIntensity = Mathf.Clamp(inAirTimer / 2.5f, 0.1f, 1f);

                if (rb.velocity.y < -fallingVelocityThreshold)
                {
                    RumbleManager.Instance.StartRumble(lowRumbleFrequency * rumbleIntensity,
                        highRumbleFrequency * rumbleIntensity, rumbleDuration, false);
                    screenShakeManager.ScreenShake(groundShakeImpulseSource, -rb.velocity.y);
                }

                animatorManager.PlayTargetAnimation("Land", true);

                GameManager.Player.GetComponentInChildren<PlayerSounds>().PlayLanding(isGroundSlamming ? groundSlamForce : -rb.velocity.y);
            }

            Vector3 raycastHitPoint = hit.point;
            targetPos.y = raycastHitPoint.y; // Our target position is the position where the raycast hits the ground
            hitDistance = hit.distance;
            
            inAirTimer = 0.75f;
            isGrounded = true;
            maxJumpCount = totalJumps;
            isGroundSlamming = false;

            PlayerManager.UpdateState(States.Grounded);

            animatorManager.Animator.SetBool("isLockedInAnim", false);
        }
        else isGrounded = false;

        // Floating capsule
        if (isGrounded && !isJumping && !isSwinging && !isGrappling && shouldHaveGravity)
        {
            if (playerManager.isLockedInAnim || inputManager.moveAmount > 0f)
                transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime / 0.1f);
            else transform.position = targetPos;
        }
    }

    private IEnumerator JumpCooldown()
    {
        canJump = false;
        yield return new WaitForSeconds(jumpCooldown);
        canJump = true;
        jumpCount = 0;
    }

    private void HandleGroundSlamming()
    {
        if (PlayerManager.State != States.Aerial || isGroundSlamming) return;
        if (!inputManager.groundSlamInput) return;

        StartCoroutine(GroundSlam());
    }

    private IEnumerator GroundSlam()
    {
        shouldHaveGravity = false;
        rb.velocity = Vector3.zero;
        yield return new WaitForSeconds(groundSlamWait);
        rb.AddForce(Vector3.down * groundSlamInitialForce, ForceMode.Impulse);
        shouldHaveGravity = true;

        animatorManager.Animator.SetBool("isLockedInAnim", true);
        rb.velocity = Vector3.zero;
        rb.AddForce(Vector3.down * groundSlamForce, ForceMode.Impulse);
        
        RumbleManager.Instance.StartRumble(0.5f, 1f, 0.75f);

        isGroundSlamming = true;
    }

    private void LedgeGrab()
    {
        // TODO: Make this actually ledge grab instead of an instant change
        bool foundLedge = Physics.Raycast(ledgeCheckTransform.position, Vector3.down, out RaycastHit hit,
            ledgeCheckLength);
        
        if (!foundLedge) return;
        
        Vector3 hitPoint = hit.point;
        transform.position = hitPoint;
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        
        // Ground check
        Vector3 raycastOrigin = transform.position;
        raycastOrigin.y += raycastHeightOffset;
        
        Gizmos.DrawLine(raycastOrigin, raycastOrigin + Vector3.down * groundCheckDistance);
        Gizmos.DrawWireSphere(raycastOrigin + Vector3.down * hitDistance, 0.2f);
        
        // Ledge
        Vector3 ledgeOrigin = ledgeCheckTransform.position;
        Gizmos.DrawLine(ledgeOrigin, ledgeOrigin + Vector3.down * ledgeCheckLength);
    }
}