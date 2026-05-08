using UnityEngine;
using System.Collections;

public class PlayerManager : MonoBehaviour
{
    private MapLoader mapLoader;
    private GameObject player;

    public bool isPlayerLoaded { get; private set; }
    public System.Action PlayerLoadComplete;

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
        StartCoroutine(FindPlayerRoutine());
    }

    private IEnumerator FindPlayerRoutine()
    {
        yield return new WaitForEndOfFrame();

        player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            isPlayerLoaded = true;
            PlayerLoadComplete?.Invoke();
        }
    }

    public GameObject GetPlayerObj()
    {
        return player;
    }
}
