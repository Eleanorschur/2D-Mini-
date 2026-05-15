using UnityEngine;

public class Lever : MonoBehaviour
{
    public PlayerManager playerManager;
    private Movement playerMovement;
    private LeverManager leverManager;
    private SpriteRenderer spriteRenderer;
    public Sprite upSprite;
    public Sprite downSprite;
    private ZKey currentZKey;

    private bool nearLever;
    private bool activeLever;
    public bool ActiveLever => activeLever;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
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
        leverManager = GetComponentInParent<LeverManager>();

        LeverAct(false);
        activeLever = false;
        nearLever = false;
    }

    private void PlayerLoadComplete()
    {
        playerMovement = playerManager.GetPlayerObj().GetComponent<Movement>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            nearLever = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if ( ! collision.CompareTag("Player")) return;

        nearLever = false;

        if (currentZKey != null)
        {
            currentZKey.Hide(); // ��� �Ϸ� �� �ݳ�
            currentZKey = null;
        }
    }

    public void LeverAct(bool open)
    {
        if (this == null || spriteRenderer == null) return;

        spriteRenderer.sprite = open ? downSprite : upSprite;

        if (open)
        {
            AudioManager.Instance.PlayLever(); // audioManager를 위한 코드 추가
            leverManager.leverAddCounter();
        }
    }

    private void Update()
    {
        if (activeLever) return;

        if (nearLever && playerMovement.IsGrounded && currentZKey == null)
        {
            currentZKey = ZKeyPool.Instance.GetZKey();
            currentZKey.Setup(this.transform);
        }

        if (currentZKey != null && ! playerMovement.IsGrounded)
        {
            currentZKey.Hide();
            currentZKey = null;
        }

        if (Input.GetKeyDown(KeyCode.Z) && nearLever && playerMovement.IsGrounded)
        {
            activeLever = true;
            LeverAct(true);

            if (currentZKey != null)
            {
                currentZKey.Hide();
                currentZKey = null;
            }
        }
    }

    public void LeverReset()
    {
        spriteRenderer.sprite = upSprite;
        activeLever = false;
        nearLever = false;
    }
}
