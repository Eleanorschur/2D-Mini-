using UnityEngine;

public class AnimationListner : MonoBehaviour
{
    private PlayerManager playerManager;
    private Animator animator;

    void Awake()
    {
        //animator = GetComponent<Animator>();
    }

    void Start()
    {
        playerManager = GetComponentInParent<PlayerManager>();
    }
}
