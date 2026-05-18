using UnityEngine;

public class ParallaxLayer : MonoBehaviour
{
    [Header("카메라 참조")]
    private Transform cameraTransform;
    private Camera mainCamera;
    private Vector3 lastCameraPosition;

    [Header("패럴랙스 속도 계수 (0 ~ 1)")]
    [SerializeField] private float parallaxFactorX = 0.5f;
    [SerializeField] private float parallaxFactorY = 0.1f;

    [Header("무한 루프 배경 설정")]
    [SerializeField] private bool infiniteHorizontal = true;

    [Tooltip("화면 밖으로 얼마나 더 나간 뒤 재배치할지")]
    [SerializeField] private float loopPadding = 3f;

    private SpriteRenderer spriteRenderer;

    [Header("자동 스크롤")]
    [Tooltip("양수면 오른쪽으로 이동, 음수면 왼쪽으로 이동")]
    [SerializeField] private float autoScrollSpeedX = 0f;

    private void Start()
    {
        mainCamera = Camera.main;

        if (mainCamera != null)
        {
            cameraTransform = mainCamera.transform;
            lastCameraPosition = cameraTransform.position;
        }

        spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer == null)
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void LateUpdate()
    {
        if (cameraTransform == null || mainCamera == null) return;

        // 1. 자동 스크롤
        if (autoScrollSpeedX != 0f)
        {
            transform.position += new Vector3(autoScrollSpeedX * Time.deltaTime, 0f, 0f);
        }

        // 2. 카메라 이동에 따른 패럴랙스
        Vector3 deltaMovement = cameraTransform.position - lastCameraPosition;

        float moveX = deltaMovement.x * parallaxFactorX;
        float moveY = deltaMovement.y * parallaxFactorY;

        transform.position += new Vector3(moveX, moveY, 0f);

        lastCameraPosition = cameraTransform.position;

        // 3. 화면 밖 기준으로 자연스럽게 재배치
        if (infiniteHorizontal && spriteRenderer != null)
        {
            LoopByCameraEdge();
        }
    }

    private void LoopByCameraEdge()
    {
        float cameraHalfWidth = mainCamera.orthographicSize * mainCamera.aspect;

        float cameraLeft = cameraTransform.position.x - cameraHalfWidth;
        float cameraRight = cameraTransform.position.x + cameraHalfWidth;

        float spriteHalfWidth = spriteRenderer.bounds.extents.x;

        float spriteLeft = spriteRenderer.bounds.min.x;
        float spriteRight = spriteRenderer.bounds.max.x;

        // 오른쪽으로 흘러가는 구름
        // 완전히 오른쪽 밖으로 나가면 왼쪽 밖으로 이동
        if (autoScrollSpeedX > 0f)
        {
            if (spriteLeft > cameraRight + loopPadding)
            {
                float newX = cameraLeft - spriteHalfWidth - loopPadding;
                transform.position = new Vector3(newX, transform.position.y, transform.position.z);
            }
        }
        // 왼쪽으로 흘러가는 구름
        // 완전히 왼쪽 밖으로 나가면 오른쪽 밖으로 이동
        else if (autoScrollSpeedX < 0f)
        {
            if (spriteRight < cameraLeft - loopPadding)
            {
                float newX = cameraRight + spriteHalfWidth + loopPadding;
                transform.position = new Vector3(newX, transform.position.y, transform.position.z);
            }
        }
    }
}