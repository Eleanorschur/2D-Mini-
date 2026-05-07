using System.Collections.Generic;
using UnityEngine;

public class CheckPointManager : MonoBehaviour
{
    private List<GameObject> checkPointList = new();
    private GameObject fianlCheckPoint;

    void Awake()
    {

    }

    void Start()
    {
        checkPointList.Clear();
        GameObject[] checkPoints = GameObject.FindGameObjectsWithTag("Checkpoint");
        checkPointList.AddRange(checkPoints);
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
