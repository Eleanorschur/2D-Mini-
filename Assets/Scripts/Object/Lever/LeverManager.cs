using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LeverManager : MonoBehaviour
{
    private MapLoader mapLoader;
    private ExitManager exitManager;
    [SerializeField]private ExitDoor exitDoor;
    private BoxCollider2D coll2D;
    private Lever lever;

    [SerializeField] private List<GameObject> leverList = new();
    [SerializeField] private int leverCurrentCount = 0;

    void Awake()
    {
        mapLoader = FindAnyObjectByType<MapLoader>();
        exitManager = FindAnyObjectByType<ExitManager>();
    }

    void OnEnable()
    {
        if (mapLoader != null)
            mapLoader.MapLoadComplete += OnMapLoadFinished;

        if (exitManager != null)
            exitManager.ExitLoadComplete += OnExitLoadFinished;
    }

    void OnDisable()
    {
        if (mapLoader != null)
            mapLoader.MapLoadComplete -= OnMapLoadFinished;

        if (exitManager != null)
            exitManager.ExitLoadComplete -= OnExitLoadFinished;
    }

    private void OnMapLoadFinished()
    {
        StopAllCoroutines();
        StartCoroutine(RefreshListRoutine());
    }

    private IEnumerator RefreshListRoutine()
    {
        leverList.Clear();

        yield return new WaitForEndOfFrame();

        foreach (Transform child in transform)
        {
            if (child.CompareTag("Lever"))
            {
                leverList.Add(child.gameObject);
            }
        }

        leverList.TrimExcess();
    }

    private void OnExitLoadFinished()
    {
        exitDoor = exitManager.GetExitObj();
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
