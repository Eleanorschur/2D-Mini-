using UnityEngine;

public class Movement : MonoBehaviour
{
    private PlayerData playerData;
    private PlayerManager playerManager;
    private Rigidbody2D rigid2D;
    private CapsuleCollider2D col2D;

    public LayerMask groundLayer;
    private Vector3 originScale;
    private Vector3 leftScale;
    private Vector3 rightScale;

    private bool moveLock = false;
    private float rayDistance = 0.1f;

    // audioManager를 위한 코드 추가
    private bool wasGrounded = false;

    private float moveDir;
    public float MoveDir => moveDir;

    private bool isGrounded = false;
    public bool IsGrounded => isGrounded;

    private bool isLanding = false;
    public bool IsLanding => isLanding; 

    void Awake()
    {
        playerData = GetComponent<PlayerData>();
        rigid2D = GetComponent<Rigidbody2D>();
        col2D = GetComponent<CapsuleCollider2D>();
    }

    void Start()
    {
        playerManager = GetComponentInParent<PlayerManager>();

        rigid2D.freezeRotation = true;
        rigid2D.gravityScale = 4;
        moveLock = false;

        originScale = transform.localScale;
        leftScale = new Vector3(-originScale.x, originScale.y, originScale.z);
        rightScale = new Vector3(originScale.x, originScale.y, originScale.z);
    }

    void Update()
    {
        if (moveLock) return;

        moveDir = Input.GetAxisRaw("Horizontal");

        // audioManager를 위한 코드 추가
        wasGrounded = isGrounded;
        CheckGrounded();
        if (!wasGrounded && isGrounded)
            AudioManager.Instance.PlayLand();

        //if (Input.GetButtonDown("Jump") && isGrounded)
        //    Jump();

        // 05.01 ���� ����Ű�� �����̽��ٿ��� XŰ�� ����
        if (Input.GetKeyDown(KeyCode.X) && isGrounded)
            Jump();


        if (Input.GetKeyDown(KeyCode.X))
        {
            if (rigid2D.linearVelocity.y > 0)
                rigid2D.linearVelocity = new Vector2(rigid2D.linearVelocity.x, rigid2D.linearVelocity.y);
        }
    }

    private void FixedUpdate()
    {
        if (moveLock)
        {
            if (rigid2D.linearVelocity != Vector2.zero)
                rigid2D.linearVelocity = Vector2.zero;
            return;
        }

        if ( ! isLanding)
            Move();
    }

    private void CheckGrounded()
    {
        Bounds bounds = col2D.bounds;

        RaycastHit2D hit = Physics2D.BoxCast(bounds.center, new Vector2(bounds.size.x * 0.9f, bounds.size.y), 0f, Vector2.down, rayDistance, groundLayer);

        isGrounded = hit.collider != null;
    }

    private void Move()
    {
        if (moveDir > 0)
            transform.localScale = rightScale;
        else if (moveDir < 0)
            transform.localScale = leftScale;

        rigid2D.linearVelocity = new Vector2(moveDir * playerData.playerMoveSpeed, rigid2D.linearVelocity.y);
    }

    private void Jump()
    {
        rigid2D.linearVelocity = new Vector2(rigid2D.linearVelocity.x, playerData.playerJumpForce);
        AudioManager.Instance.PlayJump(); // audioManager를 위한 코드 추가
    }

    public void MoveLock(bool locking)
    {
        moveLock = locking;
    }
}
