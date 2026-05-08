using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FollowPlayer : MonoBehaviour
{
    private GameObject player;
    private PlayerManager playerManager;
    private Companion companion;
    private RecodeMovement recode;

    private Vector3 originScale;
    private Vector3 leftScale;
    private Vector3 rightScale;

    private int currentFollowIndex = -1;
    private int delayFrames = 0;
    [SerializeField]private float followSpeed = 50f;

    void Awake()
    {
        playerManager = FindAnyObjectByType<PlayerManager>();
        companion = GetComponent<Companion>();
    }

    void Start()
    {
        StartCoroutine(WaitForPlayerObj());
        originScale = transform.localScale;
        leftScale = new Vector3(-originScale.x, originScale.y, originScale.z);
        rightScale = new Vector3(originScale.x, originScale.y, originScale.z);
    }

    private void FixedUpdate()
    {
        if (recode == null || ! companion.ActiveCompanion) return;

        Follow();
    }

    private IEnumerator WaitForPlayerObj()
    {
        while (playerManager.player == null)
        {
            yield return null;
        }
        player = playerManager.GetPlayerObj();
        recode = player?.GetComponent<RecodeMovement>();
        Debug.Log("FollowPlayer : player 오브젝트 취득 완료");
    }


    private void Follow()
    {
        List<RecodeMovement.PlayerMove> path = recode.GetRecode();

        if (currentFollowIndex == -1) return;

        if (path.Count > delayFrames)
        {
            int targetIndex = Mathf.Clamp(path.Count - 1 - delayFrames, 0, path.Count - 1);

            RecodeMovement.PlayerMove targetData = path[targetIndex];

            transform.position = Vector3.Lerp(transform.position, targetData.Position, followSpeed * Time.fixedDeltaTime);

            if (targetData.Dir.x > 0)
                transform.localScale = rightScale;
            else if (targetData.Dir.x < 0)
                transform.localScale = leftScale;
        }
    }

    public void SetIndex(int index)
    {
        currentFollowIndex = index;
        delayFrames = (currentFollowIndex + 1) * 10;
    }
}