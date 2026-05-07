using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    public Sprite activeSprite;
    public Sprite deactiveSprite;
    private Movement playerMovement;
    private SpriteRenderer spriteRenderer;
    private CheckPointManager checkPointManager;

    private bool nearCheckPoint = false;
    private bool activateCheckPoint = false;

    private void Awake()
    {
        checkPointManager = GetComponentInParent<CheckPointManager>();
        playerMovement = FindAnyObjectByType<Movement>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        nearCheckPoint = false;
        activateCheckPoint = false;
        spriteRenderer.sprite = deactiveSprite;
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
