using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneAutoLoader : MonoBehaviour
{
    [SerializeField] private float totalDelay = 5f;     // 전체 시간
    [SerializeField] private float fadeOutDuration = 1f; // 페이드아웃 시간
    [SerializeField] private string nextSceneName = "NEOWILogo 2";

    [SerializeField] private SpriteRenderer logo; // TeamLogo 연결

    private void Start()
    {
        StartCoroutine(Flow());
    }

    private IEnumerator Flow()
    {
        // 1. 기다림 (페이드아웃 시작 전까지)
        yield return new WaitForSeconds(totalDelay - fadeOutDuration);

        // 2. 페이드아웃 시작
        float timer = 0f;
        Color color = logo.color;

        while (timer < fadeOutDuration)
        {
            timer += Time.deltaTime;
            float alpha = 1f - (timer / fadeOutDuration);

            color.a = Mathf.Clamp01(alpha);
            logo.color = color;

            yield return null;
        }

        // 3. 완전히 투명
        color.a = 0f;
        logo.color = color;

        // 4. 씬 전환
        UnityEngine.SceneManagement.SceneManager.LoadScene(nextSceneName);
    }
}