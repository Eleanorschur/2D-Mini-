using System.Collections.Generic;
using UnityEngine;

public class LeverManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> leverList = new();
    private ExitDoor exitDoor;
    private BoxCollider2D coll2D;
    private Lever lever;

    [SerializeField] private int leverCurrentCount = 0;

    void Awake()
    {
        exitDoor = FindAnyObjectByType<ExitDoor>();
    }

    void Start()
    {
        leverList.Clear();
        GameObject[] levers = GameObject.FindGameObjectsWithTag("DoorLever");
        leverList.AddRange(levers);
    }

    public void leverAddCounter()
    {
        ++leverCurrentCount;

        if (leverCurrentCount >= leverList.Count)
            exitDoor.DoorOpen(true);
    }

    public void LeverColliderActive(bool active)
    {
        foreach (GameObject lever in leverList)
        {
            coll2D = lever.GetComponent<BoxCollider2D>();
            coll2D.enabled = active; 
        }
    }

    public void AllLeverReset()
    {
        foreach (GameObject obj in leverList)
        {
            lever = obj.GetComponent<Lever>();
            lever.LeverReset();
        }
    }
}
