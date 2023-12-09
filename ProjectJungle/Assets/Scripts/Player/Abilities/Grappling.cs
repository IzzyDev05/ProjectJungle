using UnityEngine;
using FMOD.Studio;

public class Grappling : MonoBehaviour
{
    [SerializeField] private float grappleSpeed = 15f;
    [SerializeField] private float grappleDistance = 35f;
    [SerializeField] private LayerMask grappleLayer;
    [SerializeField] private Transform freeLookCam;
    [SerializeField] private Transform aimCam;

    private PlayerInputManager inputManager;
    private PlayerLocomotion playerLocomotion;
    private Rigidbody playerRB;
    private Vector3 grappleTarget;
    private LineRenderer lr;
    private Transform mainCam;

    private EventInstance grapplingRetractingSound;

    private void Start()
    {
        inputManager = GetComponentInParent<PlayerInputManager>();
        playerLocomotion = GetComponentInParent<PlayerLocomotion>();
        playerRB = GetComponentInParent<Rigidbody>();
        lr = GetComponent<LineRenderer>();
        mainCam = Camera.main.transform;

        grapplingRetractingSound = AudioManager.instance.CreateEventInstance(FModEvents.instance.grappleRetract, this.transform.parent);
    }

    private void Update()
    {
        if (inputManager.AimInput) {
            freeLookCam.gameObject.SetActive(false);
            aimCam.gameObject.SetActive(true);
            playerLocomotion.shouldMove = false;
        }
        else {
            freeLookCam.gameObject.SetActive(true);
            aimCam.gameObject.SetActive(false);
            playerLocomotion.shouldMove = true;
        }

        if (inputManager.AimInput && inputManager.SwingInput) InitializeGrapple();
        else EndGrapple();
    }

    private void FixedUpdate()
    {
        if (playerLocomotion.IsGrappling) MoveTowardsGrappleTarget();
    }

    private void LateUpdate()
    {
        DrawRope();
    }

    private void InitializeGrapple()
    {
        if (playerLocomotion.IsGrappling) return;

        //Vector3 camAim = new Vector3(aimCam.forward.x + aimCam.rotation.x, aimCam.forward.y, aimCam.forward.z);
        Vector3 camAim = new Vector3(mainCam.forward.x, mainCam.forward.y, mainCam.forward.z);

        Ray ray = new Ray(aimCam.position, camAim);
        bool didRayHitSomething = Physics.Raycast(ray, out RaycastHit hit, grappleDistance, grappleLayer);

        if (didRayHitSomething) {
            grappleTarget = hit.point;
            lr.positionCount = 2;
            playerLocomotion.IsGrappling = true;

            AudioManager.instance.PlayOneShot(FModEvents.instance.grappleHit, grappleTarget);
        }
    }

    private void MoveTowardsGrappleTarget()
    {
        Vector3 grappleDirection = (grappleTarget - transform.position).normalized;
        playerRB.velocity = grappleDirection * grappleSpeed;

        PLAYBACK_STATE grapplingRetractingPlaybackState;
        grapplingRetractingSound.getPlaybackState(out grapplingRetractingPlaybackState);
        if (grapplingRetractingPlaybackState.Equals(PLAYBACK_STATE.PLAYING) == false)
        {
            grapplingRetractingSound.start();
        }

        float distanceToTarget = Vector3.Distance(transform.position, grappleTarget);
        if (distanceToTarget < 1f) {
            AudioManager.instance.PlayOneShot(FModEvents.instance.grappleRelease, grappleTarget);
            EndGrapple();
        }
    }

    private void EndGrapple()
    {
        playerLocomotion.IsGrappling = false;
        lr.positionCount = 0;
        grappleTarget = Vector3.zero;

        PLAYBACK_STATE grapplingRetractingPlaybackState;
        grapplingRetractingSound.getPlaybackState(out grapplingRetractingPlaybackState);
        if (grapplingRetractingPlaybackState.Equals(PLAYBACK_STATE.PLAYING))
        {
            grapplingRetractingSound.stop(STOP_MODE.IMMEDIATE);
            AudioManager.instance.PlayOneShot(FModEvents.instance.grappleRelease, this.transform.parent.position);
        }
    }

    private Vector3 currentGrapplePosition = Vector3.zero;
    private void DrawRope()
    {
        if (grappleTarget == Vector3.zero) return;

        currentGrapplePosition = Vector3.Lerp(currentGrapplePosition, grappleTarget, Time.deltaTime * 8f);
        lr.SetPosition(0, playerRB.position);
        lr.SetPosition(1, grappleTarget);
    }

    /*
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        var testPos = new Vector3(mainCam.forward.x, mainCam.forward.y, mainCam.forward.z);
        Gizmos.DrawRay(aimCam.position, testPos * 40f);
    }
    */
}