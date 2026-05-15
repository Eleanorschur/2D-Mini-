using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    private PlayerManager playerManager;
    private Movement playerMovement;
    private StatusCheck statusCheck;
    private ItemCheck itemCheck;
    private SpriteRenderer spriteRenderer;
    private CheckPointManager checkPointManager;

    public Sprite activeSprite;
    public Sprite deactiveSprite;

    private bool nearCheckPoint = false;
    private bool activateCheckPoint = false;

    [SerializeField]private int saveStatus = 0;
    public int SaveStatus => saveStatus;

    private void Awake()
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
        checkPointManager = GetComponentInParent<CheckPointManager>();

        nearCheckPoint = false;
        activateCheckPoint = false;
        spriteRenderer.sprite = deactiveSprite;
    }

    private void PlayerLoadComplete()
    {
        playerMovement = playerManager.GetPlayerObj().GetComponent<Movement>();
        statusCheck = playerManager.GetPlayerObj().GetComponent<StatusCheck>();
        itemCheck = playerManager.GetPlayerObj().GetComponent<ItemCheck>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            nearCheckPoint = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        nearCheckPoint = false;
    }

    private void Update()
    {
        if (nearCheckPoint && playerMovement.IsGrounded && ! activateCheckPoint)
        {
            activateCheckPoint = true;
            spriteRenderer.sprite = activeSprite;
            checkPointManager.SetFinalCheckPoint(this.gameObject);
            itemCheck.ListClear();
        }
    }

    public void SetActiveCheckPoint(bool active)
    {
        activateCheckPoint = active;
        spriteRenderer.sprite = active ? activeSprite : deactiveSprite;

        if (active && statusCheck != null)
        {
            checkPointManager.SetPlayerStatus(statusCheck.CurrentStatus);
        }
    }

    public void CheckPointReset()
    {
        nearCheckPoint = false;
        activateCheckPoint = false;
        spriteRenderer.sprite = deactiveSprite;
    }
}
