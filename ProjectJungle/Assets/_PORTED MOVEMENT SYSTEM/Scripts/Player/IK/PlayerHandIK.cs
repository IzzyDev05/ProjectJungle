using System;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlayerHandIK : MonoBehaviour
{
    [SerializeField] private TwoBoneIKConstraint rightHandRig;
    [SerializeField] private Transform rightHandTarget;
    [SerializeField] private float animationTransitionSpeed = 0.1f;
    [SerializeField] private float maxWeight = 1f;

    private float targetWeight = 0f;

    private void Start()
    {
        rightHandRig.weight = 0f;
    }

    private void Update()
    {
        rightHandRig.weight = Mathf.MoveTowards(rightHandRig.weight, targetWeight, animationTransitionSpeed * Time.deltaTime);
    }

    public void StartHandIK(bool point, Vector3 rightTargetPoint)
    {
        if (!point) return;
        
        rightHandTarget.position = rightTargetPoint;
        targetWeight = maxWeight;
    }

    public void StopHandIK()
    {
        targetWeight = 0f;
    }
}