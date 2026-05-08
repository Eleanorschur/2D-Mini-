using UnityEngine;
using System.Collections;

public class CheckPoint : MonoBehaviour
{
    private GameObject player;
    private PlayerManager playerManager;
    public Sprite activeSprite;
    public Sprite deactiveSprite;
    private Movement playerMovement;
    private SpriteRenderer spriteRenderer;
    private CheckPointManager checkPointManager;

    private bool nearCheckPoint = false;
    private bool activateCheckPoint = false;

    private void Awake()
    {
        playerManager = FindAnyObjectByType<PlayerManager>();
        checkPointManager = GetComponentInParent<CheckPointManager>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        StartCoroutine(WaitForPlayerObj());
        nearCheckPoint = false;
        activateCheckPoint = false;
        spriteRenderer.sprite = deactiveSprite;
    }

    private IEnumerator WaitForPlayerObj()
    {
        while (playerManager.player == null)
        {
            yield return null;
        }

        player = playerManager.GetPlayerObj();
        playerMovement = FindAnyObjectByType<Movement>();
        Debug.Log("CheckPoint : player 오브젝트 취득 완료");
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
        }
    }

    public void SetActiveCheckPoint(bool active)
    {
        activateCheckPoint = active;
        spriteRenderer.sprite = active ? activeSprite : deactiveSprite;
    }

    public void CheckPointReset()
    {
        nearCheckPoint = false;
        activateCheckPoint = false;
        spriteRenderer.sprite = deactiveSprite;
    }
}
