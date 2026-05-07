using UnityEngine;

public class Switch : MonoBehaviour
{
    private PlatformManager platformManager;
    private Movement playerMovement;
    private StatusCheck statusCheck;
    private ZKey Zkey;
    private ZKey currentZKey;

    public int switchNumber;

    private bool nearSwitch;

    void Awake()
    {
        playerMovement = FindAnyObjectByType<Movement>();
        platformManager = FindAnyObjectByType<PlatformManager>();
        statusCheck = FindAnyObjectByType<StatusCheck>();
        Zkey = FindAnyObjectByType<ZKey>();
    }

    void Start()
    {
        nearSwitch = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            nearSwitch = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if ( ! collision.CompareTag("Player")) return;

        nearSwitch = false;

        if (currentZKey != null)
        {
            currentZKey.Hide(); // 사용 완료 후 반납
            currentZKey = null;
        }
    }

    void Update()
    {
        if (nearSwitch && playerMovement.IsGrounded && currentZKey == null)
        {
            currentZKey = ZKeyPool.Instance.GetZKey();
            currentZKey.Setup(this.transform);
        }

        if (currentZKey != null && ! playerMovement.IsGrounded)
        {
            currentZKey.Hide();
            currentZKey = null;
        }

        if (Input.GetKeyDown(KeyCode.Z) && nearSwitch && playerMovement.IsGrounded)
        {
            statusCheck.ChangeForm(switchNumber);
            platformManager.SwitchingPlatformHide(switchNumber);

            if (currentZKey != null)
            {
                currentZKey.Hide();
                currentZKey = null;
            }
        }
    }

    public void SwitchReset()
    {
        nearSwitch = false;
    }
}
