using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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

    [SerializeField] Slider volumeSlider;
    [SerializeField] TMP_InputField volumeValueField;

    private void Awake()
    {
        volumeSlider = this.GetComponentInChildren<Slider>();
        volumeValueField = this.GetComponentInChildren<TMP_InputField>();

        SetUpSliderInput();
    }

    private void Update()
    {
        UpdateSliderValue();
    }

    /// <summary>
    /// Updated the value of the slider to match the volume level of the audio
    /// </summary>
    void UpdateSliderValue()
    {
        switch (volumeType)
        {
            case VolumeType.MASTER:
                {
                    volumeSlider.value = AudioManager.instance.masterVolume * 100;
                    break;
                }
            case VolumeType.AMBIENT:
                {
                    volumeSlider.value = AudioManager.instance.ambientVolume * 100;
                    break;
                }
            case VolumeType.SFX:
                {
                    volumeSlider.value = AudioManager.instance.sfxVolume * 100;
                    break;
                }
        }
    }

    /// <summary>
    /// Update the FMod volume when the slider changes
    /// </summary>
    public void OnSliderValueChanged()
    {
        // Ajust the value of the volume slider to be between 0 and 1.
        float adjustedVolumeValue = volumeSlider.value / 100f;
        switch (volumeType)
        {
            case VolumeType.MASTER:
                {
                    AudioManager.instance.masterVolume = adjustedVolumeValue;
                    break;
                }
            case VolumeType.AMBIENT:
                {
                    AudioManager.instance.ambientVolume = adjustedVolumeValue;
                    break;
                }
            case VolumeType.SFX:
                {
                    AudioManager.instance.sfxVolume = adjustedVolumeValue;
                    break;
                }
        }

        MatchInputFieldToSlider(volumeSlider.value);
    }

    /// <summary>
    /// Updates the FMod volume when the input field changes
    /// </summary>
    public void OnInputFieldValueChanged()
    {
        // Exit if the string cannot be converted into a float.
        if (isParsible(volumeValueField.text) == false)
        {
            return;
        }

        // Converted string and adjust to be between 0 and 1.
        float parsedVolumeValue = int.Parse(volumeValueField.text) / 100f;
        
        if (parsedVolumeValue < 0)
        {
            parsedVolumeValue *= -1;
        }

        switch (volumeType) 
        {
            case VolumeType.MASTER:
                {
                    AudioManager.instance.masterVolume = parsedVolumeValue;
                    break;
                }
            case VolumeType.AMBIENT:
                {
                    AudioManager.instance.ambientVolume = parsedVolumeValue;
                    break;
                }
            case VolumeType.SFX:
                {
                    AudioManager.instance.sfxVolume = parsedVolumeValue;
                    break;
                }
        }

        MatchSliderToInputField(volumeValueField.text);
    }

    // VALUE MATCHERS

    /// <summary>
    /// Set the input field value to match the slider value
    /// </summary>
    /// <param name="sliderValue">A integer that represents the slider's value</param>
    void MatchInputFieldToSlider(float sliderValue)
    {
        if (volumeValueField.contentType != TMP_InputField.ContentType.IntegerNumber)
        {
            volumeValueField.contentType = TMP_InputField.ContentType.IntegerNumber;

            return;
        }

        volumeValueField.text = string.Format("{0:F0}", Mathf.Abs(sliderValue));

    }

    /// <summary>
    /// Sets the slider to match the value of the input field
    /// </summary>
    /// <param name="inputFieldValue">A string that represents the input field's value</param>
    void MatchSliderToInputField(string inputFieldValue)
    {
        float parsedValue = 0;
        if (isParsible(inputFieldValue) == true)
        {
           parsedValue = Mathf.Abs(float.Parse(inputFieldValue));
        }

        volumeSlider.value = parsedValue;
    }


    //HELPERS

    /// <summary>
    /// Checks if the string can be converted into a float
    /// </summary>
    /// <param name="text">A string</param>
    /// <returns>true if the string can be converted into a float</returns>
    bool isParsible(string text)
    {
        if (float.TryParse(text, out float result))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// Set the listeners for the slider and input field
    /// </summary>
    void SetUpSliderInput()
    {
        #region SLIDER
        volumeSlider.onValueChanged.RemoveAllListeners();

        volumeSlider.onValueChanged.AddListener(delegate { OnSliderValueChanged(); });
        #endregion

        #region INPUT_FIELD
        volumeValueField.onValueChanged.RemoveAllListeners();

        volumeValueField.onValueChanged.AddListener(delegate { OnInputFieldValueChanged(); });
        #endregion
    }
}
