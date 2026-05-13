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

    private void OnMapLoadFinished() { }

    private void PlayerLoadComplete()
    {
        startPosition = playerManager.GetPlayerObj().transform.position;
        itemCheck = playerManager.GetPlayerObj().GetComponent<ItemCheck>();
        statusCheck = playerManager.GetPlayerObj().GetComponent<StatusCheck>();
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
        }
        else
        {
            revivePosition = startPosition;
            statusCheck.ChangeForm(0);
            platformManager.SwitchingPlatformHide(0);
        }

        revivePosition.y += reviveAddY;
        playerManager.GetPlayerObj().transform.position = revivePosition;

        playerManager.GetPlayerObj().GetComponent<Rigidbody2D>().linearVelocity = Vector3.zero;
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
