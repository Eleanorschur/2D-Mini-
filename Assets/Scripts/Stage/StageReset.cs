using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class StageReset : MonoBehaviour
{
    private GameObject player;
    private PlayerManager playerManager;
    private ExitDoor exitDoor;
    private ExitManager exitManager;
    private StatusCheck statusCheck;
    private ItemCheck itemCheck;
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
        playerManager = FindAnyObjectByType<PlayerManager>();
        exitManager = FindAnyObjectByType<ExitManager>();
        platformManager = FindAnyObjectByType<PlatformManager>();
        switchManager = FindAnyObjectByType<SwitchManager>();
        checkPointManager = FindAnyObjectByType<CheckPointManager>();
        companionManager = FindAnyObjectByType<CompanionManager>();
    }

    void Start()
    {
        StartCoroutine(WaitForPlayerObj());
        StartCoroutine(WaitForExitObj());
        resetLock = false;
    }

    void Update()
    {
        if ( ! Input.GetKeyDown(KeyCode.R) || resetLock)
            return;

        ManualStageReset();
    }

    private IEnumerator WaitForPlayerObj()
    {
        while (playerManager.player == null)
        {
            yield return null;
        }

        player = playerManager.GetPlayerObj();
        SetStartPosition(player.transform.position);
        itemCheck = FindAnyObjectByType<ItemCheck>();
        statusCheck = FindAnyObjectByType<StatusCheck>();
        Debug.Log("StageReset : player 오브젝트 취득 완료");
    }

    private IEnumerator WaitForExitObj()
    {
        while (exitManager.exit == null)
        {
            yield return null;
        }

        exitDoor = exitManager.GetExitObj();
        Debug.Log("StageReset : Exit 오브젝트 취득 완료");
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

    public void SetStartPosition(Vector3 position)
    {
        startPosition = position;
    }

    public void ResetLock(bool locking)
    {
        resetLock = locking;
    }
}
