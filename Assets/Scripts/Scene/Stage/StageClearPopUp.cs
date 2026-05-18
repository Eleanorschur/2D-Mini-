using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;
using UnityEngine.UI;

public class StageClearPopup : MonoBehaviour
{
    [SerializeField] private GameObject popupPanel;
    [SerializeField] private SettingPanelAnimator popupAnimator;

    [Header("UI Text")]
    [SerializeField] private TMP_Text elapsedTimeText;

    private Timer timer;

    [Header("Stage Control")]
    [SerializeField] private MapLoader mapLoader;
    [SerializeField] private ExitManager exitManager;
    [SerializeField] private StageReset stageReset;

    [Header("Stage Setting")]
    [SerializeField] private int finalStageNumber = 3;

    [Header("Ending Scene")]
    [SerializeField] private string happyEndingSceneName = "HappyEnding";

    [Header("Star UI")]
    [SerializeField] private Image[] starImages;

    [SerializeField] private Sprite emptyStarSprite;
    [SerializeField] private Sprite filledStarSprite;

    private CompanionManager companionManager;

    private bool isClearProcessing = false;

    private void Start()
    {
        if (mapLoader == null)
            mapLoader = FindAnyObjectByType<MapLoader>();

        if (exitManager == null)
            exitManager = FindAnyObjectByType<ExitManager>();

        if (stageReset == null)
            stageReset = FindAnyObjectByType<StageReset>();

        if (companionManager == null)
            companionManager = FindAnyObjectByType<CompanionManager>();

        if (timer == null)
            timer = FindAnyObjectByType<Timer>();

        if (popupAnimator != null)
            popupAnimator.HideImmediately();

        if (popupPanel != null)
            popupPanel.SetActive(false);

        isClearProcessing = false;
    }

    public void ShowPopup()
    {
        if (isClearProcessing)
            return;

        isClearProcessing = true;

        timer = FindAnyObjectByType<Timer>();

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        /*
         * 현재 스테이지가 마지막 스테이지라면
         * Stage Clear 팝업을 띄우지 않고 바로 HappyEnding 씬으로 이동
         */

        SaveStageProgress();

        int currentStageNumber = 1;

        if (mapLoader != null)
            currentStageNumber = mapLoader.CurrentStageNumber;
        else
            currentStageNumber = PlayerPrefs.GetInt("SelectedStage", 1);

        Debug.Log("현재 클리어한 스테이지 번호: " + currentStageNumber);

        if (currentStageNumber >= finalStageNumber)
        {
            Debug.Log("Stage3 클리어 - HappyEnding 씬으로 이동");

            Time.timeScale = 1f;

            LoadHappyEndingScene();
            return;
        }

        /*
         * 마지막 스테이지가 아니면
         * Stage1, Stage2처럼 기존 Stage Clear 팝업 표시
         */
        if (popupPanel != null)
            popupPanel.SetActive(true);

        // StageClearPopUpPanel이 켜진 직후, 현재 언어로 텍스트 다시 갱신
        if (LanguageManager.Instance != null)
            LanguageManager.Instance.UpdateAllTexts();

        UpdateStarUI();
        

        if (popupAnimator != null)
            popupAnimator.Open();

        Time.timeScale = 0f;

        StartCoroutine(UpdateElapsedTimeTextNextFrame());

        Debug.Log("Stage Clear Popup 열림");
    }

    private void LoadHappyEndingScene()
    {
        if (SceneTransitionManager.Instance != null)
        {
            SceneTransitionManager.Instance.LoadSceneWithFade(happyEndingSceneName);
        }
        else
        {
            SceneManager.LoadScene(happyEndingSceneName);
        }
    }

    private IEnumerator UpdateElapsedTimeTextNextFrame()
    {
        yield return null;

        if (timer == null)
            timer = FindAnyObjectByType<Timer>();

        if (elapsedTimeText != null && timer != null)
        {
            int remainingTime = timer.GetRemainingTime();

            elapsedTimeText.text = remainingTime.ToString();
            elapsedTimeText.SetText(remainingTime.ToString());
            elapsedTimeText.ForceMeshUpdate();

            Debug.Log("1프레임 뒤 팝업 시간 표시: " + remainingTime);
        }
        else
        {
            Debug.LogError("elapsedTimeText 또는 timer가 없습니다.");
        }
    }

    public void OnClickBackTitle()
    {
        Debug.Log("타이틀 화면으로 이동");

        AudioManager.Instance?.PlayButtonSFX(); //05.16. AudioManager를 위해 추가
        Time.timeScale = 1f;
        isClearProcessing = false;

        SceneManager.LoadScene("2_Title");
    }

    public void OnClickRetry()
    {
        Debug.Log("현재 스테이지 다시 시작");

        AudioManager.Instance?.PlayButtonSFX(); //05.16. AudioManager를 위해 추가
        Time.timeScale = 1f;
        isClearProcessing = false;

        if (popupPanel != null)
            popupPanel.SetActive(false);

        if (mapLoader != null)
            mapLoader.RetryCurrentStage();
    }

    public void OnClickNextStage()
    {
        Debug.Log("다음 스테이지로 이동");

        AudioManager.Instance?.PlayButtonSFX(); //05.16. AudioManager를 위해 추가
        Time.timeScale = 1f;
        isClearProcessing = false;

        if (popupPanel != null)
            popupPanel.SetActive(false);

        if (mapLoader != null)
            mapLoader.NextStage();
        else
            Debug.LogError("MapLoader가 연결되지 않았습니다.");
    }

    private void UpdateStarUI()
    {
        if (companionManager == null)
            companionManager = FindAnyObjectByType<CompanionManager>();

        int rescuedCount = 0;

        if (companionManager != null)
            rescuedCount = companionManager.GetRescuedCompanionCount();

        Debug.Log("구출한 동료 수: " + rescuedCount);

        for (int i = 0; i < starImages.Length; i++)
        {
            if (starImages[i] == null)
                continue;

            starImages[i].sprite = i < rescuedCount ? filledStarSprite : emptyStarSprite;
        }

    }

    private void SaveStageProgress()
    {
        int currentStage = 1;

        if (mapLoader != null)
            currentStage = mapLoader.CurrentStageNumber;
        else
            currentStage = PlayerPrefs.GetInt("SelectedStage", 1);

        int rescuedCount = 0;

        if (companionManager == null)
            companionManager = FindAnyObjectByType<CompanionManager>();

        if (companionManager != null)
            rescuedCount = companionManager.GetRescuedCompanionCount();

        rescuedCount = Mathf.Clamp(rescuedCount, 0, 3);

        string starKey = $"Stage{currentStage}_Stars";
        int previousStars = PlayerPrefs.GetInt(starKey, 0);

        // 예전에 3개 얻었는데 다시 해서 1개 얻으면 1개로 떨어지지 않게 Max 사용
        int bestStars = Mathf.Max(previousStars, rescuedCount);

        PlayerPrefs.SetInt(starKey, bestStars);

        // 다음 스테이지 잠금 해제
        int nextStage = currentStage + 1;

        if (nextStage <= 3)
        {
            PlayerPrefs.SetInt($"Stage{nextStage}_Locked", 0);
        }

        PlayerPrefs.Save();

        Debug.Log($"Stage {currentStage} 저장 완료 / 별: {bestStars} / 다음 스테이지 잠금 해제");
    }


}