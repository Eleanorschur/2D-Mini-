using UnityEngine;

public class Switch : MonoBehaviour
{
    private PlayerManager playerManager;
    private SwitchRed switchRed;
    private SwitchBlue switchBlue;
    private Movement playerMovement;
    private StatusCheck statusCheck;
    private ZKey currentZKey;

    public int switchNumber;
    private bool nearSwitch;

    void Awake()
    {
    }

    void OnEnable()
    {
        playerManager = FindAnyObjectByType<PlayerManager>();

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
        switchRed = GetComponentInParent<SwitchRed>();
        switchBlue = GetComponentInParent<SwitchBlue>();
        nearSwitch = false;
    }

    private void PlayerLoadComplete()
    {
        playerMovement = playerManager.GetPlayerObj().GetComponent<Movement>();
        statusCheck = playerManager.GetPlayerObj().GetComponent<StatusCheck>();
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

            if (switchNumber == 1)
                switchRed.Switching();
            else if (switchNumber == 2)
                switchBlue.Switching();

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
