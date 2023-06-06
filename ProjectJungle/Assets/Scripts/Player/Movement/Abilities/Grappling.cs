using UnityEngine;
using Cinemachine;

public class Grappling : MonoBehaviour
{
    [Header("Temporary")]
    [SerializeField] private bool grapplingMode;
    [SerializeField] private CinemachineFreeLook lookCam;
    [SerializeField] private Transform lookAtPoint;
    [SerializeField] private Transform playerPoint;
    [SerializeField] GameObject crosshair;

    [Header("Grappling")]
    [SerializeField] private float maxGrappleDistance = 25f;
    [SerializeField] private float grappleDelayTime = 0.25f;
    [SerializeField] private float grappleCD = 1f;
    [SerializeField] private float overshootYAxis = 2f;

    [Header("References")]
    [SerializeField] private Transform gunTip;
    [SerializeField] private LayerMask grappableLayer;
    [SerializeField] private LineRenderer lr;

    private Transform cam;
    private PlayerInputManager inputManager;
    private PlayerLocomotion playerLocomotion;
    private Vector3 grapplePoint;
    private float grappleCDTimer;
    private bool grappleInput;
    private bool isGrappling;

    private void Start() {
        cam = Camera.main.transform;
        inputManager = GetComponent<PlayerInputManager>();
        playerLocomotion = GetComponent<PlayerLocomotion>();
    }

    private void Update() {
        grapplingMode = inputManager.TempGrappleMode;
        if (grapplingMode) {
            crosshair.SetActive(true);
            lookCam.LookAt = lookAtPoint;
        }
        else {
            crosshair.SetActive(false);
            lookCam.LookAt = playerPoint;
        }

        grappleInput = inputManager.GrappleInput;

        if (grappleInput) StartGrapple();
        if (grappleCDTimer > 0) grappleCDTimer -= Time.deltaTime;
    }

    private void LateUpdate() {
        if (isGrappling) {
            lr.SetPosition(0, gunTip.position);
        }
    }

    private void StartGrapple() {
        if (grappleCDTimer > 0) return;

        isGrappling = true;

        RaycastHit hit;
        if (Physics.Raycast(cam.position, cam.forward, out hit, maxGrappleDistance, grappableLayer)) {
            grapplePoint = hit.point;
            Invoke(nameof(ExecuteGrapple), grappleDelayTime);
        }
        else {
            grapplePoint = cam.position + cam.forward * maxGrappleDistance;
            Invoke(nameof(StopGrapple), grappleDelayTime);
        }

        lr.enabled = true;
        lr.SetPosition(1, grapplePoint);
    }

    private void ExecuteGrapple() {
        Vector3 lowestPoint = new Vector3(transform.position.x, transform.position.y - 1f, transform.position.z);
        float grapplePointRelativeYPos = grapplePoint.y - lowestPoint.y;
        float highestPointOnArc = grapplePointRelativeYPos + overshootYAxis;

        if (grapplePointRelativeYPos < 0) highestPointOnArc = overshootYAxis;

        playerLocomotion.JumpToPosition(grapplePoint, highestPointOnArc);
        Invoke(nameof(StopGrapple), 1f);
    }

    private void StopGrapple() {
        isGrappling = false;
        playerLocomotion.IsGrappling = false;
        lr.enabled = false;
        grappleCDTimer = grappleCD;
    }
}