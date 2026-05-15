using UnityEngine;

public class Switch : MonoBehaviour
{
    private PlayerManager playerManager;
    private SwitchRed switchRed;
    private SwitchBlue switchBlue;
    private StatusCheck statusCheck;
    private ItemCheck itemCheck;

    public int switchNumber;

    void Awake()
    {

    }

    void OnEnable()
    {
        playerManager = FindAnyObjectByType<PlayerManager>();

        if (playerManager != null)
            playerManager.PlayerLoadComplete += PlayerLoadComplete;
    }

    void OnDisable()
    {
        if (playerManager != null)
            playerManager.PlayerLoadComplete -= PlayerLoadComplete;
    }

    void Start()
    {
        switchRed = GetComponentInParent<SwitchRed>();
        switchBlue = GetComponentInParent<SwitchBlue>();
    }

    private void PlayerLoadComplete()
    {
        statusCheck = playerManager.GetPlayerObj().GetComponent<StatusCheck>();
        itemCheck = playerManager.GetPlayerObj().GetComponent<ItemCheck>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player"))
            return;

        statusCheck.ChangeForm(switchNumber);

        if (switchNumber == 1)
            switchRed.Switching();
        else if (switchNumber == 2)
            switchBlue.Switching();

        itemCheck.AddSwitchList(this.gameObject);

        this.gameObject.SetActive(false);
    }

    public void SwitchActive(bool active)
    {
        this.gameObject.SetActive(active);
    }
}
