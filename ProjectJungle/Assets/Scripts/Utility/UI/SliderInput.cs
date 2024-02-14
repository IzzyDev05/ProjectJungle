using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SliderInput : MonoBehaviour
{
    enum TYPE
    {
        MASTER,
        AMBIENT,
        SFX
    }

    [Header("Type")]
    [SerializeField] TYPE type;

    [SerializeField] Slider slider;
    [SerializeField] TMP_InputField inputField;

    private void Awake()
    {
        slider = this.GetComponentInChildren<Slider>();
        inputField = this.GetComponentInChildren<TMP_InputField>();

        SetUpSliderInput();
    }

    private void Update()
    {
        UpdateSliderValue();
    }

    /// <summary>
    /// Updated the value of the slider to match the corresponding type.
    /// </summary>
    void UpdateSliderValue()
    {
        switch (type)
        {
            case TYPE.MASTER:
                {
                    slider.value = AudioManager.instance.masterVolume * 100;
                    break;
                }
            case TYPE.AMBIENT:
                {
                    slider.value = AudioManager.instance.ambientVolume * 100;
                    break;
                }
            case TYPE.SFX:
                {
                    slider.value = AudioManager.instance.sfxVolume * 100;
                    break;
                }
        }
    }

    /// <summary>
    /// Update the value of the type when the slider changes. The value of the type should be between 0 and 1.
    /// </summary>
    public void OnSliderValueChanged()
    {
        // Ajust the value of the slider to be between 0 and 1.
        float adjustedValue = slider.value / 100f;
        switch (type)
        {
            case TYPE.MASTER:
                {
                    AudioManager.instance.masterVolume = adjustedValue;
                    break;
                }
            case TYPE.AMBIENT:
                {
                    AudioManager.instance.ambientVolume = adjustedValue;
                    break;
                }
            case TYPE.SFX:
                {
                    AudioManager.instance.sfxVolume = adjustedValue;
                    break;
                }
        }

        MatchInputFieldToSlider(slider.value);
    }

    /// <summary>
    /// Updates the value of the type when the input field changes. The value of the type should be between 0 and 1.
    /// </summary>
    public void OnInputFieldValueChanged()
    {
        // Exit if the string cannot be converted into a float.
        if (isParsible(inputField.text) == false)
        {
            return;
        }

        // Converted string and adjust to be between 0 and 1.
        float parsedVolumeValue = int.Parse(inputField.text) / 100f;
        
        if (parsedVolumeValue < 0)
        {
            parsedVolumeValue *= -1;
        }

        switch (type) 
        {
            case TYPE.MASTER:
                {
                    AudioManager.instance.masterVolume = parsedVolumeValue;
                    break;
                }
            case TYPE.AMBIENT:
                {
                    AudioManager.instance.ambientVolume = parsedVolumeValue;
                    break;
                }
            case TYPE.SFX:
                {
                    AudioManager.instance.sfxVolume = parsedVolumeValue;
                    break;
                }
        }

        MatchSliderToInputField(inputField.text);
    }

    // VALUE MATCHERS

    /// <summary>
    /// Set the input field value to match the slider value
    /// </summary>
    /// <param name="sliderValue">A integer that represents the slider's value</param>
    void MatchInputFieldToSlider(float sliderValue)
    {
        if (inputField.contentType != TMP_InputField.ContentType.IntegerNumber)
        {
            inputField.contentType = TMP_InputField.ContentType.IntegerNumber;

            return;
        }

        inputField.text = string.Format("{0:F0}", Mathf.Abs(sliderValue));

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

        slider.value = parsedValue;
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
        slider.onValueChanged.RemoveAllListeners();

        slider.onValueChanged.AddListener(delegate { OnSliderValueChanged(); });
        #endregion

        #region INPUT_FIELD
        inputField.onValueChanged.RemoveAllListeners();

        inputField.onValueChanged.AddListener(delegate { OnInputFieldValueChanged(); });
        #endregion
    }
}
