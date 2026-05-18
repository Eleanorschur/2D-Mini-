using UnityEngine;

public class Player : MonoBehaviour
{
    private PlayerManager playerManager;

    void Awake()
    {
       
    }
    void Start()
    {
        playerManager = GetComponentInParent<PlayerManager>();

        if (playerManager != null)
            playerManager.RegisterPlayer(this.gameObject);
    }

}

