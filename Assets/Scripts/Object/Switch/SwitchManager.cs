using System.Collections.Generic;
using UnityEngine;

public class SwitchManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> switchRedList = new();
    [SerializeField] private List<GameObject> switchBlueList = new();

    void Awake()
    {
        
    }

    void Start()
    {
        switchRedList.Clear();
        switchBlueList.Clear();

        GameObject[] redSwitchs = GameObject.FindGameObjectsWithTag("Switch_Red");
        GameObject[] blueSwitchs = GameObject.FindGameObjectsWithTag("Switch_Blue");

        switchRedList.AddRange(redSwitchs);
        switchBlueList.AddRange(blueSwitchs);
    }

    public void AllSwitchReset()
    {
        foreach (GameObject redSwitch in switchRedList)
        {
            Switch rs = redSwitch.GetComponent<Switch>();
            rs.SwitchReset();
        }

        foreach (GameObject blueSwitch in switchBlueList)
        {
            Switch bs = blueSwitch.GetComponent<Switch>();
            bs.SwitchReset();
        }
    }
}
