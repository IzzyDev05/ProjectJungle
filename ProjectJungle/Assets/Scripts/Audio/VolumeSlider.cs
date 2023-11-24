using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    enum VolumeType
    {
        MASTER,
        AMBIENT,
        SFX
    }

    [Header("Type")]
    [SerializeField] VolumeType volumeType;

    Slider volumeSlider;

    private void Awake()
    {
        volumeSlider = this.GetComponentInChildren<Slider>();
    }

    private void Update()
    {
        switch (volumeType)
        {
            case VolumeType.MASTER:
                {
                    volumeSlider.value = AudioManager.instance.masterVolume;
                    break;
                }
            case VolumeType.AMBIENT:
                {
                     volumeSlider.value = AudioManager.instance.ambientVolume;
                    break;
                }
            case VolumeType.SFX:
                {
                    volumeSlider.value = AudioManager.instance.sfxVolume;
                    break;
                }
        }

    }


    public void OnSliderValueChanged()
    {
        switch (volumeType)
        {
            case VolumeType.MASTER:
                {
                    AudioManager.instance.masterVolume = volumeSlider.value;
                    break;
                }
            case VolumeType.AMBIENT:
                {
                    AudioManager.instance.ambientVolume = volumeSlider.value;
                    break;
                }
            case VolumeType.SFX:
                {
                    AudioManager.instance.sfxVolume = volumeSlider.value;
                    break;
                }
        }
    }
}
