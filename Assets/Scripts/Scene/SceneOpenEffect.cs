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


    // 씬 시작 — 페이드 인
    public void PlayIrisIn()
    {
        gameObject.SetActive(true);
        StopAllCoroutines();
        StartCoroutine(IrisRoutine(open: true));
    }

    // 출구 진입 — 페이드 아웃
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

        float startTime = Time.unscaledTime;

        while (true)
        {
            float elapsed = Time.unscaledTime - startTime;
            if (elapsed >= duration) break;

            float t = easeCurve.Evaluate(elapsed / duration);
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
        float halfH = cam.orthographicSize;
        float halfW = halfH * cam.aspect;

        Vector2 maskPos = slimeMask.transform.position;
        Vector2 camPos = cam.transform.position;

        Vector2[] corners = {
        camPos + new Vector2(-halfW, -halfH),
        camPos + new Vector2( halfW, -halfH),
        camPos + new Vector2(-halfW,  halfH),
        camPos + new Vector2( halfW,  halfH),
    };

        float maxDist = 0f;
        foreach (var c in corners)
            maxDist = Mathf.Max(maxDist, Vector2.Distance(maskPos, c));

        float spriteSize = slimeMask.sprite != null ? slimeMask.sprite.bounds.size.x : 1f;
        return (maxDist * 3f / spriteSize) * 1.1f;
    }
}
