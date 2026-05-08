using UnityEngine;

public class ExitManager : MonoBehaviour
{
    public ExitDoor exit { get; private set; }
    public System.Action OnReadyExit;

    void Awake()
    {
        
    }

    void Start()

    {
        
    }

    public void SetExitObj()
    {
        exit = FindAnyObjectByType<ExitDoor>();

        if (exit != null)
            OnReadyExit?.Invoke();
    }

    public ExitDoor GetExitObj()
    {
        return exit;
    }
}
