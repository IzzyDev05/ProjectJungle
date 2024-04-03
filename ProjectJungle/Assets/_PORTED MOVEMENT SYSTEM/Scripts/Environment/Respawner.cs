using System;
using UnityEngine;

public class Respawner : MonoBehaviour
{
    [SerializeField] private Transform respawnPoint;

    public Vector3 GetRespawnPoint()
    {
        return respawnPoint.position;
    }
}