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
        switchRedList.Clear();

        yield return new WaitForEndOfFrame();

        foreach (Transform child in transform)
        {
            if (child.CompareTag("Switch_Red"))
            {
                switchRedList.Add(child.gameObject);
            }
        }

        switchRedList.TrimExcess();
    }

    public void SwitchReset()
    {
        foreach (GameObject redSwitch in switchRedList)
        {
            redSwitch.GetComponent<Switch>().SwitchReset();
        }
    }
}
