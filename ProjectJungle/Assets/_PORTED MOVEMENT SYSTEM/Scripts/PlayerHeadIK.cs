using System;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.Serialization;

public class PlayerHeadIK : MonoBehaviour
{
    [SerializeField] private MultiAimConstraint headAimRig;
    [SerializeField] private Transform headTarget;
    [SerializeField] private float animationTransitionSpeed = 0.1f;

    private float targetWeight = 0f;

    private void Start()
    {
        headAimRig.weight = 0f;
    }

    private void Update()
    {
        headAimRig.weight = Mathf.MoveTowards(headAimRig.weight, targetWeight, animationTransitionSpeed * Time.deltaTime);
    }

    public void StartHeadIK(Vector3 rightTargetPoint)
    {
        headTarget.position = rightTargetPoint;
        targetWeight = 1f;
    }

    public void StopHeadIK()
    {
        targetWeight = 0f;
        headTarget.position = Vector3.zero;
    }
}