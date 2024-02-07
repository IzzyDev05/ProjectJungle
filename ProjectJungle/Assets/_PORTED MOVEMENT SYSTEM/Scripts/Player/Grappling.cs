using System.Collections;
using UnityEngine;

[RequireComponent(typeof(PlayerLocomotion))]
public class Grappling : MonoBehaviour
{
    [Header("Grappling Variables")]
    [SerializeField] private Transform grapplePointRef;
    [SerializeField] private float maxGrappleDistance = 100f;
    [SerializeField] private float grappleForce = 20f;
    [SerializeField] private LayerMask grappleLayer;
    [SerializeField] private float grapplingCD = 0.75f;
    
    [Header("Joint Variables")] 
    [SerializeField] private float swingForce = 4.5f;
    [SerializeField] private float springDamper = 7f;
    [SerializeField] private float springMassScale = 4.5f;
    [SerializeField] private float minDistance = 0.25f;
    [SerializeField] private float maxDistance = 0.75f;
    
    [Header("References")]
    [SerializeField] private GameObject freeLookCam;
    [SerializeField] private GameObject aimCam;
    
    private Camera cam;
    private PlayerLocomotion playerLocomotion;
    private PlayerManager playerManager;
    private InputManager inputManager;
    private Swinging swinging;
    private LineRenderer lr;
    private Rigidbody rb;
    private SpringJoint spring;

    private bool isGrappling;
    private bool canGrapple = true;
    private Vector3 grapplePoint;

    private void Start()
    {
        freeLookCam.SetActive(true);
        aimCam.SetActive(false);

        cam = Camera.main;
        playerLocomotion = GetComponent<PlayerLocomotion>();
        playerManager = GetComponent<PlayerManager>();
        inputManager = GetComponent<InputManager>();
        swinging = GetComponent<Swinging>();
        lr = GetComponent<LineRenderer>();
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        playerLocomotion.isGrappling = isGrappling;
     
        if (playerManager.isLockedInAnim) return;
        HandleAimingAndGrappling();
    }

    private void LateUpdate()
    {
        if (isGrappling) DrawRope();
    }
    
    private Vector3 currentGrapplePosition = Vector3.zero;
    private void DrawRope()
    {
        currentGrapplePosition = Vector3.Lerp(currentGrapplePosition, grapplePoint, Time.deltaTime * 4f);

        if (lr.positionCount == 0) lr.positionCount = 2;
        
        lr.SetPosition(0, grapplePointRef.position);
        lr.SetPosition(1, grapplePoint);
    }

    private void HandleAimingAndGrappling()
    {
        if (playerLocomotion.isSwinging) return;

        if (inputManager.rightMouse)
        {
            StartAiming();
            if (inputManager.leftMouse) StartGrappling();
        }
        else StopAimingAndGrappling();
    }

    private void StartAiming()
    {
        playerLocomotion.isAiming = true;

        freeLookCam.SetActive(false);
        aimCam.SetActive(true);
        
        Vector3 rayOrigin = cam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.0f));
        if (Physics.Raycast(rayOrigin, aimCam.transform.forward, out RaycastHit hit, maxGrappleDistance, grappleLayer))
        {
            grapplePoint = hit.point;
        }
    }

    private void StartGrappling()
    {
        if (!canGrapple) return;
        
        isGrappling = true;
        
        RumbleManager.Instance.StartRumble(playerLocomotion.lowRumbleFrequency, playerLocomotion.highRumbleFrequency,
            0f, true);
        
        PlayerManager.UpdateState(States.Grappling);
        
        // Grapple logic
        if (spring) return;

        spring = gameObject.AddComponent<SpringJoint>();

        ApplySpringJointValues();
        rb.AddForce(transform.forward * grappleForce, ForceMode.Impulse);

        playerLocomotion.inAirTimer = 0f;
    }

    private void ApplySpringJointValues()
    {
        spring.autoConfigureConnectedAnchor = false;
        spring.anchor = grapplePointRef.localPosition;
        spring.connectedAnchor = grapplePoint;

        spring.minDistance = minDistance;
        spring.maxDistance = maxDistance;

        spring.spring = swingForce;
        spring.damper = springDamper;
        spring.massScale = springMassScale;   
    }

    private void StopAimingAndGrappling()
    {
        if (spring) Destroy(spring);
        
        if (isGrappling)
        {
            var previousState = PlayerManager.PreviousState;
            PlayerManager.UpdateState(previousState);
            
            RumbleManager.Instance.StopRumble();

            lr.positionCount = 0;
            StartCoroutine(GrappleCooldown());
            StartCoroutine(swinging.SwingCooldown());
            isGrappling = false;
        }
        
        playerLocomotion.isAiming = false;

        freeLookCam.SetActive(true);
        aimCam.SetActive(false);
    }

    private IEnumerator GrappleCooldown()
    {
        canGrapple = false;
        yield return new WaitForSeconds(grapplingCD);
        canGrapple = true;
    }
}