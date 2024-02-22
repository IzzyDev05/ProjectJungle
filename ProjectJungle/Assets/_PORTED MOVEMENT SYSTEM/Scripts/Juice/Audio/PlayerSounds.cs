using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;
using Unity.VisualScripting;

/*
 *  Script Created using the help of Omnisepher Game & Sound's video: FMOD Time L02 - Switches & Footsteps | FMOD | Unity -> https://www.youtube.com/watch?v=naSl0DbqACA 
 */

public class PlayerSounds : MonoBehaviour
{
    private EventInstance footsteps;
    private EventInstance jumping;
    private EventInstance landing;

    public void Start()
    {
        footsteps = AudioManager.Instance.CreateEventInstance(FModEvents.Instance.footsteps, GameManager.Player.transform);
        jumping = AudioManager.Instance.CreateEventInstance(FModEvents.Instance.jumpingSound, GameManager.Player.transform);
        landing = AudioManager.Instance.CreateEventInstance(FModEvents.Instance.landingSound, GameManager.Player.transform);
    }

    /// <summary>
    /// Plays the footstep sounds as a Animation event
    /// </summary>
    public void PlayFootsteps()
    {
        if (footsteps.isValid())
        {
            GroundTypeChecker();
            SpeedToIntensitiy();

            footsteps.start();
        }
    }

    public void PlayJumping()
    {
        if (jumping.isValid())
        {
            GroundTypeChecker();
            jumping.start();
        }
    }

    public void PlayLanding()
    {
        if (landing.isValid())
        {
            GroundTypeChecker();
            landing.start();
        }
    }

    /// <summary>
    /// Checks the type of ground to change the footstep sounds
    /// </summary>
    private void GroundTypeChecker()
    {
        RaycastHit hit;

        Ray ray = new Ray(GetComponentInParent<Transform>().position + Vector3.up * 0.5f, Vector3.down);

        if (Physics.Raycast(ray,out hit, 1.0f, Physics.AllLayers, QueryTriggerInteraction.Ignore)) 
        {
            Renderer groundRenderer = hit.collider.GetComponentInChildren<Renderer>();

            if (groundRenderer)
            {
                // Debug.Log(groundRenderer.material.name);
                if (groundRenderer.material.name.Contains("Grass"))
                {
                    footsteps.setParameterByName("Footsteps", 1);
                }
                else
                {
                    footsteps.setParameterByName("Footsteps", 0);
                }
            }
        }
    }

    /// <summary>
    /// Controls the intensity of the footsteps in relation to the speed of the movement
    /// </summary>
    private void SpeedToIntensitiy()
    {
        PlayerLocomotion playerLoco = GameManager.Player.GetComponentInChildren<PlayerLocomotion>();
        InputManager inputManager = GameManager.Player.GetComponentInChildren<InputManager>();

        if (playerLoco.IsSprinting)
        {
            //Debug.Log("Sprinting steps are louder");
            footsteps.setParameterByName("GainBySpeed", 5f);
        }
        else if (inputManager.moveAmount >= 0.5f)
        {
            //Debug.Log("Running steps are loud");
            footsteps.setParameterByName("GainBySpeed", 0f);
        }
        else
        {
            //Debug.Log("Walking steps are quite");
            footsteps.setParameterByName("GainBySpeed", -5f);
        }
    }
}
