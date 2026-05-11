using UnityEngine;

public class AnimationListner : MonoBehaviour
{
    private PlayerManager playerManager;
    private Animator animator;

    void Awake()
    {
        playerManager = GetComponentInParent<PlayerManager>();
        //animator = GetComponent<Animator>();
    }

    void OnEnable()
    {
        if (playerManager != null)
            playerManager.PlayerLoadComplete += UpdatePlayerReference;
    }

    void UpdatePlayerReference()
    {
        GameObject newPlayer = playerManager.GetPlayerObj();
        //animator = newPlayer.GetComponent<Animator>();
    }

    void Start()
    {

    }
}
