using System;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.Serialization;

public class PlayerBodyIK : MonoBehaviour
{
    [SerializeField] private MultiAimConstraint bodyAimRig;
    [SerializeField] private Transform bodyTarget;
    [SerializeField] private float animationTransitionSpeed = 0.1f;
    [SerializeField] private float maxWeight = 0.5f;

    private float targetWeight = 0f;

    private void Start()
    {
        bodyAimRig.weight = 0f;
    }

    private void Update()
    {
        bodyAimRig.weight = Mathf.MoveTowards(bodyAimRig.weight, targetWeight, animationTransitionSpeed * Time.deltaTime);
    }

    public void StartBodyIK(Vector3 rightTargetPoint)
    {
        bodyTarget.position = rightTargetPoint;
        targetWeight = maxWeight;
    }

    public void StopBodyIK()
    {
        targetWeight = 0f;
    }
}