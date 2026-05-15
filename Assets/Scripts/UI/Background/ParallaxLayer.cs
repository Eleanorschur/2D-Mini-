using UnityEngine;

public class ParallaxLayer : MonoBehaviour
{
    [Header("카메라 참조")]
    private Transform cameraTransform;
    private Vector3 lastCameraPosition;

    [Header("패럴랙스 속도 계수 (0 ~ 1)")]
    [Tooltip("0이면 카메라와 완전히 똑같이 움직임(안 움직이는 것처럼 보임), 1이면 카메라 이동을 전혀 안 따라감")]
    [SerializeField] private float parallaxFactorX = 0.5f;
    [SerializeField] private float parallaxFactorY = 0.1f; // 세로는 보통 작게 줍니다.

    [Header("무한 루프 배경 설정 (옵션)")]
    [SerializeField] private bool infiniteHorizontal = true;
    private float textureUnitSizeX;

    void Start()
    {
        // 메인 카메라(시네머신이 조종하는 뇌)의 Transform을 가져옵니다.
        if (Camera.main != null)
        {
            cameraTransform = Camera.main.transform;
            lastCameraPosition = cameraTransform.position;
        }

        // 가로 무한 배경 기능을 위해 자식 중 첫 번째 SpriteRenderer의 가로 크기를 측정합니다.
        if (infiniteHorizontal)
        {
            SpriteRenderer spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                // 스프라이트의 실제 월드 크기 가로폭 계산
                textureUnitSizeX = spriteRenderer.sprite.bounds.size.x * spriteRenderer.transform.localScale.x;
            }
        }
    }

    // 시네머신 카메라는 주로 LateUpdate 포스트 프로세스에서 움직이므로 
    // 배경 이동도 LateUpdate에서 처리해야 화면이 떨리지 않습니다.
    void LateUpdate()
    {
        if (cameraTransform == null) return;

        // 1. 카메라가 이번 프레임에 움직인 거리(Delta) 계산
        Vector3 deltaMovement = cameraTransform.position - lastCameraPosition;

        // 2. 패럴랙스 계수를 곱하여 배경 이동 거리 계산
        // Factor가 0.7이면 카메라는 10 이동할 때 배경은 7만큼 카메라를 따라가므로 결과적으로 3만큼 뒤처져 보입니다(느리게 움직임).
        float moveX = deltaMovement.x * parallaxFactorX;
        float moveY = deltaMovement.y * parallaxFactorY;

        transform.position += new Vector3(moveX, moveY, 0);

        // 다음 프레임을 위해 현재 카메라 위치 저장
        lastCameraPosition = cameraTransform.position;

        // 3. 무한 루프 스크롤 처리 (플레이어가 배경 끝으로 가려고 하면 배경을 워프시킴)
        if (infiniteHorizontal && textureUnitSizeX > 0)
        {
            float offsetPositionX = cameraTransform.position.x - transform.position.x;

            if (Mathf.Abs(offsetPositionX) >= textureUnitSizeX)
            {
                float relativeOffset = offsetPositionX % textureUnitSizeX;
                transform.position = new Vector3(cameraTransform.position.x - relativeOffset, transform.position.y, transform.position.z);
            }
        }
    }
}