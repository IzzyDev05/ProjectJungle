using UnityEngine;

[RequireComponent (typeof(PlayerLocomotion))]
[RequireComponent (typeof(PlayerInputManager))]
[RequireComponent (typeof(UIAndInteractionManager))]
public class PlayerManager : MonoBehaviour
{
    private PlayerInputManager inputManager;
    private UIAndInteractionManager UIInteractionManager;
    private PlayerLocomotion playerLocomotion;
    private Animator anim;

    public bool isInteracting;

    private void Start()
    {
        inputManager = GetComponent<PlayerInputManager>();
        playerLocomotion = GetComponent<PlayerLocomotion>();
        UIInteractionManager = GetComponent<UIAndInteractionManager>();
        anim = GetComponent<Animator>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        inputManager.HandleAllInputs();
        UIInteractionManager.HandleUIAndInteractionInputs();
    }

    private void FixedUpdate()
    {
        playerLocomotion.HandleAllMovement();
    }

    private void LateUpdate()
    {
        isInteracting = anim.GetBool("isInteracting");
        playerLocomotion.IsJumping = anim.GetBool("isJumping");
    }
}