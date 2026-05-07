using UnityEngine;

public class PlatformNormal : MonoBehaviour
{
    [SerializeField] public GameObject platformNormal;

    void Awake()
    {
        platformNormal = GameObject.FindGameObjectWithTag("Platform_Noraml");
    }

    void Start()
    {

    }
}
