using UnityEngine;

public class PlayerLocomotion : MonoBehaviour
{
    [Header("Grounded speeds")]
    [SerializeField] private float walkingSpeed = 4f;
    [SerializeField] private float runningSpeed = 8f;
    [SerializeField] private float sprintingSpeed = 15f;
    [SerializeField] private float rotationSpeed = 15f;

    [Header("Aerial speeds")]
    [SerializeField] private float jumpHeight = 5f;
    [SerializeField] private float leapingSpeed = 2f;
    [SerializeField] private float fallingSpeed = 6f;

    [Header("General")]
    [SerializeField] private float gravityIntensity = -9.81f;

    [Header("References")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    private PlayerInputManager inputManager;
    private PlayerManager playerManager;
    private PlayerAnimationManager animationManager;
    private CharacterController characterController;
    private Rigidbody rb;
    private Transform cam;
    private Vector3 moveDirection;
    
    [SerializeField] private bool isSprinting;
    [SerializeField] private bool isGrounded = true;
    [SerializeField] private bool isJumping;

    #region PROPERTIES
    public bool IsSprinting {
        get {
            return isSprinting;
        }
        set {
            isSprinting = value;
        }
    }
    public bool IsGrounded {
        get {
            return isGrounded;
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
    #endregion

    private void Start() {
        inputManager = GetComponent<PlayerInputManager>();
        playerManager = GetComponent<PlayerManager>();
        animationManager = GetComponentInChildren<PlayerAnimationManager>();
        characterController = GetComponent<CharacterController>();
        rb = GetComponent<Rigidbody>();
        cam = Camera.main.transform;
    }

    public void HandleAllInputs() {
        inputManager.HandleAllInputs();
        isGrounded = IsPlayerGrounded();
        
        HandleFallingAndLanding();

        if (playerManager.IsInteracting) return;

        HandleMovement();
        HandleRotation();
    }

    private void HandleMovement() {
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
        characterController.Move(movementVelocity * Time.deltaTime);
    }

    private void HandleRotation() {
        Vector3 targetDirection = Vector3.zero;
        targetDirection = cam.forward * inputManager.VerticalInput + cam.right * inputManager.HorizontalInput;
        moveDirection.Normalize();
        targetDirection.y = 0f;

        if (targetDirection == Vector3.zero) targetDirection = transform.forward;

        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        Quaternion playerRotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        transform.rotation = playerRotation;
    }

    private void HandleFallingAndLanding() {
        if (!isGrounded && !isJumping) {
            if (!playerManager.IsInteracting) {
                animationManager.PlayTargetAnimation("Falling", true);
            }

            characterController.Move(transform.forward * leapingSpeed * Time.deltaTime);
            characterController.Move(Vector3.down * fallingSpeed * Time.deltaTime);
        }

        if (Physics.Raycast(groundCheck.position, Vector3.down, 1f, groundLayer)) {
            if (!isGrounded) {
                animationManager.PlayTargetAnimation("Landing", true);
            }
        }
    }

    public void HandleJumping() {
        if (!isGrounded) return;
        
        animationManager.SetBool("isJumping", true);
        animationManager.PlayTargetAnimation("Jump", false);

        float jumpSpeed = Mathf.Sqrt(-3 * gravityIntensity * jumpHeight);
        Vector3 playerVelocity = moveDirection;
        playerVelocity.y += jumpSpeed;
        characterController.Move(playerVelocity * Time.deltaTime);
    }

    private bool IsPlayerGrounded() {
        return Physics.Raycast(groundCheck.position, Vector3.down, 0.2f, groundLayer);
    }

    private void OnDrawGizmos() {
        RaycastHit hit;
        bool isHit = Physics.Raycast(groundCheck.position, Vector3.down, out hit, 0.2f, groundLayer);

        // Draw the raycast in the scene view
        Gizmos.color = isHit ? Color.green : Color.red;
        Gizmos.DrawRay(groundCheck.position, Vector3.down * 0.2f);
    }
}