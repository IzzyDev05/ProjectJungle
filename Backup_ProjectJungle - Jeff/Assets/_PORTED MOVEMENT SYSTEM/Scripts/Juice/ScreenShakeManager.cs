using Cinemachine;
using UnityEngine;

public class ScreenShakeManager : MonoBehaviour
{
    public void ScreenShake(CinemachineImpulseSource impulseSource, float power)
    {
        power = StaticUtils.Remap(power, 0f, 40f, 0f, 1.5f);
        impulseSource.GenerateImpulse(power);
    }
}