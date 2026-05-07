using UnityEngine;

public class ExitDoor : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private StageReset stageReset;
    private Movement playerMovement;
    private Timer timer;
    public Sprite openDoor;
    public Sprite closeDoor;
    private ZKey Zkey;
    private ZKey currentZKey;

    private bool nearDoor = false;
    private bool isDoorOpen = false;
    private bool activeDoor = false;

    void Awake()
    {
        Zkey = FindAnyObjectByType<ZKey>();
        timer = FindAnyObjectByType<Timer>();
        stageReset = FindAnyObjectByType<StageReset>();
        playerMovement = FindAnyObjectByType<Movement>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        nearDoor = false;
        isDoorOpen = false;
        activeDoor = false;
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

            if (currentZKey != null)
            {
                currentZKey.Hide();
                currentZKey = null;
            }
        }
    }
}
