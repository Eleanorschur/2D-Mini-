using UnityEngine;
using TMPro;

public class LocalizedText : MonoBehaviour
{
    [TextArea]
    public string koreanText;

    [TextArea]
    public string englishText;

    private TextMeshProUGUI textUI;

    private void Awake()
    {
        textUI = GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        UpdateText();
    }

    public void UpdateText()
    {
        if (textUI == null)
            textUI = GetComponent<TextMeshProUGUI>();

        if (textUI == null)
        {
            Debug.LogWarning($"{gameObject.name} 에 TextMeshProUGUI가 없습니다.");
            return;
        }

        if (LanguageManager.Instance == null)
        {
            Debug.LogWarning("LanguageManager.Instance가 없습니다.");
            return;
        }

        if (LanguageManager.Instance.currentLanguage == LanguageManager.Language.Korean)
        {
            textUI.text = koreanText;
        }
        else
        {
            textUI.text = englishText;
        }
    }
}