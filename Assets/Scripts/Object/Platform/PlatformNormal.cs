using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlatformNormal : MonoBehaviour
{
    public MapLoader mapLoader;

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
            if (child.CompareTag("Platform_Normal"))
            {
                platformList.Add(child.gameObject);
            }
        }
    }
}
