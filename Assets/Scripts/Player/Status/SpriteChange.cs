using UnityEngine;

public class SpriteChange : MonoBehaviour
{
    private PlayerManager playerManager;
    [SerializeField]private SpriteRenderer spriteRenderer;
    public Sprite slimeGreen; 
    public Sprite slimeRed; 
    public Sprite slimeBlue; 

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
        spriteRenderer = newPlayer.GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        
    }
    
    public void ChangeForm(int status)
    {
        if (spriteRenderer == null)
        {
            Debug.LogWarning("SpriteRenderer 참조가 아직 준비되지 않았습니다.");
            return;
        }

        switch (status)
        {
            case 0:
                spriteRenderer.sprite = slimeGreen;
                break;
            case 1:
                spriteRenderer.sprite = slimeRed;
                break;
            case 2:
                spriteRenderer.sprite = slimeBlue;
                break;
        }
    }
}
