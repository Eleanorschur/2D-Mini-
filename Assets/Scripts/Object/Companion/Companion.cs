using UnityEngine;

public class Companion : MonoBehaviour
{
    private CompanionManager companionManager;
    private FollowPlayer followPlayer;
    private Movement playerMovement;
    private ZKey Zkey;
    private ZKey currentZKey;

    private bool nearCompanion;
    private bool activeCompanion;
    public bool ActiveCompanion => activeCompanion;

    void Awake()
    {
        companionManager = GetComponentInParent<CompanionManager>();
        followPlayer = GetComponent<FollowPlayer>();
        playerMovement = FindAnyObjectByType<Movement>();
        Zkey = FindAnyObjectByType<ZKey>();
    }

    void Start()
    {
        nearCompanion = false;
        activeCompanion = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            nearCompanion = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if ( ! collision.CompareTag("Player")) return;

        nearCompanion = false;

        if (currentZKey != null)
        {
            currentZKey.Hide();
            currentZKey = null;
        }
    }

    void Update()
    {
        if (activeCompanion) return;

        if (nearCompanion && playerMovement.IsGrounded && currentZKey == null)
        {
            currentZKey = ZKeyPool.Instance.GetZKey();
            currentZKey.Setup(this.transform);
        }

        if (currentZKey != null && !playerMovement.IsGrounded)
        {
            currentZKey.Hide();
            currentZKey = null;
        }

        if (Input.GetKeyDown(KeyCode.Z) && nearCompanion && playerMovement.IsGrounded)
        {
            activeCompanion = true;
            followPlayer.SetIndex(companionManager.GetIndex(this.gameObject));

            if (currentZKey != null)
            {
                currentZKey.Hide();
                currentZKey = null;
            }
        }
    }

    public void CompanionReset(Vector3 position)
    {
        nearCompanion = false;
        activeCompanion = false;
        transform.position = position;
    }
}
