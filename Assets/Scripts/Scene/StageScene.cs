using UnityEngine;

public class StageScene : MonoBehaviour
{
    void Start()
    {
        // audioManager를 위한 코드 추가
        MapLoader mapLoader = FindAnyObjectByType<MapLoader>();
        if (mapLoader != null)
            mapLoader.MapLoadComplete += OnFirstStageLoaded;
    }

    // audioManager를 위한 코드 추가
    private void OnFirstStageLoaded()
    {
        MapLoader mapLoader = FindAnyObjectByType<MapLoader>();
        if (mapLoader != null)
            mapLoader.MapLoadComplete -= OnFirstStageLoaded;

        Timer timer = FindAnyObjectByType<Timer>();
        if (timer != null)
            AudioManager.Instance.StartTimerTracking(timer.defaultTime, 10);
    }
}
