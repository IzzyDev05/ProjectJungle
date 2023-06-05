using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private float maxSpeed;
    private Animator animator;
    private Rigidbody rb;

    private string speedVar = "speed";
    private string jumpVar = "isJumping";

    private void Start() {
        animator = GetComponent<Animator>();
        rb = GetComponentInParent<Rigidbody>();

        maxSpeed = GetComponentInParent<PlayerController>().GetMaxSpeed();
    }

    private void Update() {
        Vector3 currentVelocity = rb.velocity;
        currentVelocity.y = 0f;
        animator.SetFloat(speedVar, currentVelocity.magnitude / maxSpeed);
    }

    public void SetJumpingTrue() {
        animator.SetBool(jumpVar, true);
    }

    // Animation event
    public void SetJumpingFalse() {
        animator.SetBool(jumpVar, false);
    }
}