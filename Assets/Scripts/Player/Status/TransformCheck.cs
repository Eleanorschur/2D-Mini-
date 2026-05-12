using UnityEngine;

public class TransformCheck : MonoBehaviour
{
    private StageReset stageReset;
    private CameraShaker cameraShaker;

    private bool isFalling = false;
    public bool IsFalling => isFalling;

    private float fallingYPos = -27f;

    void Awake()
    {

    }

    void Start()
    {
        stageReset = FindAnyObjectByType<StageReset>();
        cameraShaker = FindAnyObjectByType<CameraShaker>();
    }

    void LateUpdate()
    {
        if (stageReset == null || cameraShaker == null)
            return;

        if (transform.position.y > fallingYPos)
            return;

        isFalling = true;
        stageReset.DeadStageReset();
        cameraShaker.Shake();
    }
}
