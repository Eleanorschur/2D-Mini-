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

    public void OnClickKorean()
    {
        SetButtonVisual(LanguageManager.Language.Korean);
        LanguageManager.Instance.SetKorean();
    }

    public void OnClickEnglish()
    {
        SetButtonVisual(LanguageManager.Language.English);
        LanguageManager.Instance.SetEnglish();
    }

    private void Start()
    {
        int savedLanguage = PlayerPrefs.GetInt("Language", 0);

        if (savedLanguage == 0)
            SetButtonVisual(LanguageManager.Language.Korean);
        else
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