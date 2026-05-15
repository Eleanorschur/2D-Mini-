using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenePopTransitionManager : MonoBehaviour
{
    public static ScenePopTransitionManager Instance { get; private set; }

    [Header("Pop Transition UI")]
    [SerializeField] private RectTransform popObject;
    [SerializeField] private CanvasGroup popCanvasGroup;

    [Header("Scene Transition Setting")]
    [SerializeField] private float popInDuration = 0.45f;
    [SerializeField] private float popOutDuration = 0.35f;

    [SerializeField] private float startScale = 0f;
    [SerializeField] private float overshootScale = 1.15f;
    [SerializeField] private float coverScale = 18f;

    private bool isTransitioning = false;
    private Vector3 originalScale = Vector3.one;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            if (popObject != null)
                originalScale = Vector3.one;

            HideImmediately();

            Debug.Log("ScenePopTransitionManager 생성 완료");
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadSceneWithPop(string sceneName)
    {
        Debug.Log("팝 전환 시작 요청: " + sceneName);

        if (isTransitioning)
        {
            Debug.LogWarning("이미 전환 중이라 무시됨");
            return;
        }

        StartCoroutine(LoadSceneWithPopRoutine(sceneName));
    }

    private IEnumerator LoadSceneWithPopRoutine(string sceneName)
    {
        isTransitioning = true;

        Time.timeScale = 1f;

        yield return PopCoverScreen();

        Debug.Log("씬 로드 실행: " + sceneName);
        SceneManager.LoadScene(sceneName);

        yield return null;

        yield return PopDisappear();

        isTransitioning = false;
    }

    private IEnumerator PopCoverScreen()
    {
        if (popObject == null || popCanvasGroup == null)
        {
            Debug.LogError("PopObject 또는 PopCanvasGroup이 연결되지 않았습니다.");
            yield break;
        }

        popObject.gameObject.SetActive(true);

        popCanvasGroup.alpha = 1f;
        popCanvasGroup.blocksRaycasts = true;
        popCanvasGroup.interactable = true;

        float timer = 0f;

        while (timer < popInDuration)
        {
            timer += Time.unscaledDeltaTime;
            float t = Mathf.Clamp01(timer / popInDuration);

            float scaleValue;

            if (t < 0.65f)
            {
                float p = t / 0.65f;
                scaleValue = Mathf.Lerp(startScale, overshootScale, EaseOutBack(p));
            }
            else
            {
                float p = (t - 0.65f) / 0.35f;
                scaleValue = Mathf.Lerp(overshootScale, coverScale, EaseInQuad(p));
            }

            popObject.localScale = originalScale * scaleValue;

            yield return null;
        }

        popObject.localScale = originalScale * coverScale;
    }

    private IEnumerator PopDisappear()
    {
        if (popObject == null || popCanvasGroup == null)
            yield break;

        float timer = 0f;
        Vector3 start = originalScale * coverScale;
        Vector3 end = originalScale * startScale;

        while (timer < popOutDuration)
        {
            timer += Time.unscaledDeltaTime;
            float t = Mathf.Clamp01(timer / popOutDuration);

            popObject.localScale = Vector3.Lerp(start, end, EaseInQuad(t));
            popCanvasGroup.alpha = Mathf.Lerp(1f, 0f, t);

            yield return null;
        }

        HideImmediately();
    }

    private void HideImmediately()
    {
        if (popObject != null)
        {
            popObject.gameObject.SetActive(true);
            popObject.localScale = originalScale * startScale;
        }

        if (popCanvasGroup != null)
        {
            popCanvasGroup.alpha = 0f;
            popCanvasGroup.blocksRaycasts = false;
            popCanvasGroup.interactable = false;
        }
    }

    private float EaseOutBack(float t)
    {
        float c1 = 1.70158f;
        float c3 = c1 + 1f;

        return 1f + c3 * Mathf.Pow(t - 1f, 3f) + c1 * Mathf.Pow(t - 1f, 2f);
    }

    private float EaseInQuad(float t)
    {
        return t * t;
    }
}