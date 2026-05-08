using UnityEngine;
using Unity.Cinemachine;

public class SettingCamera : MonoBehaviour
{
    private MapLoader mapLoader;
    private PlayerManager playerManager;
    private CinemachineCamera cinemachineCamera;
    private CinemachineConfiner2D confiner;

    void Awake()
    {
        mapLoader = FindAnyObjectByType<MapLoader>();
        playerManager = FindAnyObjectByType<PlayerManager>();
        cinemachineCamera = GetComponent<CinemachineCamera>();
        confiner = GetComponent<CinemachineConfiner2D>();
    }

    void OnEnable()
    {
        if (mapLoader != null)
            mapLoader.MapLoadComplete += OnMapLoadFinished;

        if (playerManager != null)
            playerManager.PlayerLoadComplete += PlayerLoadComplete;
    }

    void OnDisable()
    {
        if (mapLoader != null)
            mapLoader.MapLoadComplete -= OnMapLoadFinished;

        if (playerManager != null)
            playerManager.PlayerLoadComplete -= PlayerLoadComplete;
    }

    private void OnMapLoadFinished() { }

    private void PlayerLoadComplete()
    {
        cinemachineCamera.Target.TrackingTarget = playerManager.GetPlayerObj().transform;

        if (confiner != null)
            confiner.InvalidateBoundingShapeCache();
    }
}
