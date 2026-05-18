using UnityEngine;
using UnityEngine.UI;

public class LanguageButtonUI : MonoBehaviour
{
    [Header("Button Images")]
    [SerializeField] private Image koreanButtonImage;
    [SerializeField] private Image englishButtonImage;

    [Header("Sprites")]
    [SerializeField] private Sprite normalSprite;
    [SerializeField] private Sprite selectedSprite;

    private void Start()
    {
        if (LanguageManager.Instance == null)
        {
            Debug.LogWarning("LanguageManager.Instance가 없습니다.");
            return;
        }

        SetButtonVisual(LanguageManager.Instance.CurrentLanguage);
    }

    public void OnClickKorean()
    {
        if (LanguageManager.Instance == null)
            return;

        AudioManager.Instance?.PlayButtonSFX(); //05.16. AudioManager를 위해 추가
        LanguageManager.Instance.SetKorean();
        SetButtonVisual(LanguageManager.Language.Korean);
    }

    public void OnClickEnglish()
    {
        if (LanguageManager.Instance == null)
            return;

        AudioManager.Instance?.PlayButtonSFX(); //05.16. AudioManager를 위해 추가
        LanguageManager.Instance.SetEnglish();
        SetButtonVisual(LanguageManager.Language.English);
    }

    private void SetButtonVisual(LanguageManager.Language language)
    {
        if (language == LanguageManager.Language.Korean)
        {
            koreanButtonImage.sprite = selectedSprite;
            englishButtonImage.sprite = normalSprite;
        }
        else
        {
            koreanButtonImage.sprite = normalSprite;
            englishButtonImage.sprite = selectedSprite;
        }
    }
}