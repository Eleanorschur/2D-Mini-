using UnityEngine;
using System.Collections;

public class Switch : MonoBehaviour
{
    private GameObject player;
    private PlayerManager playerManager;
    private PlatformManager platformManager;
    private Movement playerMovement;
    private StatusCheck statusCheck;
    private ZKey currentZKey;

    public int switchNumber;

    private bool nearSwitch;

    void Awake()
    {
        playerManager = FindAnyObjectByType<PlayerManager>();
        platformManager = FindAnyObjectByType<PlatformManager>();
        statusCheck = FindAnyObjectByType<StatusCheck>();
    }

    void Start()
    {
        StartCoroutine(WaitForPlayerObj());
        nearSwitch = false;
    }
    private IEnumerator WaitForPlayerObj()
    {
        while (playerManager.player == null)
        {
            yield return null;
        }

        player = playerManager.GetPlayerObj();
        playerMovement = FindAnyObjectByType<Movement>();
        Debug.Log("Switch : player 오브젝트 취득 완료");
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
