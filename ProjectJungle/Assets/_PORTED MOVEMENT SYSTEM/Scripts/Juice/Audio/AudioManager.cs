using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

/*
 * Script made following Shaped by Rain Studios video: How to make an Audio System in Unity | Unity + FMOD Tutorial -> https://www.youtube.com/watch?v=rcBHIOjZDpk
 */

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    #region VOLUME_CONTROL
    [Header("Volume")]
    [Range(0, 1)]

    public float masterVolume = 1f;
    [Range(0, 1)]

    public float ambientVolume = 1f;
    [Range(0, 1)]

    public float sfxVolume = 1f;
    [Range(0, 1)]
    #endregion

    #region AUDIO_BUS
    private Bus masterBus;
    private Bus ambientBus;
    private Bus sfxBus;
    #endregion


    EventInstance ambienceEventInstance;
    EventInstance levelTransitionEventInstance;

    List<EventInstance> eventInstances;

    private void Awake()
    {
        #region SINGLETON
        if (Instance != null)
        {
            Debug.LogError("More than one Audio Manager in the Scene");
        }

        Instance = this;
        #endregion

        eventInstances = new List<EventInstance>();

        #region GET_AUDIO_BUS
        masterBus = RuntimeManager.GetBus("bus:/");
        ambientBus = RuntimeManager.GetBus("bus:/Ambience");
        sfxBus = RuntimeManager.GetBus("bus:/SFX");
        #endregion
    }

    private void Start()
    {
        InitializeAmbience(FModEvents.Instance.forest);

        levelTransitionEventInstance = CreateEventInstance(FModEvents.Instance.levelTransition, Camera.main.transform);
    }

    private void Update()
    {
        masterBus.setVolume(masterVolume);
        ambientBus.setVolume(ambientVolume);
        sfxBus.setVolume(sfxVolume);

        foreach (EventInstance eventInstance in eventInstances)
        {
            Update3DAttributes(eventInstance, Camera.main.transform);
        }
    }

    /// <summary>
    /// Plays an audio clip once.
    /// </summary>
    /// <param name="sound">The audio clip you want to play. Gotten from the FModEvent script.</param>
    /// <param name="worldPos">Position where you want the sound to be played from.</param>
    public void PlayOneShot(EventReference sound, Vector3 worldPos)
    {
        RuntimeManager.PlayOneShot(sound, worldPos);
    }

    /// <summary>
    /// Create a audio clip that can be played and stopped. Usually in the form of a looped audio clip.
    /// </summary>
    /// <param name="eventReference">The audio clip you want to play. Gotten from the FModEvent script.</param>
    /// <param name="origin">The transform where the sound will be located at.</param>
    /// <returns></returns>
    public EventInstance CreateEventInstance(EventReference eventReference, Transform origin = null)
    {
        EventInstance eventInstance = RuntimeManager.CreateInstance(eventReference);
        Update3DAttributes(eventInstance, origin);

        eventInstances.Add(eventInstance);

        return eventInstance;
    }

    /// <summary>
    /// Updates the position where hte audio clip will be heard from in the case where it moves.
    /// </summary>
    /// <param name="eventInstance">The audio clip you want to play. Gotten from the FModEvent script.</param>
    /// <param name="origin">The transform where the sound will be located at.</param>
    private void Update3DAttributes(EventInstance eventInstance, Transform origin)
    {
        FMOD.ATTRIBUTES_3D attributes = new FMOD.ATTRIBUTES_3D();
        attributes.position = new FMOD.VECTOR();
        attributes.position.x = origin.position.x;
        attributes.position.y = origin.position.y;
        attributes.position.z = origin.position.z;
        attributes.forward = new FMOD.VECTOR();
        attributes.forward.x = origin.forward.x;
        attributes.forward.y = origin.forward.y;
        attributes.forward.z = origin.forward.z;
        attributes.up = new FMOD.VECTOR();
        attributes.up.x = origin.up.x;
        attributes.up.y = origin.up.y;
        attributes.up.z = origin.up.z;

        eventInstance.set3DAttributes(attributes);
    }

    /// <summary>
    /// Initialize and play the ambient sound.
    /// </summary>
    /// <param name="ambienceEventReference">The ambient sound clip you want to play. Gotten from the FModEvent script.</param>
    void InitializeAmbience(EventReference ambienceEventReference)
    {
        ambienceEventInstance = CreateEventInstance(ambienceEventReference, Camera.main.transform);

        ambienceEventInstance.start();
    }

    /// <summary>
    /// Pauses and unpauses the ambient sound.
    /// </summary>
    /// <param name="pauseAmbientSound">True if you want to pause the sound (Default). False if you want to resume playing the sound.</param>
    public void PauseAmbience(bool pauseAmbientSound = true)
    {
        bool isAmbientSoundPaused;

        ambienceEventInstance.getPaused(out isAmbientSoundPaused);

        if (pauseAmbientSound && isAmbientSoundPaused == false)
        {
            ambienceEventInstance.setPaused(true);

        }
        else if (pauseAmbientSound == false && isAmbientSoundPaused)
        {
            ambienceEventInstance.setPaused(false);

        }
    }

    /// <summary>
    /// Plays the SFX for level transition.
    /// </summary>
    /// <param name="pause">A boolean to stop the SFX early.</param>
    public void PlayLevelChangeSound(bool pause = false)
    {
        if (pause)
        {
            levelTransitionEventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        }
        else
        {
            levelTransitionEventInstance.start();
        }
    }

    /// <summary>
    /// Cleans up all FMod instances.
    /// </summary>
    void CleanUp()
    {
        foreach (EventInstance eventInstance in eventInstances)
        {
            eventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            eventInstance.release();
        }
    }

    private void OnDestroy()
    {
        CleanUp();
    }
}
