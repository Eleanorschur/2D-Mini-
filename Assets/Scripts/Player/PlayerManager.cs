using UnityEngine;
using System.Collections;

public class PlayerManager : MonoBehaviour
{
    private MapLoader mapLoader;

    public GameObject player { get; private set; }
    public System.Action OnReady;

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

        SetPlayerObj();
    }

    public void MapLoadCoroutine()
    {
        StartCoroutine(WaitForMapLoadRoutine());
    }

    public void SetPlayerObj()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
            OnReady?.Invoke();
    }

    public GameObject GetPlayerObj()
    {
        return player;
    }

    public void DelPlayerObj()
    {
        player = null;
    }
}
