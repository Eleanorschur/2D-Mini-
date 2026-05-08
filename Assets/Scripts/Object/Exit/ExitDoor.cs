using UnityEngine;
using System.Collections;

public class ExitDoor : MonoBehaviour
{
    private GameObject player;
    private PlayerManager playerManager;
    private SpriteRenderer spriteRenderer;
    private StageReset stageReset;
    private Movement playerMovement;
    private Timer timer;
    public Sprite openDoor;
    public Sprite closeDoor;
    private ZKey currentZKey;

    private bool nearDoor = false;
    private bool isDoorOpen = false;
    private bool activeDoor = false;

    void Awake()
    {
        playerManager = FindAnyObjectByType<PlayerManager>();
        timer = FindAnyObjectByType<Timer>();
        stageReset = FindAnyObjectByType<StageReset>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        StartCoroutine(WaitForPlayerObj());
        nearDoor = false;
        isDoorOpen = false;
        activeDoor = false;
    }
    private IEnumerator WaitForPlayerObj()
    {
        while (playerManager.player == null)
        {
            yield return null;
        }

        player = playerManager.GetPlayerObj();
        playerMovement = FindAnyObjectByType<Movement>();
        Debug.Log("ExitDoor : player 오브젝트 취득 완료");
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
            Debug.Log("탈출 완료");

            if (currentZKey != null)
            {
                currentZKey.Hide();
                currentZKey = null;
            }
        }
    }
}
