using UnityEngine;

public class DecorationManager : MonoBehaviour
{
    public MapLoader mapLoader;
    
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

    }
}
