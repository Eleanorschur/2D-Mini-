using UnityEngine;

public class GameStartScene : MonoBehaviour
{
    private void Awake()
    {
        
    }
    private void Start()
    {
        Invoke("NextScene", 1.5f);
    }

    public void NextScene()
    {
        GameSceneManager.Instance.ChangeScene("1_Logo");
    }
}
