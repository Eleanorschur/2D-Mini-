using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleUI : MonoBehaviour
{
    [SerializeField] private SettingPanelAnimator settingPanelAnimator;
    [SerializeField] private GameObject blurOverlay;

    [SerializeField] private GameObject settingPanel;
    private void Start()
    {
        blurOverlay.SetActive(false);
        settingPanelAnimator.HideImmediately();
    }

    private void PlayButtonSFX() => AudioManager.Instance?.PlayButtonSFX(); //05.16. AudioManager를 위해 추가

    public void OnClickStart()
    {
        PlayButtonSFX(); //05.16. AudioManager를 위해 추가
        SceneManager.LoadScene("3_StageSelect");
    }

    public void OnClickSettings()
    {
        PlayButtonSFX(); //05.16. AudioManager를 위해 추가
        blurOverlay.SetActive(true);
        settingPanelAnimator.Open();
    }

    public void OnClickCloseSettings()
    {
        PlayButtonSFX();

        settingPanelAnimator.Close();

        blurOverlay.SetActive(false);

        if (settingPanel != null)
            settingPanel.SetActive(false);
    }

    public void HideBlurOverlay()
    {
        blurOverlay.SetActive(false);
    }


    public void OnClickQuit()
    {
        PlayButtonSFX(); //05.16. AudioManager를 위해 추가
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
