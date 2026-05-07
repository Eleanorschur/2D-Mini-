using UnityEngine;
using System.Collections;
using Unity.Cinemachine;

public class FindPlayer : MonoBehaviour
{
    private GameObject player;
    private CinemachineCamera cinemachineCamera;

    void Awake()
    {
        cinemachineCamera = GetComponent<CinemachineCamera>();
    }

    void Start()
    {
        StartCoroutine(FindAndAssignPlayer());
    }

    private IEnumerator FindAndAssignPlayer()
    {
        GameObject player = null;

        while (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");

            if (player != null)
            {
                cinemachineCamera.Target.TrackingTarget = player.gameObject.transform;
                yield break;
            }
            yield return new WaitForSeconds(0.5f);
        }
    }
}
