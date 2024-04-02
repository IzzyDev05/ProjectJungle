using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerVFX : MonoBehaviour
{
    [SerializeField] Transform vfxParent;
    [SerializeField] ParticleSystem doubleJumpVFX;
    [SerializeField] List<ParticleSystem> sprintVFX;

    [SerializeField] Terrain currentTerrain;

    // Start is called before the first frame update
    void Start()
    {
        doubleJumpVFX = GameObject.Find("Double Jump VFX").GetComponent<ParticleSystem>();
        
        foreach (Transform child in GameObject.Find("Sprint VFX").transform)
        {
            sprintVFX.Add(child.GetComponent<ParticleSystem>());
        }
    }

    private void Update()
    {
        currentTerrain = TerrainChecker.Instance.TerrainType(this.transform.parent);
    }

    public void DoubleJumpVFX()
    {
        if (doubleJumpVFX.isStopped)
        {
            doubleJumpVFX.Play();
        }
    }

    public void SprintVFX(bool isSprinting)
    {
        if (isSprinting)
        {
            switch (currentTerrain)
            {
                case Terrain.Dirt:
                    {                        
                        foreach (ParticleSystem other in sprintVFX)
                        {
                            if (!other.name.Contains("Dirt") && other.isPlaying)
                            {
                                other.Stop();
                            }
                            else if (other.name.Contains("Dirt"))
                            {
                                other.Play();
                            }
                        }
                        break;
                    }
                case Terrain.Grass:
                    {
                        foreach (ParticleSystem other in sprintVFX)
                        {
                            if (!other.name.Contains("Grass") && other.isPlaying)
                            {
                                other.Stop();
                            }
                            else if (other.name.Contains("Grass"))
                            {
                                other.Play();
                            }
                        }
                        break;
                    }
                default:
                    {
                        foreach (ParticleSystem other in sprintVFX)
                        {
                            if (!other.name.Contains("Dust") && other.isPlaying)
                            {
                                other.Stop();
                            }
                            else if (other.name.Contains("Dust"))
                            {
                                other.Play();
                            }
                        }
                        break;
                    }
            }
        }
        else
        {
            foreach (ParticleSystem sprintFX in sprintVFX)
            {
                if (sprintFX.isPlaying)
                {
                    sprintFX.Stop();
                }
            }
        }
    }

    #region Dynamic_VFX_Helper
    private void DynamicTerrainChange()
    {
        switch (TerrainChecker.Instance.TerrainType(this.transform.parent)) 
        {
            case Terrain.Other:
                {
                    
                    break;
                }
            case Terrain.Grass:
                {

                    break;
                }
        }

    }
    #endregion
}
