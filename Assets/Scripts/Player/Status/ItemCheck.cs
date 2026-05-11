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
        playerManager = GetComponentInParent<PlayerManager>();
        statusCheck = GetComponent<StatusCheck>();
        leverManager = FindAnyObjectByType<LeverManager>();
    }

    void Start()
    {
        itemLayer = LayerMask.NameToLayer("Item");
        
        statusCheck.ChangeForm(0);
        GetSwitchList();
    }

    void OnEnable()
    {
        if (playerManager != null)
            playerManager.PlayerLoadComplete += UpdatePlayerReference;
    }

    void UpdatePlayerReference()
    {
        GameObject newPlayer = playerManager.GetPlayerObj();
        statusCheck = newPlayer.GetComponent<StatusCheck>();
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
