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
        stageReset = FindAnyObjectByType<StageReset>();
        cameraShaker = FindAnyObjectByType<CameraShaker>();
    }

    void Start()
    {

    }

    void LateUpdate()
    {
        if (transform.position.y > fallingYPos)
            return;

        isFalling = true;
        stageReset.DeadStageReset();
        cameraShaker.Shake();
    }
}
