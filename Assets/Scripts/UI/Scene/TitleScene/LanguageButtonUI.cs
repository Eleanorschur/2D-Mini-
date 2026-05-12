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
        koreanButtonImage.sprite = selectedSprite;
        englishButtonImage.sprite = normalSprite;

        LanguageManager.Instance.SetKorean();
    }

    public void onClickEnglish()
    {
        koreanButtonImage.sprite = normalSprite;
        englishButtonImage.sprite = selectedSprite;

        LanguageManager.Instance.SetEnglish();
    }
    
    private void Start()
    {
        int savedLanguage = PlayerPrefs.GetInt("Language", 0); // Language 라는 이름의 정수가 없다면 그냥 0 이라 치고 가져와. 

        if (savedLanguage == 0)
            OnClickKorean();
        else
            onClickEnglish();
    }
}
