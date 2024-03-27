using System.Collections;
using UnityEngine;

public class RopeRenderer : MonoBehaviour
{
    [SerializeField] private Transform endPoint;
    [SerializeField] private LineRenderer rope;
    [SerializeField] private GameObject ball;
    [SerializeField] private float transitionDuration = 1.0f;

    private PlayerLocomotion playerLocomotion;

    private void Start()
    {
        playerLocomotion = GetComponent<PlayerLocomotion>();
        rope.enabled = false;
        ball.SetActive(false);
    }

    public void StartDrawingRope(Vector3 swingPoint)
    {
        if (PlayerManager.State == States.Swinging || playerLocomotion.isGrappling)
        {
            ball.SetActive(true);

            StartCoroutine(MoveEndPointOverTime(endPoint.position, swingPoint, transitionDuration));
            rope.enabled = true;
        }
        else
        {
            ball.SetActive(false);
            rope.enabled = false;
            endPoint.position = ball.transform.position;
        }
    }

    private IEnumerator MoveEndPointOverTime(Vector3 startPosition, Vector3 targetPosition, float duration)
    {
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            endPoint.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        endPoint.position = targetPosition;
    }
}