using UnityEngine;

public class StageSelectScene : MonoBehaviour
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
        GameSceneManager.Instance.ChangeScene("4_Intro");
    }
}
