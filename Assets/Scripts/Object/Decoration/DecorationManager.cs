using UnityEngine;
using System.Collections;

public class DecorationManager : MonoBehaviour
{
    private MapLoader mapLoader;

    void Awake()
    {
        mapLoader = FindAnyObjectByType<MapLoader>();
    }

    void Start()
    {
        MapLoadCoroutine();
    }

    private IEnumerator WaitForMapLoadRoutine()
    {
        while ( ! mapLoader.isMapLoaded)
        {
            yield return null;
        }

    }

    public void MapLoadCoroutine()
    {
        StartCoroutine(WaitForMapLoadRoutine());
    }

}
