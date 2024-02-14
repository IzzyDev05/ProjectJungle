using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

/*
 * Script made following Shaped by Rain Studios video: How to make an Audio System in Unity | Unity + FMOD Tutorial -> https://www.youtube.com/watch?v=rcBHIOjZDpk
 */

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance { get; private set; }

    [Header("Volume")]
    [Range(0,1)]

    public float masterVolume = 1f;
    [Range(0, 1)]

    public float ambientVolume = 1f;
    [Range(0, 1)]

    public float sfxVolume = 1f;
    [Range(0, 1)]

    private Bus masterBus;
    private Bus ambientBus;
    private Bus sfxBus;

    EventInstance ambienceEventInstance;

    List<EventInstance> eventInstances;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than one Audio Manager in the Scene");
        }

        instance = this;

        eventInstances = new List<EventInstance>();

        masterBus = RuntimeManager.GetBus("bus:/");
        ambientBus = RuntimeManager.GetBus("bus:/Ambience");
        sfxBus = RuntimeManager.GetBus("bus:/SFX");
    }

    private void Start()
    {
        InitializeAmbience(FModEvents.instance.forest);
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

    public void PlayOneShot(EventReference sound, Vector3 worldPos)
    {
        RuntimeManager.PlayOneShot(sound, worldPos);
    }

    public EventInstance CreateEventInstance(EventReference eventReference, Transform origin = null)
    {
        EventInstance eventInstance = RuntimeManager.CreateInstance(eventReference);
        Update3DAttributes(eventInstance, origin);

        eventInstances.Add(eventInstance);

        return eventInstance;
    }

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

    void InitializeAmbience(EventReference ambienceEventReference)
    {
        ambienceEventInstance = CreateEventInstance(ambienceEventReference, Camera.main.transform);

        ambienceEventInstance.start();
    }

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
