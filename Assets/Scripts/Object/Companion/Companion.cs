using UnityEngine;

public class Companion : MonoBehaviour
{
    public PlayerManager playerManager;
    private ItemCheck itemCheck;
    private CompanionManager companionManager;
    private FollowPlayer followPlayer;
    private Movement playerMovement;
    private ZKey currentZKey;

    private bool nearCompanion;
    private bool activeCompanion;
    public bool ActiveCompanion => activeCompanion;

    private Vector3 originPos = Vector3.zero;
    private Vector3 originScale;
    private Vector3 leftScale;
    private Vector3 rightScale;

    private float offsetYpos = -0.5f;

    void Awake()
    {
        followPlayer = GetComponent<FollowPlayer>(); // 같은 계층
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
        companionManager = GetComponentInParent<CompanionManager>();

        originPos = this.gameObject.transform.position;
        originPos.y += offsetYpos;
        this.gameObject.transform.position = originPos;

        originScale = new Vector3(1, 1, 1);
        leftScale = new Vector3(-originScale.x, originScale.y, originScale.z);
        rightScale = new Vector3(originScale.x, originScale.y, originScale.z);
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
            nearCompanion = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if ( ! collision.CompareTag("Player")) return;

        nearCompanion = false;

        if (currentZKey != null)
        {
            currentZKey.Hide();
            currentZKey = null;
        }
    }

    void Update()
    {
        if (activeCompanion) return;

        CompanionStandDir();

        if (nearCompanion && playerMovement.IsGrounded && currentZKey == null)
        {
            currentZKey = ZKeyPool.Instance.GetZKey();
            currentZKey.Setup(this.transform);
        }

        if (currentZKey != null && !playerMovement.IsGrounded)
        {
            currentZKey.Hide();
            currentZKey = null;
        }

        if (Input.GetKeyDown(KeyCode.Z) && nearCompanion && playerMovement.IsGrounded)
        {
            activeCompanion = true;
            followPlayer.SetIndex(itemCheck.AddCompanionList(this.gameObject));

            if (currentZKey != null)
            {
                currentZKey.Hide();
                currentZKey = null;
            }
        }
    }

    private void CompanionStandDir()
    {
        float playerX =  playerManager.GetPlayerObj().transform.position.x;
        float companionX = this.gameObject.transform.position.x;

        if (playerX - companionX > 0)
            this.transform.localScale = rightScale;
        else if (playerX - companionX < 0)
            this.transform.localScale = leftScale;
    }

    public void CompanionReset(Vector3 position)
    {
        nearCompanion = false;
        activeCompanion = false;
        transform.position = position;
    }
}
