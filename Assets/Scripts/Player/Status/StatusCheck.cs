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
        switch (status)
        {
            case 0:
                currentStatus = 0;
                spriteChange.ChangeForm(0);
                break;

            case 1:
                currentStatus = 1;
                spriteChange.ChangeForm(1);
                break;

            case 2:
                currentStatus = 2;
                spriteChange.ChangeForm(2);
                break;
        }
    }

    public void LateUpdate()
    {
        isFalling = transformCheck.IsFalling;
    }
}
