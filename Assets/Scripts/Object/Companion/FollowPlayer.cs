using UnityEngine;
using System.Collections.Generic;

public class FollowPlayer : MonoBehaviour
{
    private PlayerManager playerManager;
    private Rigidbody2D rigid2D;
    private Companion companion;
    private RecodeMovement recode;
    private Animator animator;

    private Vector3 originScale;
    private Vector3 leftScale;
    private Vector3 rightScale;


    private int currentFollowIndex = -1;
    private int delayFrames = 0;
    private int stopCount = 0;
    private int stopCountMax = 12;
    private float currentOffset = 0f;
    private float offsetSmoothSpeed = 5f;
    private float followSpeed = 50f;
    [SerializeField] private float offsetYpos = 0.05f;

    void Awake()
    {
        companion = GetComponent<Companion>(); // 같은 계층
        animator = GetComponent<Animator>(); // 같은 계층
    }

    void OnEnable()
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
        originScale = new Vector3(1, 1, 1);
        leftScale = new Vector3(-originScale.x, originScale.y, originScale.z);
        rightScale = new Vector3(originScale.x, originScale.y, originScale.z);
    }

    private void PlayerLoadComplete()
    {
        recode = playerManager.GetPlayerObj().GetComponent<RecodeMovement>();
        rigid2D = playerManager.GetPlayerObj().GetComponent<Rigidbody2D>();
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

            Vector3 targetPos = targetData.Position;
            targetPos.y += offsetYpos;

            if (Mathf.Abs(rigid2D.linearVelocityX) < 0.01f)
                stopCount++;
            else
                stopCount = 0;

            float desiredOffset = 0f;

            if (stopCount > stopCountMax)
                desiredOffset = (currentFollowIndex - 1) * 0.5f;

            currentOffset = Mathf.Lerp(currentOffset, desiredOffset, Time.fixedDeltaTime * offsetSmoothSpeed);

            targetPos.x += currentOffset;

            transform.position = Vector3.Lerp(transform.position, targetPos, followSpeed * Time.fixedDeltaTime);

            if (targetData.Dir.x > 0)
                transform.localScale = rightScale;
            else if (targetData.Dir.x < 0)
                transform.localScale = leftScale;

            animator.SetFloat("X", targetData.Dir.x);
        
        }
    }

    public void SetIndex(int index)
    {
        currentFollowIndex = index;
        delayFrames = (currentFollowIndex + 1) * 10;
    }
}