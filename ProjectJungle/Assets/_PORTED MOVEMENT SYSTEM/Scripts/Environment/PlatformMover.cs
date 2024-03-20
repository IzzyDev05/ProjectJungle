using System;
using UnityEngine;

public class PlatformMover : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float distance = 3f;
    [SerializeField] private Vector3 direction = Vector3.right;

    private Vector3 startPos;

    private void Start()
    {
        startPos = transform.position;
    }

    private void Update()
    {
        float movement = Mathf.Sin(Time.time * speed) * distance;
        transform.position = startPos + direction.normalized * movement;
    }
}