using UnityEngine;

public class LanguageManager : MonoBehaviour
{
    public static LanguageManager Instance;
    public Language CurrentLanguage => currentLanguage;

    public enum Language
    {
        Korean,
        English
    }

    public Language currentLanguage = Language.Korean;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        int savedLanguage = PlayerPrefs.GetInt("Language", 0);
        currentLanguage = (Language)savedLanguage;
    }

    private void Start()
    {
        UpdateAllTexts();
    }

    public void SetKorean()
    {
        currentLanguage = Language.Korean;
        PlayerPrefs.SetInt("Language", 0);
        PlayerPrefs.Save();

        UpdateAllTexts();
    }

    public void SetEnglish()
    {
        currentLanguage = Language.English;
        PlayerPrefs.SetInt("Language", 1);
        PlayerPrefs.Save();

        UpdateAllTexts();
    }

    public void UpdateAllTexts()
    {
        LocalizedText[] texts = FindObjectsByType<LocalizedText>(
            FindObjectsInactive.Include,
            FindObjectsSortMode.None
        );

        foreach (LocalizedText text in texts)
        {
            text.UpdateText();
        }
    }
}