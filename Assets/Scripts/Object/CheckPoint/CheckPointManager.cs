using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CheckPointManager : MonoBehaviour
{
    private MapLoader mapLoader;

    private List<GameObject> checkPointList = new();
    private GameObject fianlCheckPoint;

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

        checkPointList.Clear();
        GameObject[] checkPoints = GameObject.FindGameObjectsWithTag("Checkpoint");
        checkPointList.AddRange(checkPoints);
    }

    public void MapLoadCoroutine()
    {
        StartCoroutine(WaitForMapLoadRoutine());
    }

    public void SetFinalCheckPoint(GameObject obj)
    {
        fianlCheckPoint = obj;
        ReSettingsAllCheckPoint(obj);
    }

    public GameObject GetFinalCheckPoint()
    {
        return fianlCheckPoint;
    }

    public void ReSettingsAllCheckPoint(GameObject obj)
    {
        foreach (GameObject checkPoint in checkPointList)
        {
            CheckPoint cp = checkPoint.GetComponent<CheckPoint>();
            cp.SetActiveCheckPoint(obj == checkPoint);
        }
    }

    public void CheckPointReset()
    {
        foreach (GameObject checkPoint in checkPointList)
        {
            checkPoint.GetComponent<CheckPoint>().CheckPointReset();
        }
    }
}
