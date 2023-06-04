using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveForce = 1f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float maxSpeed = 5f;
    [SerializeField] private float groundCheckDistance = 0.3f;
    [SerializeField] private LayerMask groundLayer;

    private ThirdPersonActionsAsset playerActionsAsset;
    private InputAction move;

    private Rigidbody rb;
    private Vector3 forceDirection = Vector3.zero;
    private Camera cam;

    private void Awake() {
        playerActionsAsset = new ThirdPersonActionsAsset();
    }

    private void OnEnable() {
        playerActionsAsset.Enable();
        playerActionsAsset.Default.Jump.started += Jump;
        move = playerActionsAsset.Default.Move;
    }

    private void OnDisable() {
        playerActionsAsset.Disable();
        playerActionsAsset.Default.Jump.started -= Jump;
    }

    private void Start() {
        rb = GetComponent<Rigidbody>();
        cam = Camera.main;
    }

    private void FixedUpdate() {
        forceDirection += move.ReadValue<Vector2>().x * GetCameraRight(cam) * moveForce;
        forceDirection += move.ReadValue<Vector2>().y * GetCameraForward(cam) * moveForce;

        rb.AddForce(forceDirection, ForceMode.Impulse);
        forceDirection = Vector3.zero;

        // Makes the falling feel better
        if (rb.velocity.y < 0f) {
            rb.velocity -= Vector3.down * Physics.gravity.y * Time.fixedDeltaTime;
        }

        // Capping the max horizontal speed
        Vector3 horizontalVelocity = rb.velocity;
        horizontalVelocity.y = 0;
        if (horizontalVelocity.sqrMagnitude > maxSpeed * maxSpeed) {
            rb.velocity = horizontalVelocity.normalized * maxSpeed + Vector3.up * rb.velocity.y;
        }
        LookAt();
    }

    // Makes sure we're controlling our RigidBody's rotation to where we are looking, and our player doesn't randomly rotate
    private void LookAt() {
        Vector3 direction = rb.velocity;
        direction.y = 0f;

        if (move.ReadValue<Vector2>().sqrMagnitude > 0.1f && direction.sqrMagnitude > 0.1f) {
            rb.rotation = Quaternion.LookRotation(direction, Vector3.up);
        }
        else {
            rb.angularVelocity = Vector3.zero;
        }
    }

    private void Jump(InputAction.CallbackContext context) {
        if (!IsGrounded()) return;
        forceDirection += Vector3.up * jumpForce;
    }


    // UTILITY FUNCTIONS
    private bool IsGrounded() {
        bool hit = Physics.Raycast(transform.position, Vector3.down, groundCheckDistance, groundLayer);

        if (hit) return true;
        else return false;
    }

    // Gets a normalized camera right direction (In case the camera is tilted)
    private Vector3 GetCameraRight(Camera camera) {
        Vector3 right = camera.transform.right;
        right.y = 0f;
        return right.normalized;
    }

    private Vector3 GetCameraForward(Camera camera) {
        Vector3 forward = camera.transform.forward;
        forward.y = 0f;
        return forward.normalized;
    }
}