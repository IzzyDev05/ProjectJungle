using UnityEngine;

public class Swinging : MonoBehaviour
{
    [Header("Swinging values")]
    [SerializeField] private float maxSwingDistance = 25f;
    [SerializeField] private float swingForce = 4.5f;
    [SerializeField] private float jointDamper = 7f;
    [SerializeField] private float jointMassScale = 4.5f;
    [Range(0, 1f)][SerializeField] private float maxDistanceMultiplier = 0.75f;
    [Range(0, 1f)][SerializeField] private float minDistanceMultiplier = 0.1f;

    [Header("References")]
    [SerializeField] private LayerMask swingableLayer;

    private Transform player;

    private PlayerInputManager inputManager;
    private PlayerLocomotion playerLocomotion;
    private LineRenderer lr;
    private SpringJoint joint;
    private Rigidbody playerRB;

    private Vector3 swingPoint = Vector3.zero;
    private Vector3 swingDirection;

    private void Start()
    {
        player = transform.parent;

        inputManager = player.GetComponent<PlayerInputManager>();
        playerLocomotion = player.GetComponent<PlayerLocomotion>();
        playerRB = player.GetComponent<Rigidbody>();
        lr = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        if (inputManager.SwingInput && !inputManager.AimInput) {
            FindSwingPoint();
        }
        else {
            EndSwing();
        }
    }

    private void LateUpdate()
    {
        DrawRope();
    }

    private void FindSwingPoint()
    {
        if (playerLocomotion.IsSwinging) return;

        float coneRadius = 5f;
        RaycastHit[] hits = Physics.SphereCastAll(player.position, coneRadius, player.up, maxSwingDistance, swingableLayer);

        PerformSphereCast(hits);

        if (swingPoint != Vector3.zero) BeginSwing();
    }

    private void PerformSphereCast(RaycastHit[] hits)
    {
        float maxScore = 0f;
        float minDistance = maxSwingDistance; // Initializing at maxSwingDistance because we want to check less than, not greater than

        foreach (RaycastHit hit in hits) {
            // Calculate the angle between the surface normal and the direction from the player's position to the hit point
            Vector3 directionToHit = hit.point - player.position;
            float angle = Vector3.Angle(hit.normal, directionToHit);

            // Calculate the absolute score based on the cosine of the angle
            float score = Mathf.Abs(Mathf.Cos(angle * Mathf.Deg2Rad));

            float distanceToPoint = Vector3.Distance(player.position, hit.point);

            // If this score is better than the previous score AND is closer, then select it as the swingPoint
            if (score >= maxScore) {
                maxScore = score;
                swingPoint = hit.point;

                if (distanceToPoint < minDistance) {
                    minDistance = distanceToPoint;
                    swingPoint = hit.point;
                }
            }
        }
    }

    private void BeginSwing()
    {
        playerLocomotion.IsSwinging = true;
        playerLocomotion.ShouldHaveAirMomentum = false;
        playerLocomotion.HasAddedForwardMomentum = false;
        playerLocomotion.InAirTimer = 1f;

        lr.positionCount = 2;

        ApplyJointValues();

        // Apply a force to simulate swinging motion
        swingDirection = (swingPoint - playerRB.position).normalized;
        Vector3 perpendicularDirection = Vector3.Cross(swingDirection, Vector3.up);
        playerLocomotion.SwingDirection = perpendicularDirection;
    }

    private void ApplyJointValues()
    {
        joint = player.gameObject.AddComponent<SpringJoint>();

        joint.autoConfigureConnectedAnchor = false;
        joint.connectedAnchor = swingPoint;

        float distanceFromPoint = Vector3.Distance(player.position, swingPoint);

        joint.maxDistance = distanceFromPoint * maxDistanceMultiplier;
        joint.minDistance = distanceFromPoint * minDistanceMultiplier;

        joint.spring = swingForce;
        joint.damper = jointDamper;
        joint.massScale = jointMassScale;
    }

    private void EndSwing()
    {
        if (playerLocomotion.IsSwinging) playerLocomotion.ShouldHaveAirMomentum = true;

        playerLocomotion.IsSwinging = false;
        swingPoint = Vector3.zero;

        Destroy(joint);

        lr.positionCount = 0;
        swingPoint = Vector3.zero;
    }

    private Vector3 currentGrapplePosition = Vector3.zero;
    private void DrawRope()
    {
        if (swingPoint == Vector3.zero) return;

        currentGrapplePosition = Vector3.Lerp(currentGrapplePosition, swingPoint, Time.deltaTime * 8f);
        lr.SetPosition(0, player.position);
        lr.SetPosition(1, swingPoint);
    }
}