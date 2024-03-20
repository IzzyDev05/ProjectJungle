using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(PlayerLocomotion))]
public class Swinging : MonoBehaviour
{
    [Header("Swinging Variables")] 
    [SerializeField] private float maxSwingDistance = 75f;
    [SerializeField] private LayerMask swingableLayer;
    [SerializeField] private Transform swingPointRef;
    [SerializeField] private float swingingCD = 0.75f;
    [SerializeField] private float swingingPointForwardOffset = 5f;
    
    [Header("Joint Variables")] 
    [SerializeField] private float swingForce = 4.5f;
    [SerializeField] private float springDamper = 7f;
    [SerializeField] private float springMassScale = 4.5f;
    [SerializeField, Range(0, 1f)] private float minDistanceMultiplier = 0.25f;
    [SerializeField, Range(0, 1f)] private float maxDistanceMultiplier = 0.75f;

    [Header("Forces")] 
    [SerializeField] private float initialForwardMomentum = 2.5f;
    [SerializeField] private float initialUpwardMomentum = 2.5f;
    [SerializeField] private float swingEndThrust = 2.5f;

    private PlayerLocomotion playerLocomotion;
    private InputManager inputManager;
    private RopeRenderer ropeRenderer;
    private PlayerHandIK rightHandIK;
    private Rigidbody rb;
    private SpringJoint spring;
    private LineRenderer lr;

    private Vector3 swingPoint;
    private bool currentlySwinging;
    private bool canSwing = true;
    private float swingTime;
    //private Vector3 currentGrapplePosition = Vector3.zero;

    private void Start()
    {
        playerLocomotion = GetComponent<PlayerLocomotion>();
        inputManager = GetComponent<InputManager>();
        ropeRenderer = GetComponent<RopeRenderer>();
        rightHandIK = GetComponent<PlayerHandIK>();
        rb = GetComponent<Rigidbody>();
        lr = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        if (inputManager.leftMouse && !playerLocomotion.isAiming) FindSwingPoint();
        else EndSwing();
    }

    private void LateUpdate()
    {
        if (!currentlySwinging) return;
        
        ropeRenderer.StartDrawingRope(swingPoint, swingPointRef);
        rightHandIK.StartHandIK(swingPoint);
    }

    private void FindSwingPoint()
    {
        if (currentlySwinging || !canSwing) return;

        float coneRadius = 5f;
        Vector3 forwardPos = transform.position + transform.forward * swingingPointForwardOffset;

        RaycastHit[] hits = Physics.SphereCastAll(forwardPos, coneRadius, transform.up, maxSwingDistance,
            swingableLayer);

        PerformSphereCast(hits);
    }

    private void PerformSphereCast(RaycastHit[] hits)
    {
        float maxScore = 0f;
        float minDistance = maxSwingDistance;

        foreach (var hit in hits)
        {
            // Calculate the angle between surface normal and direction from the player's position to the hit point
            Vector3 directionToHit = hit.point - transform.position;
            float angle = Vector3.Angle(hit.normal, directionToHit);

            // Calculate the absolute score based on the cosine of the angle
            float score = Mathf.Abs(Mathf.Cos(angle * Mathf.Deg2Rad));

            float distanceToPoint = Vector3.Distance(transform.position, hit.point);

            // If this score is better than the previous AND is closer, select it as swingPoint
            if (distanceToPoint < minDistance)
            {
                if (score >= maxScore)
                {
                    maxScore = score;
                    swingPoint = hit.point;
                }
                
                minDistance = distanceToPoint;
            }
        }

        if (swingPoint != Vector3.zero)
        {
            currentlySwinging = true;
            PlayerManager.UpdateState(States.Swinging);
            StartSwing();
        }
        else
        {
            currentlySwinging = false;
            return;

            // Potential problem: newState flickering between Grounded and Aerial
            var newState = PlayerManager.PreviousState;
            PlayerManager.UpdateState(newState);
        }
    }

    private void StartSwing()
    {
        if (spring) return;

        RumbleManager.Instance.StartRumble(playerLocomotion.lowRumbleFrequency, playerLocomotion.highRumbleFrequency,
            0f, true);
        
        spring = gameObject.AddComponent<SpringJoint>();
        lr.positionCount = 2;

        ApplySpringJointValues();
        rb.AddForce(transform.forward * initialForwardMomentum, ForceMode.Impulse);
        rb.AddForce(transform.up * initialUpwardMomentum, ForceMode.Impulse);

        playerLocomotion.inAirTimer = 0f;
        swingTime = 0f;
        StartCoroutine(UpdateRumbleIntensity());
    }

    private void ApplySpringJointValues()
    {
        spring.autoConfigureConnectedAnchor = false;
        spring.anchor = swingPointRef.localPosition;
        spring.connectedAnchor = swingPoint;

        float distanceFromPoint = Vector3.Distance(transform.position, swingPoint);

        spring.minDistance = distanceFromPoint * minDistanceMultiplier;
        spring.maxDistance = distanceFromPoint * maxDistanceMultiplier;

        spring.spring = swingForce;
        spring.damper = springDamper;
        spring.massScale = springMassScale;
    }

    private void EndSwing()
    {
        if (spring)
        {
            rightHandIK.StopHandIK();
            float swingSpeed = rb.velocity.magnitude;
            float dynamicThrust = Mathf.Clamp(swingSpeed / 10f, 1f, 3f);
            Vector3 swingDirection = rb.velocity.normalized;
            
            rb.AddForce(swingDirection * (swingEndThrust * dynamicThrust), ForceMode.Impulse);
            
            RumbleManager.Instance.StopRumble();
            StopCoroutine(UpdateRumbleIntensity());

            swingPoint = Vector3.zero;
            Destroy(spring);
        }

        if (PlayerManager.State == States.Grounded || PlayerManager.State == States.Aerial) return;

        currentlySwinging = false;
        lr.positionCount = 0;

        StartCoroutine(SwingCooldown());
        PlayerManager.UpdateState(States.Aerial);
    }

    public IEnumerator SwingCooldown()
    {
        canSwing = false;
        yield return new WaitForSeconds(swingingCD);
        canSwing = true;
    }

    private IEnumerator UpdateRumbleIntensity()
    {
        while (currentlySwinging)
        {
            // Increase frequencies over 10 seconds
            float lowFreq = Mathf.Lerp(playerLocomotion.lowRumbleFrequency, playerLocomotion.highRumbleFrequency,
                swingTime / 10f);

            float highFreq = Mathf.Lerp(playerLocomotion.lowRumbleFrequency, playerLocomotion.highRumbleFrequency,
                swingTime / 10f);
            
            RumbleManager.Instance.UpdateRumble(lowFreq, highFreq);
            yield return null;
        }
    }
}