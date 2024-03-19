using System.Collections;
using UnityEngine;

public class DisableTerrainCamera : MonoBehaviour
{
    private Camera renderCamera;

    private IEnumerator Start()
    {
        renderCamera = GetComponent<Camera>();
        yield return new WaitForEndOfFrame();
        renderCamera.enabled = false;
    }
}