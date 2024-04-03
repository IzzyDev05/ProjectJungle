using System;
using UnityEngine;

public class RopeRenderer : MonoBehaviour
{
    [SerializeField] private Transform endPoint;
    [SerializeField] private LineRenderer rope;
    [SerializeField] private GameObject ball; // The rope shoots from here
    [SerializeField] private float transitionDuration = 1.0f;

    private Vector3 targetPosition;
    private Vector3 startPosition;
    private float startTime;
    private bool isTransitioning = false;

    private void Start()
    {
        rope.enabled = false;
        ball.SetActive(false);

        startPosition = ball.transform.position;
        endPoint.position = ball.transform.position;
    }

    private void Update()
    {
        if (isTransitioning)
        {
            float timeSinceStarted = Time.time - startTime;
            float percentageComplete = timeSinceStarted / transitionDuration;

            endPoint.position = Vector3.Lerp(startPosition, targetPosition, percentageComplete);

            if (percentageComplete >= 1.0f) isTransitioning = false;
        }
        
        if (!rope.enabled) endPoint.position = ball.transform.position;
    }

    public void StartDrawingRope(Vector3 swingPoint)
    {
        ball.SetActive(true);
        endPoint.position = ball.transform.position;
        rope.enabled = true;

        StartTransition(swingPoint);
    }

    public void StopDrawingRope()
    {
        rope.enabled = false;
        endPoint.position = ball.transform.position;
        ball.SetActive(false);
    }

    private void StartTransition(Vector3 newTargetPosition)
    {
        startPosition = endPoint.position; // Current position
        targetPosition = newTargetPosition; // New target position
        startTime = Time.time;
        isTransitioning = true;
    }
}