using UnityEngine;
using UnityEngine.UI;

public class CompanionIconUI : MonoBehaviour
{
    [Header("Icon Images")]
    [SerializeField] private Image[] companionIcons;

    [Header("Sprites")]
    [SerializeField] private Sprite inactiveSprite;
    [SerializeField] private Sprite activeSprite;

    private void Start()
    {
        ResetIcons();
    }

    public void UpdateIcon(int rescuedCount)
    {
        for (int i = 0; i < companionIcons.Length; i++)
        {
            if (companionIcons[i] == null)
                continue;

            companionIcons[i].sprite = i < rescuedCount ? activeSprite : inactiveSprite;
        }
    }

    public void ResetIcons()
    {
        UpdateIcon(0);
    }
}