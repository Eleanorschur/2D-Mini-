using UnityEngine;
using System.Collections;
using Unity.Cinemachine;

public class FindPlayer : MonoBehaviour
{
    private GameObject player;
    private PlayerManager playerManager;
    private CinemachineCamera cinemachineCamera;
    private CinemachineConfiner2D confiner;

    void Awake()
    {
        playerManager = FindAnyObjectByType<PlayerManager>();
        cinemachineCamera = GetComponent<CinemachineCamera>();
        confiner = GetComponent<CinemachineConfiner2D>();
    }

    void Start()
    {
        StartCoroutine(WaitForPlayerObj());
        RefreshConfiner();
    }

    private IEnumerator WaitForPlayerObj()
    {
        while (playerManager.player == null)
        {
            yield return null;
        }
        player = playerManager.GetPlayerObj();
        cinemachineCamera.Target.TrackingTarget = player.transform;
        Debug.Log("FindPlayer : player 오브젝트 취득 완료");
    }

    public void RefreshConfiner()
    {
        // 컨파이너의 경로 캐시를 무효화하여 다시 계산하게 만듭니다.
        if (confiner != null)
            confiner.InvalidateBoundingShapeCache();
    }
}
