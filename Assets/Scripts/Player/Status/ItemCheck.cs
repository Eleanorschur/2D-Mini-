using UnityEngine;
using System.Collections.Generic;

public class ItemCheck : MonoBehaviour
{
    private PlayerManager playerManager;
    private StatusCheck statusCheck;
    private LeverManager leverManager;

    [SerializeField] private List<GameObject> leversList;
    [SerializeField] private List<GameObject> redSwitchList;
    [SerializeField] private List<GameObject> blueSwitchList;
    [SerializeField] private List<GameObject> companionList;

    public List<GameObject> LeverList => leversList;

    private int itemLayer = 0;

    [SerializeField] public int leverCount = 0;
    [SerializeField] public int companionCount = -1;

    void Awake()
    {
        statusCheck = GetComponent<StatusCheck>(); // 같은 계층
    }

    void Start()
    {
        playerManager = GetComponentInParent<PlayerManager>();
        leverManager = FindAnyObjectByType<LeverManager>();

        GetSwitchList();
    }

    public void AddLeverList(GameObject lever)
    {
        leversList.Add(lever);
    }

    public void GetSwitchList()
    {
        redSwitchList.Clear();
        blueSwitchList.Clear();
        GameObject[] redSwitchs = GameObject.FindGameObjectsWithTag("Switch_Red");
        GameObject[] blueSwitchs = GameObject.FindGameObjectsWithTag("Switch_Blue");
        redSwitchList.AddRange(redSwitchs);
        blueSwitchList.AddRange(blueSwitchs);
    }

    public int AddCompanionList(GameObject companion)
    {
        ++companionCount;
        companionList.Add(companion);

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
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer != itemLayer) return;

        string readTag = other.gameObject.tag;
    }
}
