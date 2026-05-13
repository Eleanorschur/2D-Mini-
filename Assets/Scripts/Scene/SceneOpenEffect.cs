using UnityEngine;
using System.Collections;

public class SceneOpenEffect : MonoBehaviour
{
    [Header("컴포넌트")]
    [SerializeField] private SpriteMask slimeMask;
    [SerializeField] private SpriteRenderer blackOverlay;
    [SerializeField] private Transform target;

    [Header("연출 설정")]
    [SerializeField] private float duration = 1.2f;
    [SerializeField] private float delayBeforeStart = 0.2f;
    [SerializeField] private AnimationCurve easeCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    public System.Action OnEffectComplete;

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    // 씬 시작 — 구멍이 열림
    public void PlayIrisIn()
    {
        gameObject.SetActive(true);
        StopAllCoroutines();
        StartCoroutine(IrisRoutine(open: true));
    }

    // 출구 진입 — 구멍이 닫힘
    public void PlayIrisOut()
    {
        gameObject.SetActive(true);
        StopAllCoroutines();
        StartCoroutine(IrisRoutine(open: false));
    }

    private IEnumerator IrisRoutine(bool open)
    {
        if (target != null)
            slimeMask.transform.position = target.position;

        float targetScale = GetTargetScale();
        float startScale  = open ? 0f : targetScale;
        float endScale    = open ? targetScale : 0f;

        slimeMask.transform.localScale = Vector3.one * startScale;

        if (delayBeforeStart > 0)
            yield return new WaitForSeconds(delayBeforeStart);

        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = easeCurve.Evaluate(Mathf.Clamp01(elapsed / duration));
            slimeMask.transform.localScale = Vector3.one * Mathf.Lerp(startScale, endScale, t);
            yield return null;
        }

        slimeMask.transform.localScale = Vector3.one * endScale;

        OnEffectComplete?.Invoke();

        if (open)
            gameObject.SetActive(false);
    }

    private float GetTargetScale()
    {
        Camera cam = Camera.main;
        float camHeight = cam.orthographicSize * 2f;
        float camWidth  = camHeight * cam.aspect;
        float diagonal  = Mathf.Sqrt(camWidth * camWidth + camHeight * camHeight);
        float spriteSize = slimeMask.sprite != null ? slimeMask.sprite.bounds.size.x : 1f;
        return (diagonal / spriteSize) * 1.1f;
    }
}
