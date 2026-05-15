#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public static class ResetStagePrefsOnStop
{
    static ResetStagePrefsOnStop()
    {
        EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
    }

    private static void OnPlayModeStateChanged(PlayModeStateChange state)
    {
        if (state == PlayModeStateChange.ExitingPlayMode)
        {
            PlayerPrefs.DeleteKey("Stage1_Stars");
            PlayerPrefs.DeleteKey("Stage2_Stars");
            PlayerPrefs.DeleteKey("Stage3_Stars");

            PlayerPrefs.DeleteKey("Stage1_Locked");
            PlayerPrefs.DeleteKey("Stage2_Locked");
            PlayerPrefs.DeleteKey("Stage3_Locked");

            PlayerPrefs.DeleteKey("SelectedStage");

            PlayerPrefs.Save();

            Debug.Log("에디터 플레이 종료: 스테이지 저장값 초기화 완료");
        }
    }
}
#endif