using Unity.Cinemachine;
using UnityEngine;

public class CameraResolutionScaler : MonoBehaviour
{
    [Header("Cinemachine")]
    [SerializeField] private CinemachineCamera cinemachineCamera;
    [SerializeField] private CinemachinePositionComposer positionComposer;

    [Header("Base Camera Setting")]
    [SerializeField] private float baseOrthographicSize = 8.4375f;
    [SerializeField] private float baseScreenHeight = 1080f;

    [Header("Target Offset")]
    [SerializeField] private Vector3 normalTargetOffset = new Vector3(0f, 0f, 0f);
    [SerializeField] private Vector3 lowResolutionTargetOffset = new Vector3(0f, 1.5f, 0f);

    private void Awake()
    {
        if (cinemachineCamera == null)
            cinemachineCamera = GetComponent<CinemachineCamera>();

        if (positionComposer == null)
            positionComposer = GetComponent<CinemachinePositionComposer>();
    }

    private void Start()
    {
        ApplyCameraSize();
    }

    public void ApplyCameraSize()
    {
        if (cinemachineCamera == null)
        {
            Debug.LogWarning("CinemachineCamera가 연결되지 않았습니다.");
            return;
        }

        float heightRatio = baseScreenHeight / Screen.height;

        cinemachineCamera.Lens.OrthographicSize =
            baseOrthographicSize * heightRatio;

        if (positionComposer != null)
        {
            bool isWindowMode = Screen.fullScreenMode == FullScreenMode.Windowed;

            if (isWindowMode && Screen.width == 1280 && Screen.height == 720)
            {
                positionComposer.TargetOffset = lowResolutionTargetOffset;
            }
            else
            {
                positionComposer.TargetOffset = normalTargetOffset;
            }
        }
        Debug.Log($"현재 해상도: {Screen.width}x{Screen.height}");
        Debug.Log($"Orthographic Size 적용: {cinemachineCamera.Lens.OrthographicSize}");
    }
}