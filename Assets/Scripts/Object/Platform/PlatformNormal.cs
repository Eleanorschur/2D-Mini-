using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlatformNormal : MonoBehaviour
{
    private MapLoader mapLoader;

    [SerializeField] List<GameObject> platformList = new();

    void Awake()
    {
        mapLoader = FindAnyObjectByType<MapLoader>();
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

    private void OnMapLoadFinished()
    {
        StopAllCoroutines();
        StartCoroutine(RefreshListRoutine());
    }

    private IEnumerator RefreshListRoutine()
    {
        platformList.Clear();

        yield return new WaitForEndOfFrame();

        foreach (Transform child in transform)
        {
            if (child.CompareTag("Platform_Normal"))
            {
                platformList.Add(child.gameObject);
            }
        }

        platformList.TrimExcess();
    }
}
