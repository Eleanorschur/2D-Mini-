using UnityEngine;

public class StageReset : MonoBehaviour
{
    public MapLoader mapLoader;
    public PlayerManager playerManager;
    public ExitManager exitManager;
    private StatusCheck statusCheck;
    private ItemCheck itemCheck;
    private PlatformManager platformManager;
    private SwitchManager switchManager;
    private CheckPointManager checkPointManager;
    private CompanionManager companionManager;
    private Timer timer;

    private Vector3 startPosition;
    private Vector3 revivePosition;

    private float reviveAddY = 1.5f;
    private bool resetLock = false;

    void Awake()
    {

    }

    void Start()
    {
        platformManager = FindAnyObjectByType<PlatformManager>();
        switchManager = FindAnyObjectByType<SwitchManager>();
        checkPointManager = FindAnyObjectByType<CheckPointManager>();
        companionManager = FindAnyObjectByType<CompanionManager>();
        timer = FindAnyObjectByType<Timer>();

        resetLock = false;
    }

    void OnEnable()
    {
        if (mapLoader != null)
            mapLoader.MapLoadComplete += OnMapLoadFinished;

        if (playerManager != null)
            playerManager.PlayerLoadComplete += PlayerLoadComplete;

        if (exitManager != null)
            exitManager.ExitLoadComplete += OnExitLoadFinished;
    }

    void OnDisable()
    {
        if (mapLoader != null)
            mapLoader.MapLoadComplete -= OnMapLoadFinished;

        if (playerManager != null)
            playerManager.PlayerLoadComplete -= PlayerLoadComplete;

        if (exitManager != null)
            exitManager.ExitLoadComplete -= OnExitLoadFinished;
    }

    private void OnMapLoadFinished()
    {

    }

    private void PlayerLoadComplete()
    {
        GameObject playerObj = playerManager.GetPlayerObj();

        startPosition = playerObj.transform.position;
        itemCheck = playerObj.GetComponent<ItemCheck>();
        statusCheck = playerObj.GetComponent<StatusCheck>();

        // 새 스테이지 시작 시 무조건 기본 초록색으로 초기화
        if (statusCheck != null)
            statusCheck.ChangeForm(0);

        if (itemCheck != null)
            itemCheck.itemReset();

        Rigidbody2D rb = playerObj.GetComponent<Rigidbody2D>();

        if (rb != null)
            rb.linearVelocity = Vector2.zero;
    }

    private void OnExitLoadFinished() { }

    void Update()
    {
        if ( ! Input.GetKeyDown(KeyCode.R) || resetLock)
            return;

        ManualStageReset();
    }

    public void ManualStageReset()
    {
        revivePosition = startPosition;
        revivePosition.y += reviveAddY;
        playerManager.GetPlayerObj().transform.position = revivePosition;

        statusCheck.ChangeForm(0);
        itemCheck.itemReset();
        platformManager.SwitchingPlatformHide(0);
        switchManager.AllSwitchReset();
        companionManager.CompanionReset();
        checkPointManager.CheckPointReset();
        checkPointManager.SetFinalCheckPoint(null);
        exitManager.GetExitObj().DoorOpen(false);
        playerManager.GetPlayerObj().GetComponent<Rigidbody2D>().linearVelocity = Vector3.zero;
    }

    public void DeadStageReset()
    {
        if (checkPointManager.GetFinalCheckPoint() != null)
        {
            revivePosition = checkPointManager.GetFinalCheckPoint().transform.position;
            statusCheck.ChangeForm(checkPointManager.PlayerStatus);
            platformManager.SwitchingPlatformHide(checkPointManager.PlayerStatus);
            itemCheck.CheckpointReset();
        }
        else
        {
            revivePosition = startPosition;
            statusCheck.ChangeForm(0);
            platformManager.SwitchingPlatformHide(0);
            switchManager.AllSwitchReset();
            itemCheck.CheckpointReset();
        }

        revivePosition.y += reviveAddY;
        playerManager.GetPlayerObj().transform.position = revivePosition;

        playerManager.GetPlayerObj().GetComponent<Rigidbody2D>().linearVelocity = Vector3.zero;
        AudioManager.Instance?.PlayRespawnSFX(); //05.16. AudioManager를 위해 추가
    }

    public void NextStageReset()
    {
        resetLock = false;
        timer.ResetTimer();
    }

    public void SetStartPosition(Vector3 position)
    {
        startPosition = position;
    }

    public void ResetLock(bool locking)
    {
        resetLock = locking;
    }
}
