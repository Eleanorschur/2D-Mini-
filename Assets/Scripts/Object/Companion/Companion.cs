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

    private Vector3 pos = Vector3.zero;
    private Vector3 originScale = new Vector3(1, 1, 1);
    private Vector3 reverseScale = new Vector3(-1, 1, 1);

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
        pos = this.gameObject.transform.position;
        pos.y += offsetYpos;
        this.gameObject.transform.position = pos;

        companionManager = GetComponentInParent<CompanionManager>();
        this.gameObject.transform.localScale = reverseScale; // 시작 시 컴패니언 뒤돌기
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
            this.gameObject.transform.localScale = originScale; // 구출 시 다시 원상태로 스케일값 복귀

            if (currentZKey != null)
            {
                currentZKey.Hide();
                currentZKey = null;
            }
        }
    }

    public void CompanionReset(Vector3 position)
    {
        nearCompanion = false;
        activeCompanion = false;
        transform.position = position;
    }
}
