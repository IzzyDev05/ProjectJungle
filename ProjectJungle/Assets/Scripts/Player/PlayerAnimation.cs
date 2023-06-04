using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator animator;
    private Rigidbody rb;
    private float maxSpeed = 5f;

    private string speedVar = "speed";

    private void Start() {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    private void Update() {
        animator.SetFloat(speedVar, rb.velocity.magnitude / maxSpeed);
    }
}