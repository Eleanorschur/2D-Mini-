using UnityEngine;

public class StatusCheck : MonoBehaviour
{
    private PlayerManager playerManager;
    private TransformCheck transformCheck;
    private SpriteChange spriteChange;

    [SerializeField] private int currentStatus = 0;
    public int CurrentStatus => currentStatus;

    public bool isFalling = false;

    void Awake()
    {
        playerManager = GetComponentInParent<PlayerManager>();
    }

    void OnEnable()
    {
        if (playerManager != null)
            playerManager.PlayerLoadComplete += UpdatePlayerReference;
    }

    void UpdatePlayerReference()
    {
        GameObject newPlayer = playerManager.GetPlayerObj();
        transformCheck = newPlayer.GetComponent<TransformCheck>();
        spriteChange = newPlayer.GetComponent<SpriteChange>();
    }

    void Start()
    {
        currentStatus = 0;
    }

    public void ChangeForm(int status)
    {
        if (spriteChange == null)
        {
            Debug.LogWarning("SpriteChange 참조가 아직 준비되지 않았습니다.");
            return;
        }

        currentStatus = status;
        spriteChange.ChangeForm(status);
    }

    public void LateUpdate()
    {
        if (transformCheck == null) return;

        isFalling = transformCheck.IsFalling;
    }
}
