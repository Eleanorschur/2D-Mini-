using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CompanionManager : MonoBehaviour
{
    private MapLoader mapLoader;

    [SerializeField] private List<GameObject> companionList = new();
    private List<Vector3> transformList = new();

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

        companionList.Clear();
        GameObject[] companions = GameObject.FindGameObjectsWithTag("Companion");
        companionList.AddRange(companions);

        GetTransformList();
    }

    public void MapLoadCoroutine()
    {
        StartCoroutine(WaitForMapLoadRoutine());
    }

    public int GetIndex(GameObject obj)
    {
        int index = -1;

        foreach (GameObject companion in companionList)
        {
            ++index;
            if (companion == obj)
                return index;
        }

        return -1;
    }

    private void GetTransformList()
    {
        transformList.Clear();

        foreach (GameObject companion in companionList)
        {
            transformList.Add(companion.gameObject.transform.position);
        }
    }

    public void CompanionReset()
    {
        int index = 0;

        foreach (GameObject companion in companionList)
        {
            companion.GetComponent<Companion>().CompanionReset(transformList[index]);
            ++index;
        }
    }
}
