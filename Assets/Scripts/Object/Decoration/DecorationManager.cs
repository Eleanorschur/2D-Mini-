using UnityEngine;
using System.Collections;

public class DecorationManager : MonoBehaviour
{
    private MapLoader mapLoader;

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
        yield return new WaitForEndOfFrame();
    }
}
