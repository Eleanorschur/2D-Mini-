using UnityEngine;

public class Lever : MonoBehaviour
{
    public PlayerManager playerManager;
    private Movement playerMovement;
    private LeverManager leverManager;
    private SpriteRenderer spriteRenderer;
    private ItemCheck itemCheck;
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
        itemCheck = playerManager.GetPlayerObj().GetComponent<ItemCheck>();
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
            AudioManager.Instance?.PlayLeverSFX(); //05.16. AudioManager를 위해 추가
            leverManager.LeverAddCounter();
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
            itemCheck.AddLeverList(this.gameObject);

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
