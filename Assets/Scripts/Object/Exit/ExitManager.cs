using UnityEngine;
using System.Collections;

public class ExitManager : MonoBehaviour
{
    private MapLoader mapLoader;

    public ExitDoor exit { get; private set; }
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

        SetExitObj();
    }

    public void MapLoadCoroutine()
    {
        StartCoroutine(WaitForMapLoadRoutine());
    }

    public void SetExitObj()
    {
        exit = FindAnyObjectByType<ExitDoor>();

        if (exit != null)
            OnReady?.Invoke();
    }

    public ExitDoor GetExitObj()
    {
        return exit;
    }

    public void DelExitObj()
    {
        exit = null;
    }
}
