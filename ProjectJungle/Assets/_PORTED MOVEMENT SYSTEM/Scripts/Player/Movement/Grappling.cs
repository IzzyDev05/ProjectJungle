using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(PlayerLocomotion))]
public class Grappling : MonoBehaviour
{
    [Header("Grappling Variables")] [SerializeField]
    private Transform grapplePointRef;

    [SerializeField] private float maxGrappleDistance = 100f;
    [SerializeField] private float grappleForce = 20f;
    [SerializeField] private LayerMask grappleLayer;
    [SerializeField] private float grapplingCD = 0.75f;
    [SerializeField] private float grappleStartDelay = 0.25f;

    [Header("Joint Variables")] [SerializeField]
    private float jointForce = 4.5f;

    [SerializeField] private float jointDamper = 7f;
    [SerializeField] private float jointMassScale = 4.5f;
    [SerializeField] private float minDistance = 0.25f;
    [SerializeField] private float maxDistance = 0.75f;

    [Header("References")] [SerializeField]
    private GameObject freeLookCam;

    [SerializeField] private GameObject aimCam;
    [SerializeField] private GameObject reticle;

    private Camera cam;
    private PlayerLocomotion playerLocomotion;
    private PlayerHandIK rightHandIK;
    private PlayerHeadIK headIK;
    private PlayerBodyIK bodyIK;
    private InputManager inputManager;
    private Swinging swinging;
    private Rigidbody rb;
    private SpringJoint spring;
    private RopeRenderer ropeRenderer;

    private bool currentlyGrappling;
    private bool canGrapple = true;
    private Vector3 grapplePoint;
    private bool hasStartedGrappleRoutine;

    private void Start()
    {
        freeLookCam.SetActive(true);
        aimCam.SetActive(false);
        reticle.SetActive(false);

        cam = Camera.main;
        playerLocomotion = GetComponent<PlayerLocomotion>();
        rightHandIK = GetComponent<PlayerHandIK>();
        headIK = GetComponent<PlayerHeadIK>();
        bodyIK = GetComponent<PlayerBodyIK>();
        inputManager = GetComponent<InputManager>();
        swinging = GetComponent<Swinging>();
        ropeRenderer = GetComponent<RopeRenderer>();
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        playerLocomotion.isGrappling = currentlyGrappling;
        HandleAimingAndGrappling();
    }

    private void LateUpdate()
    {
        rightHandIK.StartHandIK(currentlyGrappling, grapplePoint);
        headIK.StartHeadIK(currentlyGrappling, grapplePoint);
        bodyIK.StartBodyIK(currentlyGrappling, grapplePoint);
    }

    private void HandleAimingAndGrappling()
    {
        // TODO: Make camera movable while aiming
        if (playerLocomotion.isSwinging) return;

        if (inputManager.rightMouse)
        {
            Cursor.lockState = CursorLockMode.Confined;

            StartAiming();
            if (inputManager.leftMouse) StartCoroutine(DelayStartGrappling(grappleStartDelay));
        }
        else StopAimingAndGrappling();
    }

    private void StartAiming()
    {
        playerLocomotion.isAiming = true;
        freeLookCam.SetActive(false);
        aimCam.SetActive(true);
        reticle.SetActive(true);

        if (currentlyGrappling || !canGrapple) return;

        Vector3 rayOrigin = cam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.0f));

        if (Physics.Raycast(rayOrigin, cam.transform.forward, out RaycastHit hit, maxGrappleDistance, grappleLayer))
        {
            grapplePoint = hit.point;
        }
    }

    private IEnumerator DelayStartGrappling(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (!currentlyGrappling && canGrapple && grapplePoint != Vector3.zero)
        {
            currentlyGrappling = true;
            PlayerManager.UpdateState(States.Grappling);
            StartGrappling();

            if (!hasStartedGrappleRoutine)
            {
                ropeRenderer.StartDrawingRope(grapplePoint);
                hasStartedGrappleRoutine = true;
            }
        }
    }

    private void StartGrappling()
    {
        if (!canGrapple) return;

        RumbleManager.Instance.StartRumble(playerLocomotion.lowRumbleFrequency, playerLocomotion.highRumbleFrequency,
            0f, true);

        if (spring) return;

        spring = gameObject.AddComponent<SpringJoint>();
        ApplySpringJointValues();

        rb.AddForce(transform.forward * grappleForce, ForceMode.Impulse);
        playerLocomotion.inAirTimer = 0.5f;
    }

    private void ApplySpringJointValues()
    {
        spring.autoConfigureConnectedAnchor = false;
        spring.anchor = grapplePointRef.localPosition;
        spring.connectedAnchor = grapplePoint;

        spring.minDistance = minDistance;
        spring.maxDistance = maxDistance;

        spring.spring = jointForce;
        spring.damper = jointDamper;
        spring.massScale = jointMassScale;
    }

    private void StopAimingAndGrappling()
    {
        freeLookCam.SetActive(true);
        aimCam.SetActive(false);
        reticle.SetActive(false);
        
        if (spring) Destroy(spring);

        if (currentlyGrappling)
        {
            var previousState = PlayerManager.PreviousState;
            PlayerManager.UpdateState(previousState);

            RumbleManager.Instance.StopRumble();

            grapplePoint = Vector3.zero;
            StartCoroutine(GrappleCooldown());
            StartCoroutine(swinging.SwingCooldown());

            Cursor.lockState = CursorLockMode.Confined;

            rightHandIK.StopHandIK();
            headIK.StopHeadIK();
            bodyIK.StopBodyIK();

            if (hasStartedGrappleRoutine)
            {
                ropeRenderer.StopDrawingRope();
                hasStartedGrappleRoutine = false;
            }
        }
        
        currentlyGrappling = false;
        playerLocomotion.isAiming = false;
    }

    private IEnumerator GrappleCooldown()
    {
        canGrapple = false;
        yield return new WaitForSeconds(grapplingCD);

        canGrapple = true;
    }
}