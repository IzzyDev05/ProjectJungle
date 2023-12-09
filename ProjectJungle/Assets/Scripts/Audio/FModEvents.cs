using UnityEngine;
using FMODUnity;

public class FModEvents : MonoBehaviour
{
    public static FModEvents instance { get; private set; }

    #region INVENTORY SFX
    [field: Header("Backpack SFX")]
    [field: SerializeField] public EventReference backpack { get; private set; }

    [field: Header("Pickup SFX")]
    [field: SerializeField] public EventReference pickupItem { get; private set; }
    
    [field: Header("Drop Item SFX")]
    [field: SerializeField] public EventReference dropItem { get; private set; }
    #endregion

    #region PLAYER SFX
    [field: Header("Walking Footstep SFX")]
    [field: SerializeField] public EventReference walkingFootsteps { get; private set; }

    [field: Header("Sprinting Footstep SFX")]
    [field: SerializeField] public EventReference sprintingFootsteps { get; private set; }

    [field: Header("Jumping SFX")]
    [field: SerializeField] public EventReference jumpingSound { get; private set; }

    /*[field: Header("Landing SFX")]
    [field: SerializeField] public EventReference landingSound { get; private set; }*/
    #endregion

    #region ABILITIES SFX
    [field: Header("Grapple Hit SFX")]
    [field: SerializeField] public EventReference grappleHit { get; private set; }

    [field: Header("Grapple Release SFX")]
    [field: SerializeField] public EventReference grappleRelease { get; private set; }

    [field: Header("Grapple Retract SFX")]
    [field: SerializeField] public EventReference grappleRetract { get; private set; }
    #endregion

    [field: Header("Ambience SFX")]
    [field: SerializeField] public EventReference forest { get; private set; }
    
    [field: Header("Pause SFX")]
    [field: SerializeField] public EventReference pause { get; private set; }
    
    [field: Header("Resume SFX")]
    [field: SerializeField] public EventReference resume { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than one FMod Events instance in the Scene");
        }

        instance = this;
    }

}
