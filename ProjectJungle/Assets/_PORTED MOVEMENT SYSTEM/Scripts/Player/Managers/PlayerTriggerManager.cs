using System;
using UnityEngine;

public class PlayerTriggerManager : MonoBehaviour
{
    private LevelLoader levelLoader;
    private Tags objTag;

    private void Start()
    {
        levelLoader = FindObjectOfType<LevelLoader>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("LevelEnd")) levelLoader.LoadNextLevel();
    }
}