using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LeverManager : MonoBehaviour
{
    public MapLoader mapLoader;
    private ExitDoor exitDoor;
    private BoxCollider2D coll2D;
    private Lever lever;

    [SerializeField] private List<GameObject> leverList = new();
    [SerializeField] private int leverCurrentCount = 0;

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
        leverCurrentCount = 0;
        StartCoroutine(CollectLeverRoutine());
    }

    private IEnumerator CollectLeverRoutine()
    {
        yield return null; // Destroy가 완료될 때까지 한 프레임 대기

        leverList.Clear();
        leverCurrentCount = 0;

        foreach (Transform child in transform)
        {
            if (child.CompareTag("Lever"))
            {
                leverList.Add(child.gameObject);
            }
        }

        exitDoor = FindAnyObjectByType<ExitDoor>();
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
