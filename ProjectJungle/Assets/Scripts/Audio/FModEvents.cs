using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class FModEvents : MonoBehaviour
{
    [field: Header("Pickup SFX")]
    [field: SerializeField] public EventReference pickupItem { get; private set; }

    [field: Header("Walking Footstep SFX")]
    [field: SerializeField] public EventReference walkingFootsteps { get; private set; }

    [field: Header("Sprinting Footstep SFX")]
    [field: SerializeField] public EventReference sprintingFootsteps { get; private set; }

    [field: Header("Ambience SFX")]
    [field: SerializeField] public EventReference forest { get; private set; }

    public static FModEvents instance { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than one FMod Events instance in the Scene");
        }

        instance = this;
    }

}
