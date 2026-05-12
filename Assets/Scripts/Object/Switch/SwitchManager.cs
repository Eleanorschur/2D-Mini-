using UnityEngine;

public class SwitchManager : MonoBehaviour
{
    private SwitchRed switchRed;
    private SwitchBlue switchBlue;

    void Awake()
    {

    }

    void Start()
    {
        switchRed = GetComponentInChildren<SwitchRed>();
        switchBlue = GetComponentInChildren<SwitchBlue>();
    }

    public void AllSwitchReset()
    {
        switchRed.SwitchReset();
        switchBlue.SwitchReset();
    }
}
