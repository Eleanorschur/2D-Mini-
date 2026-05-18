using UnityEngine;

public static class StagePrefsRuntimeReset
{
    private const string ResetDoneKey = "StagePrefs_Reset_Done_V1";

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void ResetStagePrefsOnFirstLaunch()
    {
        // 이미 한 번 초기화했으면 다시 초기화하지 않음
        if (PlayerPrefs.GetInt(ResetDoneKey, 0) == 1)
            return;

        PlayerPrefs.DeleteKey("Stage1_Stars");
        PlayerPrefs.DeleteKey("Stage2_Stars");
        PlayerPrefs.DeleteKey("Stage3_Stars");

        PlayerPrefs.DeleteKey("Stage1_Locked");
        PlayerPrefs.DeleteKey("Stage2_Locked");
        PlayerPrefs.DeleteKey("Stage3_Locked");

        PlayerPrefs.DeleteKey("SelectedStage");

        PlayerPrefs.SetInt(ResetDoneKey, 1);
        PlayerPrefs.Save();

        Debug.Log("빌드 실행 최초 1회 스테이지 저장값 초기화 완료");
    }
}