using UnityEngine;
using TMPro;

// 싱글톤, enum, PlayeerPrefs, foreach, FindObjectByType 

// 현재 게임 언어 관리자
public class LanguageManager : MonoBehaviour
{
    public static LanguageManager Instance;

    public enum Language
    {
        Korean, // 0
        English // 1
    }

    public Language currentLanguage = Language.Korean;

    private void Awake()
    {
        Instance = this;
    }

    public void SetKorean()
    {
        Debug.Log("한국어 버튼 눌림");

        currentLanguage = Language.Korean;

        PlayerPrefs.SetInt("Language", 0);      //SetInt : 데이터 저장

        UpdateAllTexts();
    }

    public void SetEnglish()
    {
        Debug.Log("영어 버튼 눌림");

        currentLanguage = Language.English;

        PlayerPrefs.SetInt("Language", 1);

        UpdateAllTexts();
    }

    private void Start()
    {
        int savedLanguage = PlayerPrefs.GetInt("Language", 0);  //GetInt : 데이터 불러오기 

        currentLanguage = (Language)savedLanguage;

        UpdateAllTexts();
    }

    // 모든 UI 글자 새로고침 메서드 
    public void UpdateAllTexts()
    {
        LocalizedText[] texts = FindObjectsByType<LocalizedText>(FindObjectsSortMode.None);
        // FindObjectsByType<LocalizedText> : (제네릭) , 현재 씬에 활성화된 모든 객체 중에서 LocalizedText 스크립트 붙어있는 거 몽땅 찾아내 ! 
        // FindObjectsSortMode.None : 찾은 객체들 순서 정렬 안함. 

        foreach (LocalizedText text in texts)
        {
            text.UpdateText();
        }
    }
}