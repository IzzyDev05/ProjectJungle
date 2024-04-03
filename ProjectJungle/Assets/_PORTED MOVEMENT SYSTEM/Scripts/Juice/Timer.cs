using System;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [HideInInspector] public bool ShouldAddTime;
    [SerializeField] private TextMeshProUGUI timer;
    private float levelPlayTime;

    private void Start()
    {
        timer.text = "00:00:000";
    }

    private void Update()
    {
        if (ShouldAddTime) levelPlayTime += Time.deltaTime;

        var playTime = GetCurrentPlayTime();
        timer.text = playTime;
    }

    private string GetCurrentPlayTime()
    {
        TimeSpan time = TimeSpan.FromSeconds(levelPlayTime);
        return $"{time.Minutes:D2}:{time.Seconds:D2}:{time.Milliseconds:D3}";
    }
}