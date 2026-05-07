using UnityEngine;
using UnityEngine.EventSystems;

public class Movement : MonoBehaviour
{
    private PlayerData playerData;
    private Rigidbody2D rb;
    private CapsuleCollider2D cd;

    public LayerMask groundLayer;
    private Vector3 originScale;
    private Vector3 leftScale;
    private Vector3 rightScale;

    private bool moveLock = false;
    private float rayDistance = 0.1f;

    private float moveDir;
    public float MoveDir => moveDir;

    private bool isGrounded = false;
    public bool IsGrounded => isGrounded;

    private bool isLanding = false;
    public bool IsLanding => isLanding; 

    void Awake()
    {
        playerData = GetComponent<PlayerData>();
        rb = GetComponent<Rigidbody2D>();
        cd = GetComponent<CapsuleCollider2D>();
    }

    void Start()
    {
        rb.freezeRotation = true;
        rb.gravityScale = 4;
        moveLock = false;

        originScale = transform.localScale;
        leftScale = new Vector3(-originScale.x, originScale.y, originScale.z);
        rightScale = new Vector3(originScale.x, originScale.y, originScale.z);
    }

    void Update()
    {
        if (moveLock) return;

        moveDir = Input.GetAxisRaw("Horizontal");

        CheckGrounded();

        //if (Input.GetButtonDown("Jump") && isGrounded)
        //    Jump();

        // 05.01 점프 단축키를 스페이스바에서 X키로 변경
        if (Input.GetKeyDown(KeyCode.X) && isGrounded)
            Jump();


        if (Input.GetKeyDown(KeyCode.X))
        {
            if (rb.linearVelocity.y > 0)
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y);
        }
    }

    private void FixedUpdate()
    {
        if (moveLock)
        {
            if (rb.linearVelocity != Vector2.zero)
                rb.linearVelocity = Vector2.zero;
            return;
        }

        if ( ! isLanding)
            Move();
    }

    private void CheckGrounded()
    {
        Bounds bounds = cd.bounds;

        RaycastHit2D hit = Physics2D.BoxCast(bounds.center, new Vector2(bounds.size.x * 0.9f, bounds.size.y), 0f, Vector2.down, rayDistance, groundLayer);

        isGrounded = hit.collider != null;
    }

    private void Move()
    {
        if (moveDir > 0)
            transform.localScale = rightScale;
        else if (moveDir < 0)
            transform.localScale = leftScale;

        rb.linearVelocity = new Vector2(moveDir * playerData.playerMoveSpeed, rb.linearVelocity.y);
    }

    private void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, playerData.playerJumpForce);
    }

    public void MoveLock(bool locking)
    {
        moveLock = locking;
    }
}
