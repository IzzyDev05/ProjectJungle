using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TerrainType
{
    None,
    Other,
    Grass,
    Wood,
    Plantation,
    Dirt
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

    public TerrainType CheckTerrainType(Transform rayOriginTransform)
    {
        string terrainTypeName = "";

        RaycastHit hit;

        Ray ray = new Ray(rayOriginTransform.position + Vector3.up * 0.5f, Vector3.down);

        if (Physics.Raycast(ray, out hit, 1.0f, Physics.AllLayers, QueryTriggerInteraction.Ignore))
        {
            Terrain ground = hit.collider.GetComponent<Terrain>();

            if (ground != null)
            {
                // Check if the collider hit is on the terrain
                TerrainCollider terrainCollider = hit.collider.GetComponent<TerrainCollider>();
                Terrain terrain = hit.collider.GetComponent<Terrain>();

                if (terrainCollider != null && terrain != null)
                {
                    // Get the terrain data
                    TerrainData terrainData = terrain.terrainData;

                    // Get the normalized position of the hit point
                    Vector3 terrainLocalPos = hit.point - terrain.gameObject.transform.position;
                    Vector3 normalizedPos = new Vector3(
                        Mathf.InverseLerp(0, terrainData.size.x, terrainLocalPos.x),
                        Mathf.InverseLerp(0, terrainData.size.y, terrainLocalPos.y),
                        Mathf.InverseLerp(0, terrainData.size.z, terrainLocalPos.z)
                    );

                    // Get the terrain layers
                    TerrainLayer[] terrainLayers = terrainData.terrainLayers;

                    // Sample the alpha map to determine the terrain layer
                    int layerIndex = 0;
                    float maxOpacity = 0f;
                    for (int i = 0; i < terrainLayers.Length; i++)
                    {
                        float opacity = terrainData.GetAlphamapTexture(0).GetPixelBilinear(normalizedPos.x, normalizedPos.z)[i];
                        if (opacity > maxOpacity)
                        {
                            maxOpacity = opacity;
                            layerIndex = i;
                        }
                    }

                    // You can now access the terrain layer using the layerIndex
                    TerrainLayer currentLayer = terrainLayers[layerIndex];

                    if (currentLayer.name.Contains("Dirt"))
                    {
                        terrainTypeName = "Dirt";
                    }
                    else if (currentLayer.name.Contains("Grass"))
                    {
                        terrainTypeName = "Grass";

                    }
                }
            }
            else
            {
                terrainTypeName = hit.collider.tag;
            }

            switch (terrainTypeName)
            {
                case "Wood":
                    {
                        return TerrainType.Wood;
                    }
                case "Dirt":
                    {
                        return TerrainType.Dirt;
                    }
                case "Grass":
                    {
                        return TerrainType.Grass;
                    }
                case "Plant":
                    {
                        return TerrainType.Plantation;
                    }
                default:
                    {
                        return TerrainType.Other;
                    }
            }
        }

        return TerrainType.None;
    }
}
