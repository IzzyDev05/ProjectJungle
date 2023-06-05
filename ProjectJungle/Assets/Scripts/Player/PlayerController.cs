using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float maxSpeed = 5f;
    [SerializeField] private float jumpForce;

    [Header("GroundCheck")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundDrag = 3f;

    [Header("References")]
    [SerializeField] private Transform orientation;
    [SerializeField] private Transform groundCheckObj;

    private float horizontalInput;
    private float verticalInput;
    private Vector3 moveDirection;
    private Rigidbody rb;
    private CapsuleCollider coll;
    private PlayerAnimation playerAnimation;

    private void Start() {
        rb = GetComponent<Rigidbody>();
        coll = GetComponent<CapsuleCollider>();
        playerAnimation = GetComponentInChildren<PlayerAnimation>();
    }

    private void Update() {
        print(IsGrounded());
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded()) {
            Jump();
        }
        ManageDrag();
    }

    private void FixedUpdate() {
        MovePlayer();
    }

    private void MovePlayer() {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        rb.AddForce(moveDirection * moveSpeed, ForceMode.Impulse);
        moveDirection = Vector3.zero;

        // Makes the falling a bit better
        if (rb.velocity.y < 0) {
            rb.velocity -= Vector3.down * Physics.gravity.y * Time.fixedDeltaTime;
        }
        SpeedCap();
    }

    private void Jump() {
        playerAnimation.SetJumpingTrue();
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }


    // UTILITY FUNCTIONS
    private void ManageDrag() {
        if (IsGrounded()) rb.drag = groundDrag;
        else rb.drag = 0f;
    }

    private bool IsGrounded() {
        float radius = coll.radius * 0.9f;
        Vector3 pos = transform.position + Vector3.up * (radius * 0.9f);
        return Physics.CheckSphere(pos, radius, groundLayer);
    }

    private void SpeedCap() {
        Vector3 horizontalVelocity = rb.velocity;
        horizontalVelocity.y = 0;

        if (horizontalVelocity.sqrMagnitude > maxSpeed * maxSpeed) {
            rb.velocity = horizontalVelocity.normalized * maxSpeed + Vector3.up * rb.velocity.y;
        }
    }

    public float GetMaxSpeed() {
        return maxSpeed;
    }
}