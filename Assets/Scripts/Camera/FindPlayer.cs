using UnityEngine;
using System.Collections;
using Unity.Cinemachine;

public class FindPlayer : MonoBehaviour
{
    private GameObject player;
    private PlayerManager playerManager;
    private CinemachineCamera cinemachineCamera;

    void Awake()
    {
        playerManager = FindAnyObjectByType<PlayerManager>();
        cinemachineCamera = GetComponent<CinemachineCamera>();
    }

    void Start()
    {
        StartCoroutine(WaitForPlayerObj());
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
}
