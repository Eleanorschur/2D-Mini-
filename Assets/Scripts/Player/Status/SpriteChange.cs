using UnityEngine;

public class SpriteChange : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public Sprite slimeGreen; 
    public Sprite slimeRed; 
    public Sprite slimeBlue; 

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>(); // 같은 계층
    }

    void Start()
    {
        ChangeForm(0);
    }

    public void ChangeForm(int status)
    {
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
