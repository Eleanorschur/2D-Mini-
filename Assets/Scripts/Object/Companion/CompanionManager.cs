using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CompanionManager : MonoBehaviour
{
    public MapLoader mapLoader;

    [SerializeField] private List<GameObject> companionList = new();
    private List<Vector3> transformList = new();
    private bool getList = false;
    public bool GetList => getList;

    void Awake()
    {
        getList = false;
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

    void Start()
    {
        
    }

    private void OnMapLoadFinished()
    {
        StartCoroutine(CollectCompanionRoutine());
    }

    private IEnumerator CollectCompanionRoutine()
    {
        yield return null; // Destroy가 완료될 때까지 한 프레임 대기

        companionList.Clear();

        foreach (Transform child in transform)
        {
            if (child.CompareTag("Companion"))
            {
                companionList.Add(child.gameObject);
            }
        }

        getList = true;
        GetTransformList();
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

    public List<GameObject> GetCompanionList()
    {
        if ( ! getList)
            return null;

        return companionList;
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
