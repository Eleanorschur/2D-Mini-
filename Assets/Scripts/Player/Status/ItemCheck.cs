using System.Collections.Generic;
using UnityEngine;

public class ItemCheck : MonoBehaviour
{
    private PlayerManager playerManager;
    private StatusCheck statusCheck;
    private LeverManager leverManager;
    [SerializeField] private List<GameObject> levers;
    [SerializeField] private List<GameObject> switchs_Red;
    [SerializeField] private List<GameObject> switchs_Blue;

    public List<GameObject> Levers => levers;

    private int itemLayer = 0;

    [SerializeField] public int leverCount = 0;

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
        levers.Add(lever);
    }

    public void GetSwitchList()
    {
        switchs_Red.Clear();
        switchs_Blue.Clear();

        GameObject[] redSwitchs = GameObject.FindGameObjectsWithTag("Switch_Red");
        GameObject[] blueSwitchs = GameObject.FindGameObjectsWithTag("Switch_Blue");

        switchs_Red.AddRange(redSwitchs);
        switchs_Blue.AddRange(blueSwitchs);
    }

    public void itemReset()
    {
        levers.Clear();
        leverManager.AllLeverReset();
        leverManager.LeverColliderActive(true);
        leverCount = 0;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer != itemLayer) return;

        string readTag = other.gameObject.tag;
    }
}
