using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour
{
    private TextMeshProUGUI timerText;
    private PlayerManager playerManager;
    private StageReset stageReset;
    private Movement playerMovement;
    private Coroutine countdownCoroutine;

    [Header("Timer")]
    [SerializeField] private int defaultTime = 60;
    [SerializeField] private int warningTime = 10;

    private int currentTime = 0;

    [Header("Game Over")]
    [SerializeField] private GameObject blurOverlay;
    [SerializeField] private GameObject gameOverImage;
    [SerializeField] private string badEndingSceneName = "BadEnding";
    [SerializeField] private float badEndingDelay = 1f;

    [Header("Game Over Animation")]
    [SerializeField] private float popDuration = 0.35f;
    [SerializeField] private Vector3 startScale = Vector3.zero;
    [SerializeField] private Vector3 overshootScale = new Vector3(1.2f, 1.2f, 1f);
    [SerializeField] private Vector3 normalScale = Vector3.one;

    [Header("Blur Overlay Animation")]
    [SerializeField] private float blurFadeDuration = 0.25f;
    [SerializeField] private float blurMaxAlpha = 0.55f;

    public int GetCurrentTime()
    {
        return currentTime;
    }

    public int GetRemainingTime()
    {
        return currentTime;
    }

    public int GetElapsedTime()
    {
        return defaultTime - currentTime;
    }

    private void OnEnable()
    {
        playerManager = FindAnyObjectByType<PlayerManager>();

        if (playerManager != null)
            playerManager.PlayerLoadComplete += PlayerLoadComplete;
    }

    private void OnDisable()
    {
        if (playerManager != null)
            playerManager.PlayerLoadComplete -= PlayerLoadComplete;
    }

    private void Start()
    {
        stageReset = FindAnyObjectByType<StageReset>();
        timerText = GetComponentInChildren<TextMeshProUGUI>();

        currentTime = defaultTime;

        if (blurOverlay != null)
        {
            blurOverlay.SetActive(false);

            CanvasGroup blurCanvasGroup = blurOverlay.GetComponent<CanvasGroup>();

            if (blurCanvasGroup == null)
                blurCanvasGroup = blurOverlay.AddComponent<CanvasGroup>();

            blurCanvasGroup.alpha = 0f;
            blurCanvasGroup.blocksRaycasts = false;
        }

        if (gameOverImage != null)
        {
            gameOverImage.SetActive(false);
            gameOverImage.transform.localScale = startScale;
        }

        if (timerText != null)
            timerText.gameObject.SetActive(true);

        UpdateText(currentTime);

        countdownCoroutine = StartCoroutine(CountDown());
    }

    private void PlayerLoadComplete()
    {
        if (playerManager == null)
            return;

        GameObject playerObj = playerManager.GetPlayerObj();

        if (playerObj != null)
            playerMovement = playerObj.GetComponent<Movement>();
    }

    private IEnumerator CountDown()
    {
        while (currentTime > 0)
        {
            yield return new WaitForSeconds(1.0f);

            currentTime--;
            UpdateText(currentTime);
        }

        TimeOver();
    }

    private void UpdateText(int time)
    {
        if (timerText == null)
            return;

        timerText.text = time.ToString();

        if (time <= warningTime)
            timerText.color = Color.red;
        else
            timerText.color = Color.white;
    }

    public void PauseTimer()
    {
        if (countdownCoroutine != null)
        {
            StopCoroutine(countdownCoroutine);
            countdownCoroutine = null;
        }
    }

    public void ResumeTimer()
    {
        if (countdownCoroutine == null && currentTime > 0)
        {
            countdownCoroutine = StartCoroutine(CountDown());
        }
    }

    public void StopTimer()
    {
        if (countdownCoroutine != null)
        {
            StopCoroutine(countdownCoroutine);
            countdownCoroutine = null;
        }
    }

    public void ResetTimer()
    {
        StopTimer();

        currentTime = defaultTime;

        if (blurOverlay != null)
        {
            CanvasGroup blurCanvasGroup = blurOverlay.GetComponent<CanvasGroup>();

            if (blurCanvasGroup != null)
                blurCanvasGroup.alpha = 0f;

            blurOverlay.SetActive(false);
        }

        if (gameOverImage != null)
        {
            gameOverImage.SetActive(false);
            gameOverImage.transform.localScale = startScale;

            CanvasGroup canvasGroup = gameOverImage.GetComponent<CanvasGroup>();

            if (canvasGroup != null)
                canvasGroup.alpha = 0f;
        }

        if (timerText != null)
            timerText.gameObject.SetActive(true);

        UpdateText(currentTime);
        ResumeTimer();
    }

    private void TimeOver()
    {
        StopTimer();

        if (playerMovement != null)
            playerMovement.MoveLock(true);

        if (stageReset != null)
            stageReset.ResetLock(true);

        // 기존 숫자 타이머 숨기기
        if (timerText != null)
            timerText.gameObject.SetActive(false);

        // 뒤 배경 어둡게 처리
        if (blurOverlay != null)
        {
            blurOverlay.SetActive(true);
            StartCoroutine(PlayBlurOverlayFade());
        }
        else
        {
            Debug.LogWarning("BlurOverlay가 Timer에 연결되지 않았습니다.");
        }

        // 게임 오버 이미지 표시
        if (gameOverImage != null)
        {
            gameOverImage.SetActive(true);
            StartCoroutine(PlayGameOverAnimation());
        }
        else
        {
            Debug.LogWarning("GameOverImage가 Timer에 연결되지 않았습니다.");
            StartCoroutine(LoadBadEndingAfterDelay());
        }

        Debug.Log("타임 오버");
    }

    private IEnumerator PlayBlurOverlayFade()
    {
        CanvasGroup blurCanvasGroup = blurOverlay.GetComponent<CanvasGroup>();

        if (blurCanvasGroup == null)
            blurCanvasGroup = blurOverlay.AddComponent<CanvasGroup>();

        blurCanvasGroup.alpha = 0f;
        blurCanvasGroup.blocksRaycasts = false;

        float timer = 0f;

        while (timer < blurFadeDuration)
        {
            timer += Time.unscaledDeltaTime;

            float t = timer / blurFadeDuration;
            t = Mathf.Clamp01(t);

            blurCanvasGroup.alpha = Mathf.Lerp(0f, blurMaxAlpha, t);

            yield return null;
        }

        blurCanvasGroup.alpha = blurMaxAlpha;
    }

    private IEnumerator LoadBadEndingAfterDelay()
    {
        yield return new WaitForSecondsRealtime(badEndingDelay);

        Time.timeScale = 1f;

        if (SceneTransitionManager.Instance != null)
        {
            SceneTransitionManager.Instance.LoadSceneWithFade(badEndingSceneName);
        }
        else
        {
            Debug.LogWarning("SceneTransitionManager가 없어서 바로 BadEnding으로 이동합니다.");
            SceneManager.LoadScene(badEndingSceneName);
        }
    }

    private IEnumerator PlayGameOverAnimation()
    {
        RectTransform rectTransform = gameOverImage.GetComponent<RectTransform>();
        CanvasGroup canvasGroup = gameOverImage.GetComponent<CanvasGroup>();

        if (canvasGroup == null)
            canvasGroup = gameOverImage.AddComponent<CanvasGroup>();

        rectTransform.localScale = startScale;
        canvasGroup.alpha = 0f;

        float timer = 0f;

        // 1단계: 작고 투명한 상태에서 살짝 크게 튀어나오기
        while (timer < popDuration)
        {
            timer += Time.unscaledDeltaTime;

            float t = timer / popDuration;
            t = Mathf.Clamp01(t);

            float easedT = EaseOutBack(t);

            rectTransform.localScale = Vector3.LerpUnclamped(startScale, overshootScale, easedT);
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, t);

            yield return null;
        }

        rectTransform.localScale = overshootScale;
        canvasGroup.alpha = 1f;

        timer = 0f;

        // 2단계: 살짝 커진 상태에서 원래 크기로 돌아오기
        float settleDuration = 0.15f;

        while (timer < settleDuration)
        {
            timer += Time.unscaledDeltaTime;

            float t = timer / settleDuration;
            t = Mathf.Clamp01(t);

            rectTransform.localScale = Vector3.Lerp(overshootScale, normalScale, t);

            yield return null;
        }

        rectTransform.localScale = normalScale;

        StartCoroutine(LoadBadEndingAfterDelay());
    }

    private float EaseOutBack(float t)
    {
        float c1 = 1.70158f;
        float c3 = c1 + 1f;

        return 1f + c3 * Mathf.Pow(t - 1f, 3f) + c1 * Mathf.Pow(t - 1f, 2f);
    }
}