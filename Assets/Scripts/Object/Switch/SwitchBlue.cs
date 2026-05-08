using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SwitchBlue : MonoBehaviour
{
    private MapLoader mapLoader;

    [SerializeField] private List<GameObject> switchBlueList = new();

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
        while (!mapLoader.isMapLoaded)
        {
            yield return null;
        }

        switchBlueList.Clear();
        GameObject[] blueSwitchs = GameObject.FindGameObjectsWithTag("Switch_Blue");
        switchBlueList.AddRange(blueSwitchs);
    }

    public void MapLoadCoroutine()
    {
        StartCoroutine(WaitForMapLoadRoutine());
    }

    public void SwitchReset()
    {
        foreach (GameObject blueSwitchs in switchBlueList)
        {
            blueSwitchs.GetComponent<Switch>().SwitchReset();
        }
    }
}
