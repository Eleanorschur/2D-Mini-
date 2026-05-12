using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlatformBlue : MonoBehaviour
{
    public MapLoader mapLoader;
    private PlatformData platformData;

    [SerializeField] List<GameObject> platformList = new();

    void Awake()
    {
        
    }

    void OnEnable()
    {
        if (mapLoader != null)
            mapLoader.MapLoadComplete += OnMapLoadFinished;
    }

    void OnDisable()
    {
        if (mapLoader != null)
            mapLoader.MapLoadComplete -= OnMapLoadFinished;
    }

    void Start()
    {

    }

    private void OnMapLoadFinished()
    {
        StartCoroutine(CollectPlatformsRoutine());
    }

    private IEnumerator CollectPlatformsRoutine()
    {
        yield return null; // Destroy가 완료될 때까지 한 프레임 대기

        platformList.Clear();
        foreach (Transform child in transform)
        {
            if (child.CompareTag("Platform_Blue"))
            {
                platformList.Add(child.gameObject);
            }
        }

        platformData = GetComponentInParent<PlatformData>();
        PlatformHide(true, platformData.platformHideAlpha);
    }

    public void PlatformActive(bool active)
    {
        foreach (GameObject obj in platformList)
        {
            obj.SetActive(active);
        }
    }

    public void PlatformHide(bool hide, float alphaValue)
    {
        foreach (GameObject obj in platformList)
        {
            BoxCollider2D objCollider = obj.GetComponent<BoxCollider2D>();

            if (objCollider != null)
                objCollider.enabled = !hide;

            float startValue = hide ? 1 : alphaValue;
            float endValue = hide ? alphaValue : 1;

            StartCoroutine(HideRoutine(obj, startValue, endValue, 0.5f));
        }
    }

    private IEnumerator HideRoutine(GameObject obj, float start, float end, float duration)
    {
        SpriteRenderer objRenderer = obj.GetComponent<SpriteRenderer>();

        Color objColor;

        objColor = objRenderer.color;
        float elapsed = 0;

        if (objColor.a == end)
            yield break;

        while (elapsed < 1.0f)
        {
            elapsed += Time.deltaTime / duration;
            objColor.a = Mathf.Lerp(start, end, elapsed);
            objRenderer.color = objColor;

            yield return null;
        }
    }
}
