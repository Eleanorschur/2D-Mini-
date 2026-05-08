using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public GameObject player { get; private set; }
    public System.Action OnReadyPlayer;

    void Awake()
    {
        
    }

    void Start()
    {
 
    }

    public void SetPlayerObj()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
            OnReadyPlayer?.Invoke();
    }

    public GameObject GetPlayerObj()
    {
        return player;
    }
}
