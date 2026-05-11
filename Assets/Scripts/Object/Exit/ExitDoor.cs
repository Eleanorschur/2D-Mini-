using UnityEngine;

public class ExitDoor : MonoBehaviour
{
    private MapLoader mapLoader;
    private ExitManager exitManager;
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

    void Awake()
    {
        mapLoader = FindAnyObjectByType<MapLoader>();
        exitManager = GetComponentInParent<ExitManager>();
        playerManager = FindAnyObjectByType<PlayerManager>();
        timer = FindAnyObjectByType<Timer>();
        stageReset = FindAnyObjectByType<StageReset>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        nearDoor = false;
        isDoorOpen = false;
        activeDoor = false;
    }

    void OnEnable()
    {
        if (mapLoader != null)
            mapLoader.MapLoadComplete += OnMapLoadFinished;

        if (playerManager != null)
            playerManager.PlayerLoadComplete += PlayerLoadComplete;

        if (exitManager != null)
            exitManager.ExitLoadComplete += UpdateExitReference;
    }

    void OnDisable()
    {
        if (mapLoader != null)
            mapLoader.MapLoadComplete -= OnMapLoadFinished;

        if (playerManager != null)
            playerManager.PlayerLoadComplete -= PlayerLoadComplete;

        if (exitManager != null)
            exitManager.ExitLoadComplete -= UpdateExitReference;
    }

    private void OnMapLoadFinished()
    {

    }

    private void PlayerLoadComplete()
    {
        playerMovement = playerManager.GetPlayerObj().GetComponent<Movement>();
    }

    void UpdateExitReference()
    {
        ExitDoor newExit = exitManager.GetExitObj();
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
        isDoorOpen = open;
        spriteRenderer.sprite = open ? openDoor : closeDoor;
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
            Debug.Log("Å»Ãâ ¿Ï·á");
            mapLoader.NextStage();

            if (currentZKey != null)
            {
                currentZKey.Hide();
                currentZKey = null;
            }
        }
    }
}
