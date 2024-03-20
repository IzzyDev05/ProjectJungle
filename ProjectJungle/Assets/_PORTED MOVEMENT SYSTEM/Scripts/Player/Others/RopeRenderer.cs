using System;
using UnityEngine;

public class RopeRenderer : MonoBehaviour
{
    private Vector3 currentGrapplePosition = Vector3.zero;

    private PlayerLocomotion playerLocomotion;
    private LineRenderer lr;

    private void Start()
    {
        playerLocomotion = GetComponent<PlayerLocomotion>();
        lr = GetComponent<LineRenderer>();
    }

    public void StartDrawingRope(Vector3 swingPoint, Transform swingPointRef)
    {
        if (PlayerManager.State == States.Swinging || playerLocomotion.isGrappling) DrawRope(swingPoint, swingPointRef);
    }
    
    private void DrawRope(Vector3 swingPoint, Transform swingPointRef)
    {
        if (swingPoint == Vector3.zero) return;
        if (lr.positionCount == 0) lr.positionCount = 2;

        currentGrapplePosition = Vector3.Lerp(currentGrapplePosition, swingPoint, Time.deltaTime * 25f);
        lr.SetPosition(0, swingPointRef.position);
        lr.SetPosition(1, swingPoint);
        //lr.SetPosition(1, currentGrapplePosition);
    }
}