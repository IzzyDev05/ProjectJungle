using UnityEngine;
using FMODUnity;

/*
 * Script made following Shaped by Rain Studios video: How to make an Audio System in Unity | Unity + FMOD Tutorial -> https://www.youtube.com/watch?v=rcBHIOjZDpk
 */

public class FModEvents : MonoBehaviour
{
    public static FModEvents Instance { get; private set; }

    #region INVENTORY SFX
    [field: Header("Backpack SFX")]
    [field: SerializeField] public EventReference backpack { get; private set; }

    [field: Header("Pickup SFX")]
    [field: SerializeField] public EventReference pickupItem { get; private set; }
    #endregion

    #region PLAYER SFX
    [field: Header("Footstep SFX")]
    [field: SerializeField] public EventReference footsteps { get; private set; }

    [field: Header("Jumping SFX")]
    [field: SerializeField] public EventReference jumpingSound { get; private set; }

    [field: Header("Landing SFX")]
    [field: SerializeField] public EventReference landingSound { get; private set; }

    [field: Header("Diving SFX")]
    [field: SerializeField] public EventReference divingSound { get; private set; }
    #endregion

    #region ABILITIES SFX
    [field: Header("Grapple Hit SFX")]
    [field: SerializeField] public EventReference grappleHit { get; private set; }

    [field: Header("Grapple Release SFX")]
    [field: SerializeField] public EventReference grappleRelease { get; private set; }

    [field: Header("Grapple Retract SFX")]
    [field: SerializeField] public EventReference grappleRetract { get; private set; }
    #endregion

    #region UI SFX

    #endregion

    #region OTHER SOUNDS
    [field: Header("Level Transition")]
    [field: SerializeField] public EventReference levelTransition { get; private set; }


    [field: Header("Ambience")]
    [field: SerializeField] public EventReference forest { get; private set; }
    #endregion

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("More than one FMod Events instance in the Scene");
        }

        Instance = this;
    }

}
