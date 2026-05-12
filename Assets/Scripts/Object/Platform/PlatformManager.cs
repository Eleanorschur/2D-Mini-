using UnityEngine;

public class PlatformManager : MonoBehaviour
{
    private PlatformData platformData;
    [SerializeField]private PlatformRed redPlatform;
    [SerializeField]private PlatformBlue bluePlatform;
    [SerializeField]private PlatformNormal normalPlatform;

    private void Awake()
    {
        platformData = GetComponent<PlatformData>();
    }

    private void Start()
    {
        redPlatform = GetComponentInChildren<PlatformRed>();
        bluePlatform = GetComponentInChildren<PlatformBlue>();
        normalPlatform = GetComponentInChildren<PlatformNormal>();
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
