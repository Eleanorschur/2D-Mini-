using Unity.Cinemachine;
using UnityEngine;

public class CameraShaker : MonoBehaviour
{
    private CinemachineImpulseSource impulseSource;

    void Awake()
    {
        impulseSource = GetComponent<CinemachineImpulseSource>();
    }

    public void Shake()
    {
        impulseSource.GenerateImpulse();
    }
}
