using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleUI : MonoBehaviour
{
    [SerializeField] private SettingPanelAnimator settingPanelAnimator;
    [SerializeField] private GameObject blurOverlay;

    private void Start()
    {
        blurOverlay.SetActive(false);
        settingPanelAnimator.HideImmediately();
    }

    public void OnClickStart()
    {
        SceneManager.LoadScene("3_StageSelect");
    }

    public void OnClickSettings()
    {
        blurOverlay.SetActive(true);
        settingPanelAnimator.Open();
    }

    public void OnClickCloseSettings()
    {
        settingPanelAnimator.Close();
        blurOverlay.SetActive(false);
    }

    public void HideBlurOverlay()
    {
        blurOverlay.SetActive(false);
    }


    public void OnClickQuit()
    {
#if UNITY_EDITOR
        // 유니티 에디터에서 실행 중일 때
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // 실제 빌드된 게임에서 실행 중일 때
        Application.Quit();
#endif
    }
}
