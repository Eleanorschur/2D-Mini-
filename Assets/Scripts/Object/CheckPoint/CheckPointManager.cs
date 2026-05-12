using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CheckPointManager : MonoBehaviour
{
    public MapLoader mapLoader;

    [SerializeField] private List<GameObject> checkPointList = new();
    [SerializeField] private GameObject fianlCheckPoint;

    [SerializeField] private int playerStatus = -1;
    public int PlayerStatus => playerStatus;

    void Awake()
    {

    }

    private void OnEnable()
    {
        if (mapLoader != null)
            mapLoader.MapLoadComplete += OnMapLoadFinished;
    }

    void OnDisable()
    {
        if (mapLoader != null)
            mapLoader.MapLoadComplete -= OnMapLoadFinished;
    }

    void Start()
    {

    }

    private void OnMapLoadFinished()
    {
        fianlCheckPoint = null;
        playerStatus = -1;
        StartCoroutine(CollectCheckpointRoutine());
    }

    private IEnumerator CollectCheckpointRoutine()
    {
        yield return null; // Destroy가 완료될 때까지 한 프레임 대기

        checkPointList.Clear();

        foreach (Transform child in transform)
        {
            if (child.CompareTag("Checkpoint"))
            {
                checkPointList.Add(child.gameObject);
            }
        }
    }

    public void SetFinalCheckPoint(GameObject obj)
    {
        fianlCheckPoint = obj;

        if (obj != null)
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

    public void SetPlayerStatus(int status)
    {
        playerStatus = status;
    }
}
