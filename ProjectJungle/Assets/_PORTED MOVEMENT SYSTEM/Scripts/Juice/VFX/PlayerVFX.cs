using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerVFX : MonoBehaviour
{
    [SerializeField] Transform vfxParent;
    [SerializeField] ParticleSystem doubleJumpVFX;
    [SerializeField] List<ParticleSystem> sprintVFX;

    // Start is called before the first frame update
    void Start()
    {
        doubleJumpVFX = GameObject.Find("Double Jump VFX").GetComponent<ParticleSystem>();

        foreach (Transform child in GameObject.Find("Sprint VFX").transform)
        {
            sprintVFX.Add(child.GetComponent<ParticleSystem>());
        }
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
            switch (GetComponentInChildren<PlayerSounds>().groundType)
            {
                case TerrainType.Dirt:
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
                case TerrainType.Grass:
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
}
