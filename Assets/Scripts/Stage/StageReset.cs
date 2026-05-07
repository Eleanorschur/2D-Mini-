using UnityEngine;

public class StageReset : MonoBehaviour
{
    private GameObject player;
    private StatusCheck statusCheck;
    private ItemCheck itemCheck;
    private ExitDoor exitDoor;
    private PlatformManager platformManager;
    private SwitchManager switchManager;
    private CheckPointManager checkPointManager;
    private CompanionManager companionManager;

    private Vector3 startPosition;
    private Vector3 revivePosition;

    private float reviveAddY = 1.5f;
    private bool resetLock = false;


    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        exitDoor = FindAnyObjectByType<ExitDoor>();
        itemCheck = FindAnyObjectByType<ItemCheck>();
        statusCheck = FindAnyObjectByType<StatusCheck>();
        platformManager = FindAnyObjectByType<PlatformManager>();
        switchManager = FindAnyObjectByType<SwitchManager>();
        checkPointManager = FindAnyObjectByType<CheckPointManager>();
        companionManager = FindAnyObjectByType<CompanionManager>();
    }

    void Start()
    {
        startPosition = player.transform.position;
        resetLock = false;
    }

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
        player.transform.position = revivePosition;

        statusCheck.ChangeForm(0);
        itemCheck.itemReset();
        platformManager.SwitchingPlatformHide(0);
        switchManager.AllSwitchReset();
        companionManager.CompanionReset();
        checkPointManager.CheckPointReset();
        exitDoor.DoorOpen(false);
        player.GetComponent<Rigidbody2D>().linearVelocity = Vector3.zero;
    }

    public void DeadStageReset()
    {
        if (checkPointManager.GetFinalCheckPoint() != null)
            revivePosition = checkPointManager.GetFinalCheckPoint().transform.position;
        else
            revivePosition = startPosition;

        revivePosition.y += reviveAddY;
        player.transform.position = revivePosition;

        player.GetComponent<Rigidbody2D>().linearVelocity = Vector3.zero;
    }

    public void ResetLock(bool locking)
    {
        resetLock = locking;
    }
}
