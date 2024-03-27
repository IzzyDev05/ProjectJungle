using UnityEngine;

public class PlayerVFX : MonoBehaviour
{
    [SerializeField] Transform vfxParent;
    [SerializeField] ParticleSystem doubleJumpVFX;
    [SerializeField] ParticleSystem sprintVFX;

    // Start is called before the first frame update
    void Start()
    {
        doubleJumpVFX = GameObject.Find("Double Jump VFX").GetComponent<ParticleSystem>();
        sprintVFX = GameObject.Find("Sprint VFX").GetComponent<ParticleSystem>();
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
            sprintVFX.Play();
        }
        else
        {
            sprintVFX.Stop();
        }
    }

    #region Dynamic_VFX_Helper
    private void DynamicTerrainChange()
    {
        switch (TerrainChecker.Instance.TerrainType(this.transform.parent)) 
        {
            case TerrainTypes.Other:
                {
                    
                    break;
                }
            case TerrainTypes.Grass:
                {

                    break;
                }
        }

    }
    #endregion
}
