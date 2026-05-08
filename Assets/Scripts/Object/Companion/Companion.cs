using UnityEngine;
using System.Collections;

public class Companion : MonoBehaviour
{
    private MapLoader mapLoader;
    private PlayerManager playerManager;
    private CompanionManager companionManager;
    private FollowPlayer followPlayer;
    private Movement playerMovement;
    private ZKey currentZKey;

    private bool nearCompanion;
    private bool activeCompanion;
    public bool ActiveCompanion => activeCompanion;

    void Awake()
    {
        mapLoader = FindAnyObjectByType<MapLoader>();
        playerManager = FindAnyObjectByType<PlayerManager>();
        companionManager = GetComponentInParent<CompanionManager>();
        followPlayer = GetComponent<FollowPlayer>();
    }

    void OnEnable()
    {
        if (mapLoader != null)
            mapLoader.MapLoadComplete += OnMapLoadFinished;

        if (playerManager != null)
            playerManager.PlayerLoadComplete += PlayerLoadComplete;
    }

    void OnDisable()
    {
        if (mapLoader != null)
            mapLoader.MapLoadComplete -= OnMapLoadFinished;

        if (playerManager != null)
            playerManager.PlayerLoadComplete -= PlayerLoadComplete;
    }

    private void OnMapLoadFinished() { }

    private void PlayerLoadComplete()
    {
        playerMovement = playerManager.GetPlayerObj().GetComponent<Movement>();
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
