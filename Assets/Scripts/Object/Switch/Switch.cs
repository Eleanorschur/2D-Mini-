using UnityEngine;

public class Switch : MonoBehaviour
{
    private PlayerManager playerManager;
    private SwitchRed switchRed;
    private SwitchBlue switchBlue;
    private StatusCheck statusCheck;

    public int switchNumber;

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
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        statusCheck.ChangeForm(switchNumber);

        if (switchNumber == 1)
            switchRed.Switching();
        else if (switchNumber == 2)
            switchBlue.Switching();

        AudioManager.Instance.PlaySwitch(); // audioManager를 위한 코드 추가
    }

    public void SwitchReset()
    {
    }
}
