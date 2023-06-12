using UnityEngine;

public class Swinging : MonoBehaviour
{
    [Header("Swinging values")]
    [SerializeField] private float maxSwingDistance = 25f;
    [SerializeField] private float jointSpring = 4.5f;
    [SerializeField] private float jointDamper = 7f;
    [SerializeField] private float jointMassScale = 4.5f;

    [Header("References")]
    [SerializeField] private Transform gunTip;
    [SerializeField] private GameObject player;
    [SerializeField] private LayerMask swingLayer;

    private PlayerInputManager inputManager;
    private PlayerLocomotion playerLocomotion;
    private LineRenderer lr;
    private SpringJoint joint;
    private Transform cam;
    
    private bool isSwinging;
    private Vector3 swingPoint = Vector3.zero;

    private void Start() {
        inputManager = GetComponentInParent<PlayerInputManager>();
        playerLocomotion = GetComponentInParent<PlayerLocomotion>();
        lr = GetComponent<LineRenderer>();
        cam = Camera.main.transform;
    }

    private void Update() {
        if (inputManager.AimInput) {
            // Aiming logic

            if (inputManager.SwingInput) {
                InitializeSwing();
            }
            else {
                EndSwing();
            }

            playerLocomotion.IsSwinging = isSwinging;
        }
    }

    private void LateUpdate() {
        DrawRope();
    }

    private void InitializeSwing() {
        Ray initialRay = new Ray(cam.position, cam.forward);

        if (Physics.Raycast(initialRay, out RaycastHit hit, maxSwingDistance, swingLayer)) {
            // Save the hit point
            Vector3 hitPoint = hit.point;

            // Create a ray from hit point to the player's forward
            Vector3 playerForward = gunTip.forward;
            Ray playerRay = new Ray(hitPoint, playerForward);

            // Check if player can see the hit spot
            if (Physics.Raycast(playerRay, out RaycastHit playerHit, maxSwingDistance)) {
                lr.positionCount = 2;

                if (!isSwinging) BeginSwing(hitPoint);
            }
            else {
                swingPoint = Vector3.zero;
            }
        }
        else {
            swingPoint = Vector3.zero;
        }
    }

    private void BeginSwing(Vector3 hitPoint) {
        swingPoint = hitPoint;
        isSwinging = true;

        joint = player.AddComponent<SpringJoint>();
    
        joint.autoConfigureConnectedAnchor = false;
        joint.connectedAnchor = swingPoint;

        float distanceFromPoint = Vector3.Distance(gunTip.transform.position, swingPoint);

        joint.maxDistance = distanceFromPoint * 0.6f;
        joint.minDistance = distanceFromPoint * 0.25f;

        joint.spring = jointSpring;
        joint.damper = jointDamper;
        joint.massScale = jointMassScale;
    }

    private void EndSwing() {
        isSwinging = false;

        Destroy(joint);

        lr.positionCount = 0;
        swingPoint = Vector3.zero;
    }

    private Vector3 currentGrapplePosition = Vector3.zero;
    private void DrawRope() {
        if (swingPoint == Vector3.zero) return;

        currentGrapplePosition = Vector3.Lerp(currentGrapplePosition, swingPoint, Time.deltaTime * 8f); 
        lr.SetPosition(0, gunTip.position);
        lr.SetPosition(1, currentGrapplePosition);
    }

    private void OnDrawGizmos() {
        if (!inputManager.AimInput) return;

        Ray initialRay = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        Gizmos.color = Color.red;
        Gizmos.DrawRay(initialRay);

        // Create a ray from the hit point to the player's forward
        if (Physics.Raycast(initialRay, out RaycastHit hit, maxSwingDistance, swingLayer))
        {
            Vector3 hitPoint = hit.point;

            // Calculate the direction from the hit point to the player's forward
            Vector3 hitToPlayer = gunTip.position - hitPoint;

            // Draw initial ray from camera forward to hit point
            Gizmos.color = Color.red;
            Gizmos.DrawRay(initialRay.origin, initialRay.direction * hit.distance);

            // Draw player ray from hit point to player forward
            Gizmos.color = Color.green;
            Gizmos.DrawRay(hitPoint, hitToPlayer.normalized * maxSwingDistance);
        }
    }
}