using UnityEngine;
using System.Collections; 

// 작게 시작 -> 통통 튀듯 커짐 -> 원래 크기로 돌아옴. 

public class LogoScene : MonoBehaviour
{
    [SerializeField] private RectTransform logoRect;    // 로고 UI의 위치, 크기, 스케일을 조절하기 위한 변수
    [SerializeField] private CanvasGroup logoCanvasGroup; // 로고의 투명도를 조절하기 위한 변수
                                                          // CanvasGroup.alpha가 0이면 안 보이고, 1이면 완전히 보임.
    

    [Header("Timing")]
    [SerializeField] private float appearDuration = 0.6f;
    [SerializeField] private float holdDuration = 1.2f;
    [SerializeField] private float fadeOutDuration = 0.6f;

    [Header("Scale")]
    [SerializeField] private Vector3 startScale = Vector3.zero; 
    [SerializeField] private Vector3 overshootScale = new Vector3(1.15f, 1.15f, 1f);    // 로고가 목표 크기보다 살짝 크게 커지는 크기 
                                                                                        // 1.15는 원래 크기의 115% 임. 
    [SerializeField] private Vector3 normalScale = Vector3.one;                         // 최종 크기 ( 1,1,1 )

    private void Start()
    {
        StartCoroutine(LogoAnimation());
    }

    // 애니메이션 시간에 따라 진행시키는 코루틴. 
    private IEnumerator LogoAnimation()
    {
        logoRect.localScale = startScale;
        logoCanvasGroup.alpha = 0f;

        float timer = 0f;       // 시간 재기 위한 변수 

        while ( timer < appearDuration)
        {
            timer += Time.deltaTime;
            float t = timer / appearDuration; // 현재 진행률 ( 퍼센트 0 ~ 1 )  

            float eased = EaseOutBounce(t); // eased : 점진적으로 움직이는

            logoRect.localScale = Vector3.LerpUnclamped(startScale, overshootScale , eased);    // LerpUnclamped : 1 넘어가도 그래도 계산.
            logoCanvasGroup.alpha = Mathf.Lerp(0f, 1f, t);      

            yield return null;
        }

        timer = 0f;

        while ( timer < 0.25f)
        {
            timer += Time.deltaTime;
            float t = timer / 0.25f;

            logoRect.localScale = Vector3.Lerp(overshootScale, normalScale, t);

            yield return null;
        }

        yield return new WaitForSeconds(holdDuration);

        timer = 0f;   

        while ( timer < fadeOutDuration)
        {
            timer += Time.deltaTime;
            float t = timer / fadeOutDuration;

            logoCanvasGroup.alpha = Mathf.Lerp(1f, 0f, t);

            yield return null;
        }

        NextScene();

    }

    // 통통 튀는 생동감
    private float EaseOutBack( float t ) 
    {
        float c1 = 1.70158f;
        float c3 = c1 + 1f;

        return 1f + c3 * Mathf.Pow(t - 1f, 3f) + c1 * Mathf.Pow(t -1f, 2f); // Pow : 거듭제곱 
    }

    // 고무줄이나 젤리처럼 부르르 떨면서 멈추는 느낌
    private float EaseOutElastic(float t)
    {
        float c4 = (2f * Mathf.PI) / 3f;
        return t == 0 ? 0 : t == 1 ? 1
          : Mathf.Pow(2f, -10f * t) * Mathf.Sin((t * 10f - 0.75f) * c4) + 1f;
    }


    // 통통 효과
    private float EaseOutBounce(float t)
    {
        float n1 = 7.5625f;
        float d1 = 2.75f;

        if (t < 1f / d1)
        {
            return n1 * t * t;
        }
        else if (t < 2f / d1)
        {
            t -= 1.5f / d1;
            return n1 * t * t + 0.75f;
        }
        else if (t < 2.5f / d1)
        {
            t -= 2.25f / d1;
            return n1 * t * t + 0.9375f;
        }
        else
        {
            t -= 2.625f / d1;
            return n1 * t * t + 0.984375f;
        }
    }


    public void NextScene()
    {
        GameSceneManager.Instance.ChangeScene("2_Title");
    }
}
