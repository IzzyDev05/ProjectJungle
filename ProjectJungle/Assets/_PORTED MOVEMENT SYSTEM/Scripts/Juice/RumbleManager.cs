using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class RumbleManager : MonoBehaviour
{
    public static RumbleManager Instance;
    
    private Coroutine stopRumbleAfterTimeCoroutine;
    private Gamepad pad;
    
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this);
    }

    public void StartRumble(float lowFrequency, float highFrequency, float duration, bool rumbleWhileCalled = false)
    {
        // Get reference to Gamepad
        pad = Gamepad.current;
        if (pad == null) return;

        if (!rumbleWhileCalled)
        {
            // Start rumble
            pad.SetMotorSpeeds(lowFrequency, highFrequency);
            // Stop rumble after duration
            stopRumbleAfterTimeCoroutine = StartCoroutine(StopRumbleRoutine(duration, pad));
        }
        else
        {
            pad.SetMotorSpeeds(lowFrequency, highFrequency);
        }
    }

    public void UpdateRumble(float lowFrequency, float highFrequency)
    {
        pad?.SetMotorSpeeds(lowFrequency, highFrequency);
    }

    private IEnumerator StopRumbleRoutine(float duration, Gamepad pad)
    {
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        
        // Once duration ends
        pad.SetMotorSpeeds(0f, 0f);
    }

    public void StopRumble()
    {
        pad?.SetMotorSpeeds(0f, 0f);
    }
}