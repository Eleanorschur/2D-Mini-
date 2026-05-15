using UnityEngine;
using System.Collections;

public class ExitManager : MonoBehaviour
{
    public MapLoader mapLoader;
    [SerializeField]private ExitDoor exitDoor;

    public bool isExitLoaded { get; private set; }
    public System.Action ExitLoadComplete;

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

    private void Start()
    {
        
    }

    private void OnMapLoadFinished()
    {
        StartCoroutine(ExitDoorRoutine());
    }

    private IEnumerator ExitDoorRoutine()
    {
        yield return null; // Destroy가 완료될 때까지 한 프레임 대기

        exitDoor = GetComponentInChildren<ExitDoor>();

        if (exitDoor != null)
        {
            isExitLoaded = true;
            ExitLoadComplete?.Invoke();
        }

        exitDoor.DoorOpen(false);
    }

    public ExitDoor GetExitObj()
    {
        return exitDoor;
    }

}
