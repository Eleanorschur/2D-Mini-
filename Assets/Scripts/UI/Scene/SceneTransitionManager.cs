using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : MonoBehaviour
{
    public static SceneTransitionManager Instance { get; private set; }

    [Header("Fade UI")]
    [SerializeField] private CanvasGroup fadeCanvasGroup;

    [Header("Fade Setting")]
    [SerializeField] private float fadeDuration = 0.8f;

    private bool isTransitioning = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            Debug.Log("SceneTransitionManager Awake 실행됨");

            if (fadeCanvasGroup != null)
            {
                fadeCanvasGroup.alpha = 0f;
                fadeCanvasGroup.blocksRaycasts = false;
            }
            else
            {
                Debug.LogError("Fade Canvas Group이 연결되지 않았습니다.");
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadSceneWithFade(string sceneName)
    {
        Debug.Log("Fade 씬 전환 요청: " + sceneName);

        if (isTransitioning)
        {
            Debug.LogWarning("이미 씬 전환 중입니다.");
            return;
        }

        StartCoroutine(LoadSceneWithFadeRoutine(sceneName));
    }

    private IEnumerator LoadSceneWithFadeRoutine(string sceneName)
    {
        isTransitioning = true;

        Time.timeScale = 1f;

        yield return FadeOut();

        Debug.Log("씬 로드 실행: " + sceneName);
        SceneManager.LoadScene(sceneName);

        yield return null;

        yield return FadeIn();

        isTransitioning = false;
    }

    private IEnumerator FadeOut()
    {
        if (fadeCanvasGroup == null)
        {
            Debug.LogError("FadeCanvasGroup이 null이라 FadeOut 불가");
            yield break;
        }

        fadeCanvasGroup.gameObject.SetActive(true);
        fadeCanvasGroup.blocksRaycasts = true;
        fadeCanvasGroup.interactable = true;

        float timer = 0f;

        while (timer < fadeDuration)
        {
            timer += Time.unscaledDeltaTime;

            float alpha = timer / fadeDuration;
            fadeCanvasGroup.alpha = Mathf.Clamp01(alpha);

            yield return null;
        }

        fadeCanvasGroup.alpha = 1f;
    }

    private IEnumerator FadeIn()
    {
        if (fadeCanvasGroup == null)
        {
            Debug.LogError("FadeCanvasGroup이 null이라 FadeIn 불가");
            yield break;
        }

        float timer = 0f;

        while (timer < fadeDuration)
        {
            timer += Time.unscaledDeltaTime;

            float alpha = 1f - (timer / fadeDuration);
            fadeCanvasGroup.alpha = Mathf.Clamp01(alpha);

            yield return null;
        }

        fadeCanvasGroup.alpha = 0f;
        fadeCanvasGroup.blocksRaycasts = false;
        fadeCanvasGroup.interactable = false;
    }
}