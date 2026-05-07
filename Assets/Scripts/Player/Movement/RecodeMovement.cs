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

    private Rigidbody2D rigid2D;

    private Vector2 currentPos;
    private Vector2 previousPos;

    private List<PlayerMove> pathList = new List<PlayerMove>();
    [SerializeField] private int maxStorage = 500;

    //private float recordTimer = 0f;
    //private const float recordInterval = 0.1f;

    void Awake()
    {
        rigid2D = GetComponent<Rigidbody2D>();
        pathList.Capacity = maxStorage;
    }

    void Start()
    {
        pathList.Clear();
        previousPos = transform.position;
    }

    void FixedUpdate()
    {
        //recordTimer += Time.fixedDeltaTime;

        //if (recordTimer >= recordInterval)
        //{
        //    RecordPosition();
        //    recordTimer = 0f;
        //}

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
