using UnityEngine;

public class SwitchManager : MonoBehaviour
{
    private SwitchRed switchRed;
    private SwitchBlue switchBlue;

    void Awake()
    {
        switchRed = GetComponentInChildren<SwitchRed>();
        switchBlue = GetComponentInChildren<SwitchBlue>();
    }

    void Start()
    {
        
    }

    public void AllSwitchReset()
    {
        switchRed.SwitchReset();
        switchBlue.SwitchReset();
    }
}
