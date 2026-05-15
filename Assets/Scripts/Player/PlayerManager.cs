using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public MapLoader mapLoader;
    [SerializeField]private GameObject player;
    private SettingCamera settingCamera;

    public bool isPlayerLoaded { get; private set; }
    public System.Action PlayerLoadComplete;

    void Awake()
    {

    }

    private void OnEnable()
    {
        if (mapLoader != null)
            mapLoader.MapLoadComplete += OnMapLoadFinished;
    }

    void OnDisable()
    {
        if (mapLoader != null)
            mapLoader.MapLoadComplete -= OnMapLoadFinished;
    }

    void Start()
    {

    }

    private void OnMapLoadFinished()
    {
        RegisterPlayer(GetComponentInChildren<Player>().gameObject);
    }

    public void RegisterPlayer(GameObject playerObj)
    {
        player = playerObj;
        isPlayerLoaded = true;
        PlayerLoadComplete?.Invoke();

        settingCamera = FindAnyObjectByType<SettingCamera>();
        settingCamera.PlayerLoadComplete(playerObj);
    }

    public GameObject GetPlayerObj()
    {
        if (player == null)
        {
            Debug.LogError("PlayerManager: 플레이어가 등록되지 않았습니다.");
            return null;
        }

        return player;
    }
}
