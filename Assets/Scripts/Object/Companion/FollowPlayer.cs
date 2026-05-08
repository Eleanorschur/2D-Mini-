using UnityEngine;
using System.Collections.Generic;

public class FollowPlayer : MonoBehaviour
{
    private MapLoader mapLoader;
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
        mapLoader = FindAnyObjectByType<MapLoader>();
        playerManager = FindAnyObjectByType<PlayerManager>();
        companion = GetComponent<Companion>();
    }

    void Start()
    {
        originScale = transform.localScale;
        leftScale = new Vector3(-originScale.x, originScale.y, originScale.z);
        rightScale = new Vector3(originScale.x, originScale.y, originScale.z);
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
        recode = playerManager.GetPlayerObj().GetComponent<RecodeMovement>();
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
        delayFrames = (currentFollowIndex + 1) * 10;
    }
}