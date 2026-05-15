using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class CompanionCountPool : MonoBehaviour
{
    public static CompanionCountPool Instance { get; private set; }

    public MapLoader mapLoader;
    private CompanionManager companionManager;
    [SerializeField] private GameObject CCPrefab;
    [SerializeField] private List<GameObject> CCPrefabList;

    void Awake()
    {

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
        companionManager = FindAnyObjectByType<CompanionManager>();
    }

    private void OnMapLoadFinished()
    {
        DeleteCC();
        StartCoroutine(CreateNewCCRoutine());
    }

    private IEnumerator CreateNewCCRoutine()
    {
        while ( ! companionManager.GetList)
        {
            yield return null;
        }

        int poolSize = companionManager.GetCompanionList().Count;

        Instance = this;
        for (int i = 0; i < poolSize; i++)
        {
            CreateNewCC();
        }
    }

    private void CreateNewCC()
    {
        GameObject obj = Instantiate(CCPrefab, transform);
        CCPrefabList.Add(obj);
    }

    private void DeleteCC()
    {
        foreach (GameObject obj in CCPrefabList)
        {
            Destroy(obj);
        }

        CCPrefabList.Clear();
    }

    public void UpdateCCImage(int count)
    {
        foreach (GameObject obj in CCPrefabList)
        {
            obj.GetComponent<CompanionCount>().ImageChange(count > 0);
            --count;
        }
    }
}
