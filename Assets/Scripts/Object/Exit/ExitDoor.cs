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
    private SceneOpenEffect sceneOpenEffect; //2026.05.13 페이드 아웃 동작을 위해 추가 

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
        if (this == null || animator == null) return;
        Debug.Log("Door" + open);
        isDoorOpen = open;

        if (open)
            animator.SetTrigger("Open");
        else
            animator.SetTrigger("Close");
    }

    public void ExitStage()
    {
        // 이제 문에 들어갔다고 바로 다음 스테이지로 넘기지 않음
        // 다음 스테이지 이동은 StageClearPopup의 NextStageButton에서만 처리
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
