using UnityEngine;

public class BackgroundFollowCamera : MonoBehaviour
{
    [Header("따라갈 카메라")]
    [SerializeField] private Camera targetCamera;


    [Header("위치 보정")]
    [SerializeField] private Vector2 offset = new Vector2(0f, -3.5f);

    [Header("Z 위치")]
    [SerializeField] private float zPosition = 1f;

    private void LateUpdate()
    {
        if (targetCamera == null)
        {
            targetCamera = Camera.main;
        }

        if (targetCamera == null)
            return;

        transform.position = new Vector3(
            targetCamera.transform.position.x + offset.x,
            targetCamera.transform.position.y + offset.y,
            zPosition
        );
    }
}