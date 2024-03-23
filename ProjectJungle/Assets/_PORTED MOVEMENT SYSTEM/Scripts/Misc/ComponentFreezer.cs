using UnityEngine;

public class ComponentFreezer : MonoBehaviour
{
    [SerializeField] private bool freezePosition = true;
    [SerializeField] private bool freezeRotation = true;
    
    private void Update()
    {
        if (freezePosition) transform.localPosition = Vector3.zero;
        if (freezeRotation) transform.localRotation = Quaternion.identity;
    }
}