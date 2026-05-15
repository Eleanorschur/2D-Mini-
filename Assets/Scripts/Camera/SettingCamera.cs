using UnityEngine;
using System.Collections;
using System.Collections.Generic;
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

    private void OnEnable()
    {
        playerManager = FindAnyObjectByType<PlayerManager>();

        if (playerManager != null)
            playerManager.PlayerLoadComplete += PlayerLoadComplete;
    }

    void OnDisable()
    {
        if (playerManager != null)
            playerManager.PlayerLoadComplete -= PlayerLoadComplete;
    }

    void Start()
    {

    }

    private void PlayerLoadComplete()
    {
        StartCoroutine(InvalidateConfinerCache());
    }

    private IEnumerator InvalidateConfinerCache()
    {
        yield return new WaitForEndOfFrame();

        cinemachineCamera.Target.TrackingTarget = playerManager.GetPlayerObj().transform;

        confiner.InvalidateLensCache();
        confiner.InvalidateBoundingShapeCache();
    }


    public void PlayerLoadComplete(GameObject target)
    {
        //cinemachineCamera.Target.TrackingTarget = target.transform;
        //if (confiner != null)
        //    confiner.InvalidateBoundingShapeCache();
    }
}