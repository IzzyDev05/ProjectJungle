using Cinemachine;
using UnityEngine;

public class ScreenShakeManager : MonoBehaviour
{
    public void ScreenShake(CinemachineImpulseSource impulseSource, float power)
    {
        print(power);
        power = StaticUtils.Remap(power, 0f, 40f, 1f, 3f);
        impulseSource.m_DefaultVelocity = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));
        impulseSource.GenerateImpulse(power);
    }
}