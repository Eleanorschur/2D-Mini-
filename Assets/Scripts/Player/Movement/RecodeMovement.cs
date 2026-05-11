using UnityEngine;
using System.Collections.Generic;

public class RecodeMovement : MonoBehaviour
{
    public struct PlayerMove
    {
        public Vector2 Position;
        public Vector2 Dir;
        public bool IsJumping;

        public PlayerMove(Vector2 pos, Vector2 dir, bool jump)
        {
            Position = pos;
            Dir = dir;
            IsJumping = jump;
        }
    }

    private PlayerManager playerManager;
    private Rigidbody2D rigid2D;

    private Vector2 currentPos;
    private Vector2 previousPos;

    private List<PlayerMove> pathList = new List<PlayerMove>();
    private int maxStorage = 300;

    void Awake()
    {
        playerManager = GetComponentInParent<PlayerManager>();
        rigid2D = GetComponent<Rigidbody2D>();
        pathList.Capacity = maxStorage;
    }

    void OnEnable()
    {
        if (playerManager != null)
            playerManager.PlayerLoadComplete += UpdatePlayerReference;
    }

    void UpdatePlayerReference()
    {
        GameObject newPlayer = playerManager.GetPlayerObj();
        rigid2D = newPlayer.GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        pathList.Clear();
        previousPos = transform.position;
    }

    void FixedUpdate()
    {
        RecordPosition();
    }

    private void RecordPosition()
    {
        currentPos = transform.position;

        Vector2 moveDir = (currentPos - previousPos).normalized;

        bool isJumping = rigid2D.linearVelocityY > 0.1f;

        pathList.Add(new PlayerMove(currentPos, moveDir, isJumping));

        if (pathList.Count > maxStorage)
        {
            pathList.RemoveAt(0);
        }

        previousPos = currentPos;
    }

    public List<PlayerMove> GetRecode()
    {
        return pathList;
    }
}
