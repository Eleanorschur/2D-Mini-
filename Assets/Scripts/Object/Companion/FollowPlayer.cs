using UnityEngine;
using System.Collections.Generic;

public class FollowPlayer : MonoBehaviour
{
    private Companion companion;
    private GameObject player;
    private Movement playerMovement;
    private RecodeMovement recode;

    private Vector3 originScale;
    private Vector3 leftScale;
    private Vector3 rightScale;
    private float moveDir;

    private int currentFollowIndex = -1;
    private int delayFrames = 0;
    [SerializeField]private float followSpeed = 50f;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        companion = GetComponent<Companion>();
        playerMovement = FindAnyObjectByType<Movement>();
        recode = player?.GetComponent<RecodeMovement>();
    }

    void Start()
    {
        originScale = transform.localScale;
        leftScale = new Vector3(-originScale.x, originScale.y, originScale.z);
        rightScale = new Vector3(originScale.x, originScale.y, originScale.z);
    }

    private void FixedUpdate()
    {
        if (recode == null || ! companion.ActiveCompanion) return;

        Follow();
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
        print(index);
        delayFrames = (currentFollowIndex + 1) * 10;
    }
}