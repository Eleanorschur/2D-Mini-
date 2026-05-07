using UnityEngine;

public class LogoFadeIn : MonoBehaviour
{
    private SpriteRenderer sr;

    [SerializeField] private float fadeDuration = 2f; // 페이드 시간

    private float timer = 0f;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();

        // 처음엔 완전 투명
        Color color = sr.color;
        color.a = 0f;
        sr.color = color;
    }

    void Update()
    {
        if (timer < fadeDuration)
        {
            timer += Time.deltaTime;

            float alpha = Mathf.Clamp01(timer / fadeDuration);

            Color color = sr.color;
            color.a = alpha;
            sr.color = color;
        }
    }
}