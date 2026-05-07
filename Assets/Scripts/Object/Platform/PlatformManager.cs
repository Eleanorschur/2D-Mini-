using UnityEngine;

public class PlatformManager : MonoBehaviour
{
    private PlatformData platformData;
    private PlatformRed redPlatform;
    private PlatformBlue bluePlatform;
    private PlatformNormal normalPlatform;
    public GameObject revivePlatform;

    private void Awake()
    {
        platformData = GetComponent<PlatformData>();
        redPlatform = GetComponentInChildren<PlatformRed>();
        bluePlatform = GetComponentInChildren<PlatformBlue>();
        normalPlatform = GetComponentInChildren<PlatformNormal>();

        if (normalPlatform != null)
        {
            revivePlatform = normalPlatform.gameObject;
        }
    }

    private void Start()
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
