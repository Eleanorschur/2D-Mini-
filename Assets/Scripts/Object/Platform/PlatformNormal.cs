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

        platformList.Clear();
        GameObject[] normalPlatforms = GameObject.FindGameObjectsWithTag("Platform_Normal");
        platformList.AddRange(normalPlatforms);
    }

    public void MapLoadCoroutine()
    {
        StartCoroutine(WaitForMapLoadRoutine());
    }
}
