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

    [Header("Aerial Speeds")] 
    [SerializeField] private float leapingVelocity = 1.5f;
    [SerializeField] private float fallingVelocity = 33f;
    [SerializeField] private float jumpHeight = 5f;
    [SerializeField] private float jumpCooldown = 0.5f;
    [SerializeField] private float gravityIntensity = -9.8f;
    [SerializeField] private float groundSlamForce = 20f;

    [Header("Ground Check")] 
    [SerializeField] private float groundCheckDistance = 0.5f;
    [SerializeField] private float raycastHeightOffset = 0.5f; // We want our raycast to begin a bit above our feet
    [SerializeField] private LayerMask groundLayer;

    [Header("Juice")] 
    public float lowRumbleFrequency = 0.25f;
    public float highRumbleFrequency = 1f;
    public float rumbleDuration = 0.25f;

    [Header("Others")] 
    [SerializeField] private float regularFOV = 45f;
    [SerializeField] private float swingingFOV = 75f;
    [SerializeField] private float fovChangeTime = 12.5f;

    [HideInInspector] public bool isSprinting;
    [HideInInspector] public bool isJumping;
    [HideInInspector] public bool isGrounded = true;
    [HideInInspector] public bool isSwinging;
    [HideInInspector] public bool isAiming;
    [HideInInspector] public bool isGrappling;
    [HideInInspector] public bool isGroundSlamming;
    [HideInInspector] public float inAirTimer;

    private InputManager inputManager;
    private PlayerManager playerManager;
    private PlayerAnimatorManager animatorManager;
    private Rigidbody rb;
    private Transform cam;
    private CinemachineFreeLook freeLook;

    private Vector3 moveDirection;
    private bool canJump = true;
    private int maxJumpCount = 2;
    private int jumpCount;
    
    private void Start()
    {
        inputManager = GetComponent<InputManager>();
        playerManager = GetComponent<PlayerManager>();
        animatorManager = GetComponent<PlayerAnimatorManager>();

        rb = GetComponent<Rigidbody>();
        cam = Camera.main.transform;
        freeLook = GameObject.FindWithTag("CinemachineCamera").GetComponent<CinemachineFreeLook>();
        
        freeLook.m_Lens.FieldOfView = regularFOV;
    }

    public void HandleAllMovement()
    {
        switch (PlayerManager.State)
        {
            case (States.Grappling):
                rb.useGravity = false;
                isSwinging = false;
                
                break;
            
            case (States.Swinging):
                rb.useGravity = true;
                isSwinging = true;
                
                freeLook.m_Lens.FieldOfView =
                    Mathf.Lerp(freeLook.m_Lens.FieldOfView, swingingFOV, fovChangeTime * Time.deltaTime);

                HandleRotation();
                break;
            
            case (States.Aerial):
                rb.useGravity = false;
                isSwinging = false;

                HandleGroundSlamming();
                HandleRotation();
                HandleFallingAndLanding();
                break;
            
            case (States.Grounded):
                rb.useGravity = false;
                isSwinging = false;
                
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
    }

    private void HandleRotation()
    {
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
        if (!canJump) return;

        if (isGrounded || jumpCount < maxJumpCount)
        {
            jumpCount++;
            
            animatorManager.Animator.SetBool("isJumping", true); // isJumping is set to false after animation is over
            animatorManager.PlayTargetAnimation("Jump", false);

            float jumpingVelocity = Mathf.Sqrt(-2 * gravityIntensity * jumpHeight);
            Vector3 playerVelocity = moveDirection;
            playerVelocity.y = jumpingVelocity;
            rb.velocity = playerVelocity;
        }
    }

    private void HandleFallingAndLanding()
    {
        bool isLockedInAnim = playerManager.isLockedInAnim;

        Vector3 raycastOrigin = transform.position;
        raycastOrigin.y += raycastHeightOffset;

        Vector3 targetPos = transform.position;

        if (!isGrounded && !isJumping && !isSwinging && !isGrappling)
        {
            if (!isLockedInAnim) animatorManager.PlayTargetAnimation("Falling", true);

            inAirTimer += Time.deltaTime;

            rb.AddForce(transform.forward * leapingVelocity);
            rb.AddForce(Vector3.down * (fallingVelocity * inAirTimer));
            
            // This might potentially cause issues
            PlayerManager.UpdateState(States.Aerial);
        }

        if (Physics.SphereCast(raycastOrigin, 0.2f, Vector3.down, out var hit, groundCheckDistance, groundLayer))
        {
            // If we're near the ground but not grounded AND are locked in an animation (Falling), play the landing animation
            if (!isGrounded && isLockedInAnim)
            {
                float rumbleIntensity = Mathf.Clamp(inAirTimer / 5f, 0.1f, 1f);

                RumbleManager.Instance.StartRumble(lowRumbleFrequency * rumbleIntensity,
                    highRumbleFrequency * rumbleIntensity, rumbleDuration, false);
                
                animatorManager.PlayTargetAnimation("Land", true);
            }

            Vector3 raycastHitPoint = hit.point;
            targetPos.y = raycastHitPoint.y; // Our target position is the position where the raycast hits the ground

            inAirTimer = 0f;
            isGrounded = true;
            StartCoroutine(JumpCooldown());
            isGroundSlamming = false;

            PlayerManager.UpdateState(States.Grounded);

            animatorManager.Animator.SetBool("isLockedInAnim", false);
        }
        else isGrounded = false;

        // Floating capsule
        if (isGrounded && !isJumping)
        {
            if (playerManager.isLockedInAnim || inputManager.moveAmount > 0f)
            {
                transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime / 0.1f);
            }
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

        animatorManager.Animator.SetBool("isLockedInAnim", true);
        rb.velocity = Vector3.zero;
        print("Ground slam");
        rb.AddForce(Vector3.down * groundSlamForce, ForceMode.Impulse);

        isGroundSlamming = true;
    }
}