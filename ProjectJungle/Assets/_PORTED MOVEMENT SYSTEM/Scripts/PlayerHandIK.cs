using UnityEngine;

public class PlayerHandIK : MonoBehaviour
{
    [SerializeField] private Transform rightHandPos;
    [SerializeField] private Transform leftHandPos;
    
    [SerializeField] private Transform rightHandTarget = null;
    [SerializeField] private Transform leftHandTarget = null;
    [SerializeField] private float animationTransitionSpeed = 0.1f;

    private bool isRightTargetPosNotNull;
    private bool isLeftTargetPosNotNull;
    
    public void StartHandIK(Vector3 rightTargetPoint, Vector3 leftTargetPoint)
    {
        rightHandTarget.position = rightTargetPoint;
        leftHandTarget.position = leftTargetPoint;
        
        isRightTargetPosNotNull = rightHandTarget != null;
        isLeftTargetPosNotNull = leftHandTarget != null;

        HandlePosition();
    }

    private void HandlePosition()
    {
        if (isRightTargetPosNotNull)
        {
            rightHandPos.position = Vector3.Lerp(rightHandPos.position, rightHandTarget.position,
                animationTransitionSpeed);
            HandRotation(true);
        }

        if (isLeftTargetPosNotNull)
        {
            leftHandPos.position = Vector3.Lerp(leftHandPos.position, leftHandTarget.position,
                animationTransitionSpeed);
            HandRotation(false);
        }
    }

    private void HandRotation(bool isRightHand)
    {
        Transform handPos = isRightHand ? rightHandPos : leftHandPos;
        Transform targetPos = isRightHand ? rightHandTarget : leftHandTarget;

        Vector3 targetDir = (targetPos.position - handPos.position).normalized;
        float angle = Vector3.Angle(handPos.forward, targetDir);

        // Direction of rotation
        Vector3 cross = Vector3.Cross(handPos.forward, targetDir);
        if (Vector3.Dot(handPos.up, cross) > 0) angle = -angle;

        // Apply rotation
        Quaternion targetRot = Quaternion.Euler(0, angle, 0);
        //targetRot = Quaternion.LookRotation(targetDir); (Not sure which one works)
        handPos.rotation = Quaternion.Slerp(handPos.rotation, targetRot, animationTransitionSpeed);
    }
}