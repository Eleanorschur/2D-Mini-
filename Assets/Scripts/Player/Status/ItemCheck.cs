using UnityEngine;
using System.Collections.Generic;

public class ItemCheck : MonoBehaviour
{
    private LeverManager leverManager;
    private CompanionCountPool CCCountPool;

    [SerializeField] private List<GameObject> leversList;
    [SerializeField] private List<GameObject> SwitchList;
    [SerializeField] private List<GameObject> companionList;

    public List<GameObject> LeverList => leversList;

    private int itemLayer = 0;

    [SerializeField] public int leverCount = 0;
    [SerializeField] public int companionCount = -1;

    void Awake()
    {

    }

    void Start()
    {
        leverManager = FindAnyObjectByType<LeverManager>();
        CCCountPool = FindAnyObjectByType<CompanionCountPool>();
    }

    public void AddLeverList(GameObject lever)
    {
        leversList.Add(lever);
    }

    public void AddSwitchList(GameObject sw)
    {
        SwitchList.Add(sw);
    }

    public int AddCompanionList(GameObject companion)
    {
        ++companionCount;
        companionList.Add(companion);
        CCCountPool.UpdateCCImage(companionCount + 1);

        return companionCount;
    }

    public void itemReset()
    {
        leversList.Clear();
        leverManager.AllLeverReset();
        leverManager.LeverColliderActive(true);
        leverCount = 0;

        companionList.Clear();
        companionCount = -1;
        CCCountPool.UpdateCCImage(companionCount + 1);

        SwitchList.Clear();
    }

    public void CheckpointReset()
    {
        foreach (GameObject sw in SwitchList)
        {
            sw.gameObject.GetComponent<Switch>().SwitchActive(true);
        }
        SwitchList.Clear();

        foreach (GameObject lv in leversList)
        {
            lv.gameObject.GetComponent<Lever>().LeverReset();
        }
        leversList.Clear();
        leverManager.UpdateCurrentLever();

        int ccCount = 0;
        foreach (GameObject cc in companionList)
        {
            ++ccCount;
            cc.gameObject.GetComponent<Companion>().CompanionReset(Vector3.zero);
        }
        companionCount = companionCount - ccCount;
        CCCountPool.UpdateCCImage(companionCount + 1);
        companionList.Clear();
    }

    public void ListClear()
    {
        SwitchList.Clear();
        leversList.Clear();
        companionList.Clear();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer != itemLayer) return;

        string readTag = other.gameObject.tag;
    }
}
