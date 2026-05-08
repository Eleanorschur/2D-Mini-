using UnityEngine;
using System.Collections;

public class ExitManager : MonoBehaviour
{
    private MapLoader mapLoader;
    [SerializeField] private ExitDoor exitDoor;

    public bool isExitLoaded { get; private set; }
    public System.Action ExitLoadComplete;

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
        StartCoroutine(FindExitRoutine());
    }

    private IEnumerator FindExitRoutine()
    {
        yield return new WaitForEndOfFrame();

        exitDoor = GetComponentInChildren<ExitDoor>();

        if (exitDoor != null)
        {
            isExitLoaded = true;
            ExitLoadComplete?.Invoke();
        }
    }

    public ExitDoor GetExitObj()
    {
        return exitDoor;
    }
}
