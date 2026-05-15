using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CompanionManager : MonoBehaviour
{
    public MapLoader mapLoader;
    
    [SerializeField] private CompanionIconUI companionIconUI;

    [SerializeField] private List<GameObject> companionList = new();
    
    private List<Vector3> transformList = new();

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

        GetTransformList();

        if (companionIconUI != null)
            companionIconUI.ResetIcons();
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

        if (companionIconUI != null)
            companionIconUI.ResetIcons();
    }

    public int GetRescuedCompanionCount()
    {
        int count = 0;

        foreach (GameObject companionObj in companionList)
        {
            Companion companion = companionObj.GetComponent<Companion>();

            if (companion != null && companion.ActiveCompanion)
            {
                count++;
            }
        }

        return count;
    }

    public void UpdateCompanionIconUI()
    {
        if (companionIconUI == null)
            return;

        companionIconUI.UpdateIcon(GetRescuedCompanionCount());
    }


}
