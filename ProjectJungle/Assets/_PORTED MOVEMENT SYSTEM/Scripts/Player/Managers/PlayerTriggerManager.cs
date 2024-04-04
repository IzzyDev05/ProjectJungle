using System;
using UnityEngine;

public class PlayerTriggerManager : MonoBehaviour
{
    private Transform player;
    private LevelLoader levelLoader;
    private Tags objTag;

    private bool hasKey;

    private void Start()
    {
        levelLoader = FindObjectOfType<LevelLoader>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("LevelEnd")) levelLoader.LoadNextLevel();

        if (other.CompareTag("Respawn"))
        {
            var respawnPoint = other.GetComponent<Respawner>().GetRespawnPoint();
            player.position = respawnPoint;
        }

        if (other.CompareTag("Key"))
        {
            hasKey = true;
            Destroy(other.gameObject);
        }

        if (other.CompareTag("Win"))
        {
            if (hasKey) levelLoader.WinGame();
        }
    }
}