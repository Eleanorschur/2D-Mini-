using UnityEngine;

public class StatusCheck : MonoBehaviour
{
    private TransformCheck transformCheck;
    private SpriteChange spriteChange;

    [SerializeField] private int currentStatus = 0;
    public int CurrentStatus => currentStatus;

    public bool isFalling = false;

    void Awake()
    {
        transformCheck = GetComponent<TransformCheck>();
        spriteChange = GetComponent<SpriteChange>();
    }

    void Start()
    {
        currentStatus = 0;
    }

    public void ChangeForm(int status)
    {
        Debug.Log("ChangeForm" + status);
        currentStatus = status;
        spriteChange.ChangeForm(status);
    }

    public void LateUpdate()
    {
        if (transformCheck == null) return;

        isFalling = transformCheck.IsFalling;
    }
}
