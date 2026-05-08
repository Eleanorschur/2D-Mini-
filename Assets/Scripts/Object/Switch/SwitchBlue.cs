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
        switchBlueList.Clear();

        yield return new WaitForEndOfFrame();

        foreach (Transform child in transform)
        {
            if (child.CompareTag("Switch_Blue"))
            {
                switchBlueList.Add(child.gameObject);
            }
        }

        switchBlueList.TrimExcess();
    }

    public void SwitchReset()
    {
        foreach (GameObject blueSwitchs in switchBlueList)
        {
            blueSwitchs.GetComponent<Switch>().SwitchReset();
        }
    }
}
