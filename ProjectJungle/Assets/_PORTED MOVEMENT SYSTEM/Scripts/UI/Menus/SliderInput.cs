using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Cinemachine;

public class SliderInput : MonoBehaviour
{
    enum TYPE
    {
        MASTER,
        AMBIENT,
        SFX,
        AIM_HORIZONTAL_SENS,
        AIM_VERTICAL_SENS,
        LOOK_HORIZONTAL_SENS,
        LOOK_VERTICAL_SENS
    }

    #region General
    [Header("Type")]
    [SerializeField] TYPE type;

    [SerializeField] Slider slider;
    [SerializeField] TMP_InputField inputField;
    [SerializeField] float sliderCap;
    #endregion

    #region Freelook
    [Header("Freelook")]
    [SerializeField] GameObject freeLookCam;
    [SerializeField] float H_freeLookBaseSpeed;
    [SerializeField] float V_freeLookBaseSpeed;
    #endregion

    #region Aim
    [Header("Aim")]
    [SerializeField] GameObject aimCam;
    [SerializeField] float H_AimBaseSpeed;
    [SerializeField] float V_AimBaseSpeed;
    #endregion

    private void Awake()
    {
        slider = this.GetComponentInChildren<Slider>();
        inputField = this.GetComponentInChildren<TMP_InputField>();

        if (sliderCap <= 0)
        {
            sliderCap = 10f;
        }

        slider.maxValue = sliderCap;

        foreach (Transform camera in GameObject.Find("Cameras").transform)
        {
            if (camera.name.Contains("FreeLook"))
            {
                freeLookCam = camera.gameObject;
                H_freeLookBaseSpeed = freeLookCam.GetComponent<CinemachineFreeLook>().m_XAxis.m_MaxSpeed;
                V_freeLookBaseSpeed = freeLookCam.GetComponent<CinemachineFreeLook>().m_YAxis.m_MaxSpeed;

            }

            if (camera.name.Contains("Aim"))
            {
                aimCam = camera.gameObject;
                H_AimBaseSpeed = aimCam.GetComponent<CinemachineFreeLook>().m_XAxis.m_MaxSpeed;
                V_AimBaseSpeed = aimCam.GetComponent<CinemachineFreeLook>().m_YAxis.m_MaxSpeed;
            }
        }

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
                    slider.value = AudioManager.Instance.masterVolume * sliderCap;
                    break;
                }
            case TYPE.AMBIENT:
                {
                    slider.value = AudioManager.Instance.ambientVolume * sliderCap;
                    break;
                }
            case TYPE.SFX:
                {
                    slider.value = AudioManager.Instance.sfxVolume * sliderCap;
                    break;
                }
            case TYPE.LOOK_HORIZONTAL_SENS:
                {
                    slider.value = freeLookCam.GetComponent<CinemachineFreeLook>().m_XAxis.m_MaxSpeed / FreeLookHorizontal;
                    break;
                }
            case TYPE.LOOK_VERTICAL_SENS:
                {
                    slider.value = freeLookCam.GetComponent<CinemachineFreeLook>().m_YAxis.m_MaxSpeed / FreeLookVertical;
                    break;
                }
            case TYPE.AIM_HORIZONTAL_SENS:
                {
                    slider.value = aimCam.GetComponent<CinemachineFreeLook>().m_XAxis.m_MaxSpeed / AimHorizontal;
                    break;
                }
            case TYPE.AIM_VERTICAL_SENS:
                {
                    slider.value = aimCam.GetComponent<CinemachineFreeLook>().m_YAxis.m_MaxSpeed / AimVertical;
                    break;
                }
        }
    }

    /// <summary>
    /// Update the value of the type when the slider changes. The value of the type should be between 0 and 1.
    /// </summary>
    public void OnSliderValueChanged()
    {
        // Adjust the value of the slider to be between 0 and 1.
        float adjustedValue = slider.value / sliderCap;

        switch (type)
        {
            case TYPE.MASTER:
                {
                    AudioManager.Instance.masterVolume = adjustedValue;
                    break;
                }
            case TYPE.AMBIENT:
                {
                    AudioManager.Instance.ambientVolume = adjustedValue;
                    break;
                }
            case TYPE.SFX:
                {
                    AudioManager.Instance.sfxVolume = adjustedValue;
                    break;
                }
            case TYPE.LOOK_HORIZONTAL_SENS:
                {
                    freeLookCam.GetComponent<CinemachineFreeLook>().m_XAxis.m_MaxSpeed = slider.value * FreeLookHorizontal;
                    break;
                }
            case TYPE.LOOK_VERTICAL_SENS:
                {
                    freeLookCam.GetComponent<CinemachineFreeLook>().m_YAxis.m_MaxSpeed = slider.value * FreeLookVertical;
                    break;
                }
            case TYPE.AIM_HORIZONTAL_SENS:
                {
                    aimCam.GetComponent<CinemachineFreeLook>().m_XAxis.m_MaxSpeed = slider.value * AimHorizontal;
                    break;
                }
            case TYPE.AIM_VERTICAL_SENS:
                {
                    aimCam.GetComponent<CinemachineFreeLook>().m_YAxis.m_MaxSpeed = slider.value * AimVertical;
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
        float parsedValue = float.Parse(inputField.text) / sliderCap;
        
        if (parsedValue < 0)
        {
            parsedValue *= -1;
        }

        switch (type) 
        {
            case TYPE.MASTER:
                {
                    AudioManager.Instance.masterVolume = parsedValue;
                    break;
                }
            case TYPE.AMBIENT:
                {
                    AudioManager.Instance.ambientVolume = parsedValue;
                    break;
                }
            case TYPE.SFX:
                {
                    AudioManager.Instance.sfxVolume = parsedValue;
                    break;
                }
            // For some reason this is not needed
            /*case TYPE.LOOK_HORIZONTAL_SENS:
                {
                    freeLookCam.GetComponent<CinemachineFreeLook>().m_XAxis.m_MaxSpeed = parsedValue * FreeLookHorizontal;
                    break;
                }
            case TYPE.LOOK_VERTICAL_SENS:
                {
                    freeLookCam.GetComponent<CinemachineFreeLook>().m_XAxis.m_MaxSpeed = parsedValue * FreeLookVertical;
                    break;
                }
            case TYPE.AIM_HORIZONTAL_SENS:
                {
                    aimCam.GetComponent<CinemachineFreeLook>().m_XAxis.m_MaxSpeed = parsedValue * AimHorizontal;
                    break;
                }
            case TYPE.AIM_VERTICAL_SENS:
                {
                    aimCam.GetComponent<CinemachineFreeLook>().m_YAxis.m_MaxSpeed = parsedValue * AimVertical;
                    break;
                }*/
        }

        MatchSliderToInputField(inputField.text);
    }

    #region VALUE_MATCHERS
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

        if (type == TYPE.LOOK_HORIZONTAL_SENS || type == TYPE.LOOK_VERTICAL_SENS || type == TYPE.AIM_HORIZONTAL_SENS || type == TYPE.AIM_VERTICAL_SENS)
        {
            inputField.text = sliderValue.ToString("0.##");
        }
        else
        {
            inputField.text = string.Format("{0:F0}", Mathf.Abs(sliderValue));
        }
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
            if (type == TYPE.LOOK_HORIZONTAL_SENS || type == TYPE.LOOK_VERTICAL_SENS || type == TYPE.AIM_HORIZONTAL_SENS || type == TYPE.AIM_VERTICAL_SENS)
            {
                slider.value = float.Parse(inputFieldValue);
            }
            else
            {
                parsedValue = Mathf.Abs(float.Parse(inputFieldValue));
                slider.value = parsedValue;
            }    
        }
    }
    #endregion

    #region HELPERS
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
    #endregion

    #region Getters
    float FreeLookHorizontal { get { return H_freeLookBaseSpeed; } }
    float FreeLookVertical { get { return V_freeLookBaseSpeed; } }

    float AimHorizontal { get { return H_AimBaseSpeed; } }
    float AimVertical { get { return V_AimBaseSpeed;} }
    #endregion
}
