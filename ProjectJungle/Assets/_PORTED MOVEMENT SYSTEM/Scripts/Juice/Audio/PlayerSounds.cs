using FMOD.Studio;
using UnityEngine;

/*
 *  Script Created using the help of Omnisepher Game & Sound's video: FMOD Time L02 - Switches & Footsteps | FMOD | Unity -> https://www.youtube.com/watch?v=naSl0DbqACA 
 */

public class PlayerSounds : MonoBehaviour
{
    private PlayerLocomotion playerLoco;
    private InputManager inputManager;

    #region EVENT_INSTANCES
    private EventInstance footsteps;
    private EventInstance jumping;
    private EventInstance landing;
    private EventInstance diving;
    #endregion

    public TerrainType groundType { get; private set; }

    public void Start()
    {
        playerLoco = FindObjectOfType<PlayerLocomotion>();
        inputManager = FindObjectOfType<InputManager>();

        footsteps = AudioManager.Instance.CreateEventInstance(FModEvents.Instance.footsteps, GameManager.Player.transform);
        jumping = AudioManager.Instance.CreateEventInstance(FModEvents.Instance.jumpingSound, GameManager.Player.transform);
        landing = AudioManager.Instance.CreateEventInstance(FModEvents.Instance.landingSound, GameManager.Player.transform);
        diving = AudioManager.Instance.CreateEventInstance(FModEvents.Instance.divingSound, GameManager.Player.transform);
    }

    #region PUBLIC_SFX_PLAYERS
    /// <summary>
    /// Plays the footstep sounds as a Animation event
    /// </summary>
    public void PlayFootsteps()
    {
        if (footsteps.isValid() && playerLoco.isGrounded)
        {
            GroundTypeChecker();
            SpeedToIntensity();

            footsteps.start();
        }
    }

    /// <summary>
    /// Stops the footstep sounds.
    /// </summary>
    public void StopFootsteps()
    {
        PLAYBACK_STATE state;
        footsteps.getPlaybackState(out state);
        if (state.Equals(PLAYBACK_STATE.PLAYING))
        {
            footsteps.stop(STOP_MODE.IMMEDIATE);
        }
    }

    /// <summary>
    /// Plays the jumping sound.
    /// </summary>
    public void PlayJumping(bool doubleJump = false)
    {
        if (jumping.isValid())
        {
            if (doubleJump)
            {
                jumping.setParameterByName("Jumping", 1);
            }
            else
            {
                jumping.setParameterByName("Jumping", 0);
                //PlayFootsteps();
            }

            jumping.start();
        }
    }

    /// <summary>
    /// Plays the Landing SFX. The intensity of the SFX increases with the speed and distance from the ground the player is at.
    /// </summary>
    /// <param name="fallingVelocity">The speed in which the player falls at.</param>
    public void PlayLanding(float fallingVelocity)
    {
        if (playerLoco.isGroundSlamming)
        {
            landing.setParameterByName("FallingIntensity", 10f);
            landing.setParameterByName("GainBySpeed", 10f);
        }
        else
        {
            landing.setParameterByName("FallingIntensity", Mathf.Clamp(-10f + fallingVelocity, -10f, 5f));
        }

        if (landing.isValid())
        {
            GroundTypeChecker();
            landing.start();
            StopGroundSlamFalling();
        }
    }

    public void PlayGroundSlamFalling()
    {
        if (playerLoco.isGroundSlamming)
        {
            diving.start();
        }
    }

    public void StopGroundSlamFalling()
    {
        diving.stop(STOP_MODE.IMMEDIATE);
    }
    #endregion

    #region DYNAMIC_AUDIO_HELPERS
    /// <summary>
    /// Checks the type of ground to change the footstep and landing sounds
    /// </summary>
    private void GroundTypeChecker()
    {
        groundType = TerrainChecker.Instance.CheckTerrainType(this.transform.parent);
        switch (groundType)
        {
            case TerrainType.Wood:
                {
                    footsteps.setParameterByName("Footsteps", 0);
                    landing.setParameterByName("Landing", 0);
                    break;
                }
            case TerrainType.Grass:
                {
                    footsteps.setParameterByName("Footsteps", 1);
                    landing.setParameterByName("Landing", 1);
                    break;
                }
            case TerrainType.Dirt:
                {
                    footsteps.setParameterByName("Footsteps", 2);
                    landing.setParameterByName("Landing", 2);
                    break;
                }
            case TerrainType.Plantation:
                {
                    footsteps.setParameterByName("Footsteps", 3);
                    landing.setParameterByName("Landing", 3);
                    break;
                }
            default:
                {
                    // Wood Sounds
                    footsteps.setParameterByName("Footsteps", 0);
                    landing.setParameterByName("Landing", 0);
                    break;
                }
        }

    }

    /// <summary>
    /// Controls the intensity of the footsteps in relation to the speed of the movement
    /// </summary>
    private void SpeedToIntensity()
    {
        if (playerLoco.isSprinting)
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
            footsteps.setParameterByName("GainBySpeed", -10f);
        }
    }

    #endregion
}
