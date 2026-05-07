using System.Collections.Generic;
using UnityEngine;

public class CompanionManager : MonoBehaviour
{
    private List<GameObject> companionList = new();
    [SerializeField]private List<Vector3> transformList = new();
    private Companion companion;

    void Awake()
    {
        companion = GetComponentInChildren<Companion>();
    }

    void Start()
    {
        companionList.Clear();
        GameObject[] companions = GameObject.FindGameObjectsWithTag("Companion");
        companionList.AddRange(companions);

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
