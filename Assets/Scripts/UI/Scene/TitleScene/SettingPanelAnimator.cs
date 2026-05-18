using UnityEngine;
using System.Collections;

public class SettingPanelAnimator : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private RectTransform panelRect;
    [SerializeField] private CanvasGroup panelCanvasGroup;

    [Header("Open Animation")]
    [SerializeField] private float openDuration = 0.35f;
    [SerializeField] private Vector3 startScale = new Vector3(0.75f, 0.75f, 1f);
    [SerializeField] private Vector3 overshootScale = new Vector3(1.08f, 1.08f, 1f);
    [SerializeField] private Vector3 normalScale = Vector3.one;

    [Header("Close Animation")]
    [SerializeField] private float closeDuration = 0.2f;
    [SerializeField] private Vector3 closeScale = new Vector3(0.85f, 0.85f, 1f);

    private Coroutine currentCoroutine;

    private void Awake()
    {
        if (panelRect == null)
            panelRect = GetComponent<RectTransform>();

        if (panelCanvasGroup == null)
            panelCanvasGroup = GetComponent<CanvasGroup>();

        HideImmediately();
    }

    public void HideImmediately()
    {
        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
            currentCoroutine = null;
        }

        gameObject.SetActive(true);

        panelRect.localScale = normalScale;
        panelCanvasGroup.alpha = 0f;
        panelCanvasGroup.interactable = false;
        panelCanvasGroup.blocksRaycasts = false;
    }

    public void Open()
    {
        gameObject.SetActive(true);

        if (currentCoroutine != null)
            StopCoroutine(currentCoroutine);

        currentCoroutine = StartCoroutine(OpenAnimation());
    }

    public void Close()
    {
        if (!gameObject.activeInHierarchy)
            return;

        if (currentCoroutine != null)
            StopCoroutine(currentCoroutine);

        currentCoroutine = StartCoroutine(CloseAnimation());
    }

    private IEnumerator OpenAnimation()
    {
        panelRect.localScale = startScale;
        panelCanvasGroup.alpha = 0f;
        panelCanvasGroup.interactable = false;
        panelCanvasGroup.blocksRaycasts = false;

        float timer = 0f;

        while (timer < openDuration)
        {
            timer += Time.unscaledDeltaTime;

            float t = Mathf.Clamp01(timer / openDuration);
            float eased = EaseOutBack(t);

            panelRect.localScale = Vector3.LerpUnclamped(startScale, overshootScale, eased);
            panelCanvasGroup.alpha = Mathf.Lerp(0f, 1f, t);

            yield return null;
        }

        timer = 0f;
        float settleDuration = 0.12f;

        while (timer < settleDuration)
        {
            timer += Time.unscaledDeltaTime;

            float t = Mathf.Clamp01(timer / settleDuration);
            panelRect.localScale = Vector3.Lerp(overshootScale, normalScale, t);

            yield return null;
        }

        panelRect.localScale = normalScale;
        panelCanvasGroup.alpha = 1f;
        panelCanvasGroup.interactable = true;
        panelCanvasGroup.blocksRaycasts = true;

        currentCoroutine = null;
    }

    private IEnumerator CloseAnimation()
    {
        panelCanvasGroup.interactable = false;
        panelCanvasGroup.blocksRaycasts = false;

        Vector3 currentScale = panelRect.localScale;

        float timer = 0f;

        while (timer < closeDuration)
        {
            timer += Time.unscaledDeltaTime;

            float t = Mathf.Clamp01(timer / closeDuration);

            panelRect.localScale = Vector3.Lerp(currentScale, closeScale, t);
            panelCanvasGroup.alpha = Mathf.Lerp(1f, 0f, t);

            yield return null;
        }

        panelCanvasGroup.alpha = 0f;
        panelRect.localScale = normalScale;
        panelCanvasGroup.interactable = false;
        panelCanvasGroup.blocksRaycasts = false;

        currentCoroutine = null;

        gameObject.SetActive(false);
    }

    private float EaseOutBack(float t)
    {
        float c1 = 1.70158f;
        float c3 = c1 + 1f;

        return 1f + c3 * Mathf.Pow(t - 1f, 3f) + c1 * Mathf.Pow(t - 1f, 2f);
    }
}