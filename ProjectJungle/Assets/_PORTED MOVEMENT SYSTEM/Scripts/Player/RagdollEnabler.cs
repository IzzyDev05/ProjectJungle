using UnityEngine;

public class RagdollEnabler : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Transform ragdollRoot;
    [SerializeField] private Collider playerColl;
    [SerializeField] private GameObject ragCam;
    
    [SerializeField] private bool startRagdoll = false;
    
    private Rigidbody[] rbs;
    private CharacterJoint[] joints;
    private Collider[] colliders;
    private PlayerLocomotion playerLocomotion;

    private void Start()
    {
        rbs = ragdollRoot.GetComponentsInChildren<Rigidbody>();
        joints = ragdollRoot.GetComponentsInChildren<CharacterJoint>();
        colliders = ragdollRoot.GetComponentsInChildren<Collider>();
        playerLocomotion = FindObjectOfType<PlayerLocomotion>();
        
        ragCam.SetActive(false);

        if (startRagdoll) EnableRagdoll();
        else DisableRagdoll();
    }

    public void ToggleRagdoll()
    {
        startRagdoll = !startRagdoll;
        
        if (startRagdoll) EnableRagdoll();
        else DisableRagdoll();
    }
    
    private void EnableRagdoll()
    {
        animator.enabled = false;
        playerColl.enabled = false;
        playerLocomotion.enabled = false;
        ragCam.SetActive(true);

        foreach (CharacterJoint joint in joints) joint.enableCollision = true;
        foreach (Collider coll in colliders) coll.enabled = true;

        foreach (Rigidbody rb in rbs)
        {
            rb.detectCollisions = true;
            rb.useGravity = true;
        }
    }

    private void DisableRagdoll()
    {
        animator.enabled = true;
        playerColl.enabled = true;
        playerLocomotion.enabled = true;
        ragCam.SetActive(false);

        foreach (CharacterJoint joint in joints) joint.enableCollision = false;
        foreach (Collider coll in colliders) coll.enabled = false;
        
        foreach (Rigidbody rb in rbs)
        {
            rb.detectCollisions = false;
            rb.useGravity = false;
        }
    }
}