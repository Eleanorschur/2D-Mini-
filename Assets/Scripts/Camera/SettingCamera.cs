using UnityEngine;
using Unity.Cinemachine;

public class SettingCamera : MonoBehaviour
{
    private PlayerManager playerManager;
    private CinemachineCamera cinemachineCamera;
    private CinemachineConfiner2D confiner;

    void Awake()
    {
        cinemachineCamera = GetComponent<CinemachineCamera>();
        confiner = GetComponent<CinemachineConfiner2D>();
    }

    private void Start()
    {
        playerManager = FindAnyObjectByType<PlayerManager>();
    }

    public void PlayerLoadComplete(GameObject target)
    {
        cinemachineCamera.Target.TrackingTarget = target.transform;

        if (confiner != null)
            confiner.InvalidateBoundingShapeCache();
    }
}
