using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class AimCamMovement : MonoBehaviour
{
    [SerializeField] private Transform pivot;
    [SerializeField] private float rotationSpeed = 100f;
    
    private PlayerLocomotion playerLocomotion;

    private Vector2 rotation;
    private InputAction mouseDelta;

    private void Awake()
    {
        // Create an InputAction for mouse delta
        mouseDelta = new InputAction("MouseDelta", binding: "<Mouse>/delta");
        mouseDelta.Enable();
    }

    private void Start()
    {
        playerLocomotion = FindObjectOfType<PlayerLocomotion>();
    }

    private void Update()
    {
        if (!playerLocomotion.isAiming) return;
        
        rotation = mouseDelta.ReadValue<Vector2>();
        
        float horizontalRotation = rotation.x * rotationSpeed * Time.deltaTime;
        transform.RotateAround(pivot.position, Vector3.up, horizontalRotation);

        float verticalRotation = rotation.y * rotationSpeed * Time.deltaTime;
        transform.RotateAround(pivot.position, -Vector3.right, verticalRotation);
        
        var angles = pivot.localEulerAngles;
        angles.x = ClampAngle(angles.x, -90f, 90f);
        
        transform.rotation = quaternion.Euler(transform.rotation.x, transform.rotation.y, 0f);

        //pivot.localEulerAngles = angles;
    }
    
    private float ClampAngle(float angle, float from, float to)
    {
        if (angle < 0f) angle = 360 + angle;
        if (angle > 180f) return Mathf.Max(angle, 360+from);
        return Mathf.Min(angle, to);
    }

    private void OnDisable()
    {
        mouseDelta.Disable();
    }
}