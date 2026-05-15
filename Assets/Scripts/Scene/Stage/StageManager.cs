using UnityEngine;
using UnityEngine.SceneManagement;

public class StageManager : MonoBehaviour
{
    private Timer timer;
    private StageReset stageReset;

    [Header("Option UI")]
    [SerializeField] private GameObject settingPanel;
    [SerializeField] private GameObject blurOverlay;
    [SerializeField] private SettingPanelAnimator settingPanelAnimator;

    private bool pauseGame = false;
    private bool isSettingsOpen = false;

    private void Awake()
    {
        stageReset = GetComponent<StageReset>();
        AutoFindOptionReferences();
    }

    private void Start()
    {
        timer = FindAnyObjectByType<Timer>();

        pauseGame = false;
        isSettingsOpen = false;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        Time.timeScale = 1f;

        CloseOptionImmediately();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleSettings();
        }
    }

    private void AutoFindOptionReferences()
    {
        if (settingPanel == null)
        {
            GameObject foundSettingPanel = GameObject.Find("SettingPanel");

            if (foundSettingPanel != null)
                settingPanel = foundSettingPanel;
        }

        if (blurOverlay == null)
        {
            GameObject foundBlurOverlay = GameObject.Find("BlurOverlay");

            if (foundBlurOverlay != null)
                blurOverlay = foundBlurOverlay;
        }

        if (settingPanelAnimator == null && settingPanel != null)
        {
            settingPanelAnimator = settingPanel.GetComponent<SettingPanelAnimator>();
        }
    }

    private void CloseOptionImmediately()
    {
        if (settingPanel != null)
            settingPanel.SetActive(true);

        if (settingPanelAnimator != null)
            settingPanelAnimator.HideImmediately();

        if (blurOverlay != null)
            blurOverlay.SetActive(false);

        isSettingsOpen = false;
        pauseGame = false;
    }

    public void ToggleSettings()
    {
        if (isSettingsOpen)
        {
            CloseSettings();
        }
        else
        {
            OpenSettings();
        }
    }

    public void OpenSettings()
    {
        Debug.Log("옵션창 열기");

        AutoFindOptionReferences();

        if (isSettingsOpen)
            return;

        isSettingsOpen = true;
        pauseGame = true;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        if (stageReset != null)
            stageReset.ResetLock(true);

        if (timer != null)
            timer.PauseTimer();

        if (blurOverlay != null)
            blurOverlay.SetActive(true);

        if (settingPanel != null)
            settingPanel.SetActive(true);

        if (settingPanelAnimator != null)
        {
            settingPanelAnimator.Open();
        }
        else
        {
            Debug.LogError("SettingPanelAnimator를 찾지 못했습니다.");
        }

        Time.timeScale = 0f;
    }

    public void CloseSettings()
    {
        Debug.Log("옵션창 닫기");

        AutoFindOptionReferences();

        if (!isSettingsOpen)
            return;

        isSettingsOpen = false;
        pauseGame = false;

        Time.timeScale = 1f;

        if (settingPanelAnimator != null)
            settingPanelAnimator.Close();

        if (blurOverlay != null)
            blurOverlay.SetActive(false);

        if (stageReset != null)
            stageReset.ResetLock(false);

        if (timer != null)
            timer.ResumeTimer();

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void ReturnToTitle()
    {
        Debug.Log("타이틀 화면으로 이동");

        Time.timeScale = 1f;

        if (ScenePopTransitionManager.Instance != null)
        {
            ScenePopTransitionManager.Instance.LoadSceneWithPop("2_Title");
        }
        else
        {
            SceneManager.LoadScene("2_Title");
        }
    }
}