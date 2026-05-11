using UnityEngine;

public class TransformCheck : MonoBehaviour
{
    private PlayerManager playerManager;
    private StageReset stageReset;
    private CameraShaker cameraShaker;

    private bool isFalling = false;
    public bool IsFalling => isFalling;

    private float fallingYPos = -27f;

    void Awake()
    {
        playerManager = GetComponentInParent<PlayerManager>();
        stageReset = FindAnyObjectByType<StageReset>();
        cameraShaker = FindAnyObjectByType<CameraShaker>();
    }

    void OnEnable()
    {
        if (playerManager != null)
            playerManager.PlayerLoadComplete += UpdatePlayerReference;
    }

    void UpdatePlayerReference()
    {
        GameObject newPlayer = playerManager.GetPlayerObj();
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
