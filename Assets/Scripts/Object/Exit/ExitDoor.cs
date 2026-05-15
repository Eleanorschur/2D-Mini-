using UnityEngine;

public class ExitDoor : MonoBehaviour
{
    public ExitManager exitManager;
    private PlayerManager playerManager;
    private Movement playerMovement;
    private SpriteRenderer spriteRenderer;
    private StageReset stageReset;
    private Timer timer;
    private ZKey currentZKey;

    public Sprite openDoor;
    public Sprite closeDoor;

    private bool nearDoor = false;
    private bool isDoorOpen = false;
    private bool activeDoor = false;

    private StageClearPopup stageClearPopup;


    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>(); // 같은 계층
    }

    void OnEnable()
    {
        playerManager = FindAnyObjectByType<PlayerManager>();
        exitManager = GetComponentInParent<ExitManager>();

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
        timer = FindAnyObjectByType<Timer>();
        stageReset = FindAnyObjectByType<StageReset>();

        stageClearPopup = FindAnyObjectByType<StageClearPopup>();

        nearDoor = false;
        isDoorOpen = false;
        activeDoor = false;


    }

    private void PlayerLoadComplete()
    {
        playerMovement = playerManager.GetPlayerObj().GetComponent<Movement>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            nearDoor = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if ( ! collision.CompareTag("Player")) return;

        nearDoor = false;

        if (currentZKey != null)
        {
            currentZKey.Hide();
            currentZKey = null;
        }
    }

    public void DoorOpen(bool open)
    {
        if (this == null || spriteRenderer == null) return;

        isDoorOpen = open;
        spriteRenderer.sprite = open ? openDoor : closeDoor;
    }

    public void ExitStage()
    {
        exitManager.NextStage();
    }

    private void Update()
    {
        if (activeDoor)
            return;

        if (nearDoor && isDoorOpen && playerMovement.IsGrounded && currentZKey == null)
        {
            currentZKey = ZKeyPool.Instance.GetZKey();
            currentZKey.Setup(this.transform);
        }

        if (currentZKey != null && !playerMovement.IsGrounded)
        {
            currentZKey.Hide();
            currentZKey = null;
        }

        if (Input.GetKeyDown(KeyCode.Z) && nearDoor && playerMovement.IsGrounded && isDoorOpen)
        {
            activeDoor = true;
            playerMovement.MoveLock(true);
            stageReset.ResetLock(true);
            timer.StopTimer();
            Debug.Log("탈출 완료");

            if (stageClearPopup != null)
            {
                stageClearPopup.ShowPopup();
            }
            else
            {
                Debug.LogError("StageClearPopup을 찾지 못했습니다.");
            }

            if (currentZKey != null)
            {
                currentZKey.Hide();
                currentZKey = null;
            }
        }
    }
}
