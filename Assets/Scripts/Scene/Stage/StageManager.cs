using UnityEngine;
using UnityEngine.SceneManagement;

public class StageManager : MonoBehaviour
{
    private StageReset stageReset;
    private Timer timer;

    [Header("Option UI")]
    [SerializeField] private GameObject settingPanel;
    [SerializeField] private GameObject blurOverlay;
    [SerializeField] private SettingPanelAnimator settingPanelAnimator;

    private bool isSettingsOpen = false;

    private void Awake()
    {
        stageReset = GetComponent<StageReset>();
        AutoFindOptionReferences();
    }

    private void Start()
    {
        timer = FindAnyObjectByType<Timer>();

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
            Debug.Log($"ESC 감지 / 오브젝트: {gameObject.name} / ID: {GetInstanceID()} / 씬: {gameObject.scene.name}");
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

        AutoFindOptionReferences();

        if (isSettingsOpen)
            return;

        Debug.Log($"옵션창 열기 / 오브젝트: {gameObject.name} / ID: {GetInstanceID()} / 씬: {gameObject.scene.name}");

        isSettingsOpen = true;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        if (stageReset != null)
            stageReset.ResetLock(true);

        if (blurOverlay != null)
            blurOverlay.SetActive(true);

        if (settingPanel != null)
            settingPanel.SetActive(true);

        if (settingPanelAnimator != null)
        {
            settingPanelAnimator.Open();
        }
        if (timer != null)
            timer.PauseTimer();
        else
        {
            Debug.LogError("SettingPanelAnimator를 찾지 못했습니다.");
        }
    }

    public void CloseSettings()
    {

        AutoFindOptionReferences();

        if (!isSettingsOpen)
            return;

        Debug.Log("옵션창 닫기");

        isSettingsOpen = false;
      
        if (timer != null)
        timer.ResumeTimer();

        if (settingPanelAnimator != null)
            settingPanelAnimator.Close();

        if (blurOverlay != null)
            blurOverlay.SetActive(false);
       
        if (settingPanel != null)
            settingPanel.SetActive(false);


        if (stageReset != null)
            stageReset.ResetLock(false);

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