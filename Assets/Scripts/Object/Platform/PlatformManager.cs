using UnityEngine;

public class PlatformManager : MonoBehaviour
{
    private MapLoader mapLoader;
    private PlatformData platformData;
    private PlatformRed redPlatform;
    private PlatformBlue bluePlatform;
    private PlatformNormal normalPlatform;

    private void Awake()
    {
        mapLoader = FindAnyObjectByType<MapLoader>();
        platformData = GetComponent<PlatformData>();
        redPlatform = GetComponentInChildren<PlatformRed>();
        bluePlatform = GetComponentInChildren<PlatformBlue>();
        normalPlatform = GetComponentInChildren<PlatformNormal>();
    }

    private void Start()
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

    private void OnMapLoadFinished()
    {
        redPlatform.PlatformHide(true, platformData.platformHideAlpha);
        bluePlatform.PlatformHide(true, platformData.platformHideAlpha);
    }

    public void SwitchingPlatformHide(int status)
    {
        switch (status)
        {
            case 0:
                redPlatform.PlatformHide(true, platformData.platformHideAlpha);
                bluePlatform.PlatformHide(true, platformData.platformHideAlpha);
                break;
            case 1:
                redPlatform.PlatformHide(false, platformData.platformHideAlpha);
                bluePlatform.PlatformHide(true, platformData.platformHideAlpha);
                break;
            case 2:
                redPlatform.PlatformHide(true, platformData.platformHideAlpha);
                bluePlatform.PlatformHide(false, platformData.platformHideAlpha);
                break;
        }
    }

    public void AllPlatformActive(bool active)
    {
        redPlatform.PlatformActive(active);
        bluePlatform.PlatformActive(active);
    }
}
