using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CheckPointManager : MonoBehaviour
{
    private MapLoader mapLoader;

    [SerializeField] private List<GameObject> checkPointList = new();
    private GameObject fianlCheckPoint;

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
        StopAllCoroutines();
        StartCoroutine(RefreshListRoutine());
    }

    private IEnumerator RefreshListRoutine()
    {
        checkPointList.Clear();

        yield return new WaitForEndOfFrame();

        foreach (Transform child in transform)
        {
            if (child.CompareTag("Checkpoint"))
            {
                checkPointList.Add(child.gameObject);
            }
        }

        checkPointList.TrimExcess();
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
