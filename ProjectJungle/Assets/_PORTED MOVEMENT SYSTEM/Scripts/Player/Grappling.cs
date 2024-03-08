using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

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
    [SerializeField] private float jointForce = 4.5f;
    [SerializeField] private float jointDamper = 7f;
    [SerializeField] private float jointMassScale = 4.5f;
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
     
        //if (playerManager.isLockedInAnim) return;
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
        // TODO: Make camera movable while aiming
        if (playerLocomotion.isSwinging) return;

        if (inputManager.rightMouse)
        {
            Cursor.lockState = CursorLockMode.Confined;
            
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

        // TODO: Make this stuff better
        /*
        if (PlayerManager.State == States.Aerial) Time.timeScale = 0.25f;
        else
            if (rb.velocity != Vector3.zero) rb.velocity = Vector3.zero;
        */
        
        Vector3 rayOrigin = cam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.0f));
        if (Physics.Raycast(rayOrigin, aimCam.transform.forward, out RaycastHit hit, maxGrappleDistance, grappleLayer))
        {
            grapplePoint = hit.point;
        }
    }

    private void StartGrappling()
    {
        if (!canGrapple || grapplePoint == Vector3.zero) return;
        
        isGrappling = true;
        PlayerManager.UpdateState(States.Grappling);
        
        // Grapple logic
        if (spring) return;
        
        RumbleManager.Instance.StartRumble(playerLocomotion.lowRumbleFrequency, playerLocomotion.highRumbleFrequency,
            0f, true);

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

        spring.spring = jointForce;
        spring.damper = jointDamper;
        spring.massScale = jointMassScale;   
    }

    private void StopAimingAndGrappling()
    {
        if (spring) Destroy(spring);
        if (Time.timeScale != 1f) Time.timeScale = 1f;
        
        if (isGrappling)
        {
            var previousState = PlayerManager.PreviousState;
            PlayerManager.UpdateState(previousState);
            
            RumbleManager.Instance.StopRumble();

            lr.positionCount = 0;
            grapplePoint = Vector3.zero;
            StartCoroutine(GrappleCooldown());
            StartCoroutine(swinging.SwingCooldown());
            
            Cursor.lockState = CursorLockMode.Confined;
            
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