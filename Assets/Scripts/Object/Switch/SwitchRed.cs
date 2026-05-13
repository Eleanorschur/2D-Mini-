using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchRed : MonoBehaviour
{
    public MapLoader mapLoader;
    private PlatformManager platformManager;

    [SerializeField] private List<GameObject> switchRedList = new();

    void Awake()
    {

    }

    void OnEnable()
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
        StartCoroutine(CollectSwitchRoutine());
    }

    private IEnumerator CollectSwitchRoutine()
    {
        yield return null; // Destroy가 완료될 때까지 한 프레임 대기

        switchRedList.Clear();

        foreach (Transform child in transform)
        {
            if (child.CompareTag("Switch_Red"))
            {
                switchRedList.Add(child.gameObject);
            }
        }

        platformManager = FindAnyObjectByType<PlatformManager>();
    }

    public void Switching()
    {
        platformManager.SwitchingPlatformHide(1);
    }

    public void SwitchReset()
    {
        foreach (GameObject redSwitch in switchRedList)
        {
            redSwitch.GetComponent<Switch>().SwitchActive(true);
        }
    }
}
