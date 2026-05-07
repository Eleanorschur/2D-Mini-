using UnityEngine;
using System.Collections;

public class TitlePopInEffect : MonoBehaviour
{
    [SerializeField] private float duration = 0.8f;
    [SerializeField] private float startScale = 0.75f;
    [SerializeField] private float overshootScale = 1.08f;

    private SpriteRenderer sr;
    private Vector3 originalScale;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        originalScale = transform.localScale;
    }

    private void Start()
    {
        StartCoroutine(PopIn());
    }

    private IEnumerator PopIn()
    {
        transform.localScale = originalScale * startScale;

        Color color = sr.color;
        color.a = 0f;
        sr.color = color;

        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            float t = Mathf.Clamp01(timer / duration);

            color.a = Mathf.SmoothStep(0f, 1f, t);
            sr.color = color;

            float scaleValue;

            if (t < 0.65f)
            {
                float p = t / 0.65f;
                scaleValue = Mathf.Lerp(startScale, overshootScale, EaseOutBack(p));
            }
            else
            {
                float p = (t - 0.65f) / 0.35f;
                scaleValue = Mathf.Lerp(overshootScale, 1f, EaseOutBounce(p));
            }

            transform.localScale = originalScale * scaleValue;

            yield return null;
        }

        color.a = 1f;
        sr.color = color;
        transform.localScale = originalScale;
    }

    private float EaseOutBack(float t)
    {
        float c1 = 1.70158f;
        float c3 = c1 + 1f;

        return 1f + c3 * Mathf.Pow(t - 1f, 3f) + c1 * Mathf.Pow(t - 1f, 2f);
    }

    private float EaseOutBounce(float t)
    {
        if (t < 1f / 2.75f)
        {
            return 7.5625f * t * t;
        }
        else if (t < 2f / 2.75f)
        {
            t -= 1.5f / 2.75f;
            return 7.5625f * t * t + 0.75f;
        }
        else if (t < 2.5f / 2.75f)
        {
            t -= 2.25f / 2.75f;
            return 7.5625f * t * t + 0.9375f;
        }
        else
        {
            t -= 2.625f / 2.75f;
            return 7.5625f * t * t + 0.984375f;
        }
    }
}