using UnityEngine;
using UnityEngine.InputSystem;

public class ExitDoor : MonoBehaviour
{
    public ExitManager exitManager;
    private Animator animator;
    private PlayerManager playerManager;
    private Movement playerMovement;
    private StageReset stageReset;
    private Timer timer;
    private ZKey currentZKey;

    private bool nearDoor = false;
    private bool isDoorOpen = false;
    private bool activeDoor = false;

    private StageClearPopup stageClearPopup;
    private SceneOpenEffect sceneOpenEffect; //2026.05.13

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void OnEnable()
    {
        playerManager = FindAnyObjectByType<PlayerManager>();
        //exitManager = GetComponentInParent<ExitManager>();

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
        DoorOpen(false);
        sceneOpenEffect = FindAnyObjectByType<SceneOpenEffect>();  //2026.05.13
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
        if (!collision.CompareTag("Player")) return;

        nearDoor = false;

        if (currentZKey != null)
        {
            currentZKey.Hide();
            currentZKey = null;
        }
    }

    public void DoorOpen(bool open)
    {
        if (this == null || animator == null) return;
        Debug.Log("Door" + open);
        isDoorOpen = open;

        if (open)
        {
            AudioManager.Instance?.PlayDoorSFX(); //05.16. AudioManager를 위해 추가
            animator.SetTrigger("Open");
        }
        else
            animator.SetTrigger("Close");
    }

    public void ExitStage()
    {

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

            if (stageClearPopup != null)
            {
                stageClearPopup.ShowPopup();
                AudioManager.Instance?.PlayClearBGM();
            }
            else
            {

            }

            if (currentZKey != null)
            {
                currentZKey.Hide();
                currentZKey = null;
            }
            else
            {
                ExitStage();
            }

        }
    }
}