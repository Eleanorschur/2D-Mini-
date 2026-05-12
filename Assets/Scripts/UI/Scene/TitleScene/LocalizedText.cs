using UnityEngine;
using TMPro;

public class LocalizedText : MonoBehaviour
{
    [TextArea]
    public string koreanText;

    [TextArea]
    public string englishText;

    private TextMeshProUGUI textUI;   // textUI : 실제로 글자를 화면에 그리는 TextMeshProUGUI 컴포넌트 제어하기 위해 선언한 변수

    private void Awake()
    {
        textUI = GetComponent<TextMeshProUGUI>();
        // LocalizedText.cs 가 붙어있는 오브젝트에서 TextMeshProUGUI 컴포넌트 찾아 textUI 변수에 할당.
    }

    public void UpdateText()
    {
        if ( LanguageManager.Instance.currentLanguage == LanguageManager.Language.Korean) 
        {
            textUI.text = koreanText;
        }
        else
        {
            textUI.text = englishText;
        }

    }

}
