using UnityEngine;

public class AimTransition : MonoBehaviour
{
    [SerializeField] private GameObject baseCam;
    [SerializeField] private GameObject aimCam;
    [SerializeField] private GameObject aimReticle;

    private PlayerInputManager inputManager;
    private bool isAiming;

    private void Start() {
        inputManager = GetComponent<PlayerInputManager>();

        baseCam.SetActive(true);
        aimCam.SetActive(false);
        aimReticle.SetActive(false);
    }

    private void Update() {
        isAiming = inputManager.AimInput;

        if (!isAiming) {
            baseCam.SetActive(true);
            aimCam.SetActive(false);
            aimReticle.SetActive(false);
        }
        else {
            baseCam.SetActive(false);
            aimCam.SetActive(true);
            aimReticle.SetActive(true);
        }
    }
}