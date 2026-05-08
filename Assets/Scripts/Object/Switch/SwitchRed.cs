using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SwitchRed : MonoBehaviour
{
    private MapLoader mapLoader;

    [SerializeField] private List<GameObject> switchRedList = new();

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

        switchRedList.Clear();
        GameObject[] redSwitchs = GameObject.FindGameObjectsWithTag("Switch_Red");
        switchRedList.AddRange(redSwitchs);
    }

    public void MapLoadCoroutine()
    {
        StartCoroutine(WaitForMapLoadRoutine());
    }

    public void SwitchReset()
    {
        foreach (GameObject redSwitch in switchRedList)
        {
            redSwitch.GetComponent<Switch>().SwitchReset();
        }
    }
}
