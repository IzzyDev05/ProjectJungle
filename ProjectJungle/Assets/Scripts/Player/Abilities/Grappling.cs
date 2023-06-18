using UnityEngine;

public class Grappling : MonoBehaviour
{
    [Header("Grappling Values")]
    [SerializeField] private float grappleSpeed = 15f;
    [SerializeField] private float grappleDistance = 25f;

    [Header("References")]
    [SerializeField] private Transform gunTip;
    [SerializeField] private LayerMask grappleLayer;
    
    private PlayerInputManager inputManager;
    private Rigidbody playerRB;
    private Transform cam;
    private bool isGrappling;
    private Vector3 grappleTarget;

    private void Start() {
        inputManager = GetComponentInParent<PlayerInputManager>();
        playerRB = GetComponentInParent<Rigidbody>();
        cam = Camera.main.transform;
    }

    private void Update() {
        if (inputManager.AimInput && inputManager.SwingInput) {
            InitializeGrapple();
        }
    }

    private void FixedUpdate() {
        if (isGrappling) {
            MoveTowardsGrappleTarget();
        }
    }

    private void InitializeGrapple() {
        Ray initialRay = new Ray(cam.position, cam.forward);

        if (Physics.Raycast(initialRay, out RaycastHit hit, grappleDistance, grappleLayer)) {
            // Save the hit point
            Vector3 hitPoint = hit.point;

            // Create a ray from hit point to the player's forward
            Vector3 playerForward = gunTip.forward;
            Ray playerRay = new Ray(hitPoint, playerForward);

            // Check if player can see the hit spot
            if (Physics.Raycast(playerRay, out RaycastHit playerHit, grappleDistance)) {
                grappleTarget = hit.point;
                isGrappling = true;
            }
            else {
                // swingPoint = Vector3.zero;
            }
        }
        else {
            // swingPoint = Vector3.zero;
        }
    }

    private void MoveTowardsGrappleTarget() {
        Vector3 grappleDirection = (grappleTarget - transform.position).normalized;
        playerRB.velocity = grappleDirection * grappleSpeed;

        float distanceToTarget = Vector3.Distance(transform.position, grappleTarget);
        if (distanceToTarget < 1f) {
            EndGrapple();
        }
    }

    private void EndGrapple() {
        isGrappling = false;
        playerRB.velocity = Vector3.zero;
    }
}