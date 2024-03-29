using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Terrain 
{
    None,
    Other,
    Grass,
    Wood,
    Plantation
}


public class TerrainChecker : MonoBehaviour
{
    public static TerrainChecker Instance;

    private void Awake()
    {
        #region SINGLETON
        if (Instance != null)
        {
            Debug.LogError("Multiple TerrainChecker Instances found.");
        }

        Instance = this;
        #endregion
    }

    /// <summary>
    /// Gets the type of terrain.
    /// </summary>
    /// <param name="rayOriginTransform"></param>
    /// <returns></returns>
    public Terrain TerrainType(Transform rayOriginTransform)
    {
        RaycastHit hit;

        Ray ray = new Ray(rayOriginTransform.position + Vector3.up * 0.5f, Vector3.down);

        if (Physics.Raycast(ray, out hit, 1.0f, Physics.AllLayers, QueryTriggerInteraction.Ignore))
        {
            Renderer groundRenderer = hit.collider.GetComponentInChildren<Renderer>();

            if (groundRenderer)
            {
                // Debug.Log(groundRenderer.material.name);
                if (groundRenderer.material.name.Contains("Wood"))
                {
                    return Terrain.Wood;
                }
                else
                {
                     return Terrain.Grass;
                }
            }
        }

        return Terrain.None;
    }
}
