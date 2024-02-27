using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private PlayerInputManager inputManager;
    private PlayerLocomotion playerLocomotion;

    private void Start() {
        inputManager = GetComponent<PlayerInputManager>();
        playerLocomotion = GetComponent<PlayerLocomotion>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update() {
        inputManager.HandleAllInputs();
    }

    private void FixedUpdate() {
        playerLocomotion.HandleAllMovement();
    }
}