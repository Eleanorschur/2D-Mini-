using System.Collections.Generic;
using UnityEngine;

public class PlatformNormal : MonoBehaviour
{
    [SerializeField] List<GameObject> platformList = new();

    void Awake()
    {

    }

    void Start()
    {
        platformList.Clear();
        GameObject[] normalPlatforms = GameObject.FindGameObjectsWithTag("Platform_Normal");
        platformList.AddRange(normalPlatforms);
    }
}
