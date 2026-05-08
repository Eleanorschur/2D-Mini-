using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LeverManager : MonoBehaviour
{
    private MapLoader mapLoader;
    private ExitManager exitManager;
    private ExitDoor exitDoor;
    private BoxCollider2D coll2D;
    private Lever lever;

    [SerializeField] private List<GameObject> leverList = new();
    [SerializeField] private int leverCurrentCount = 0;

    void Awake()
    {
        mapLoader = FindAnyObjectByType<MapLoader>();
        exitManager = FindAnyObjectByType<ExitManager>();
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

        leverList.Clear();
        GameObject[] levers = GameObject.FindGameObjectsWithTag("Lever");
        leverList.AddRange(levers);
        StartCoroutine(WaitForExitObj());
    }

    public void MapLoadCoroutine()
    {
        StartCoroutine(WaitForMapLoadRoutine());
    }

    private IEnumerator WaitForExitObj()
    {
        while (exitManager.exit == null)
        {
            yield return null;
        }

        exitDoor = exitManager.GetExitObj();
        Debug.Log("LeverManager : Exit 오브젝트 취득 완료");
    }

    public void leverAddCounter()
    {
        ++leverCurrentCount;

        if (leverCurrentCount >= leverList.Count)
            exitDoor.DoorOpen(true);
    }

    public void LeverColliderActive(bool active)
    {
        foreach (GameObject lever in leverList)
        {
            coll2D = lever.GetComponent<BoxCollider2D>();
            coll2D.enabled = active; 
        }
    }

    public void AllLeverReset()
    {
        foreach (GameObject obj in leverList)
        {
            lever = obj.GetComponent<Lever>();
            lever.LeverReset();
        }
    }
}
