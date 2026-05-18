#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

[InitializeOnLoad]
public class ResetStagePrefsOnStop : IPreprocessBuildWithReport
{
    public int callbackOrder => 0;

    static ResetStagePrefsOnStop()
    {
        EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
        EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
    }

    private static void OnPlayModeStateChanged(PlayModeStateChange state)
    {
        // 에디터 플레이 종료 시 초기화
        if (state == PlayModeStateChange.EnteredEditMode)
        {
            ResetStagePrefs("에디터 플레이 종료 후");
        }
    }

    public void OnPreprocessBuild(BuildReport report)
    {
        // 빌드 시작 직전 초기화
        ResetStagePrefs("빌드 시작 전");
    }

    private static void ResetStagePrefs(string reason)
    {
        // 스테이지 관련 값 삭제
        PlayerPrefs.DeleteKey("Stage1_Stars");
        PlayerPrefs.DeleteKey("Stage2_Stars");
        PlayerPrefs.DeleteKey("Stage3_Stars");

        PlayerPrefs.DeleteKey("Stage1_Locked");
        PlayerPrefs.DeleteKey("Stage2_Locked");
        PlayerPrefs.DeleteKey("Stage3_Locked");

        PlayerPrefs.DeleteKey("SelectedStage");

        PlayerPrefs.Save();

        Debug.Log($"{reason} PlayerPrefs 전체 초기화 완료");
    }
}
#endif