using UnityEngine;
using System.Collections;

public class Lever : MonoBehaviour
{
    private GameObject player;
    private PlayerManager playerManager;
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
        playerManager = FindAnyObjectByType<PlayerManager>();
        leverManager = GetComponentInParent<LeverManager>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        StartCoroutine(WaitForPlayerObj());
        spriteRenderer.sprite = upSprite;
        activeLever = false;
        nearLever = false;
    }
    private IEnumerator WaitForPlayerObj()
    {
        while (playerManager.player == null)
        {
            yield return null;
        }

        player = playerManager.GetPlayerObj();
        playerMovement = FindAnyObjectByType<Movement>();
        Debug.Log("Lever : player 오브젝트 취득 완료");
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
            currentZKey.Hide(); // 사용 완료 후 반납
            currentZKey = null;
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
            spriteRenderer.sprite = downSprite;
            leverManager.leverAddCounter();

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
