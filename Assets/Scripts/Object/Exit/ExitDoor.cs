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

    private SceneOpenEffect sceneOpenEffect; //2026.05.13 페이드 아웃 동작을 위해 추가 


    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
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

        nearDoor = false;
        isDoorOpen = false;
        activeDoor = false;

        sceneOpenEffect = FindAnyObjectByType<SceneOpenEffect>();  //2026.05.13 페이드 아웃 동작을 위해 추가 

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
            AudioManager.Instance.PlayExitEnter(); // audioManager를 위한 코드 추가
            playerMovement.MoveLock(true);
            stageReset.ResetLock(true);
            timer.StopTimer();

            if (currentZKey != null)
            {
                currentZKey.Hide();
                currentZKey = null;
            }

            if (sceneOpenEffect != null)  //2026.05.13 페이드 아웃 동작을 위해 추가 
            {
                sceneOpenEffect.OnEffectComplete = null;
                sceneOpenEffect.OnEffectComplete += ExitStage;
                sceneOpenEffect.SetTarget(playerMovement.transform);
                sceneOpenEffect.PlayIrisOut();
            }
            else
            {
                ExitStage();
            }                             //2026.05.13 페이드 아웃 동작을 위해 추가 


        }
    }
}
