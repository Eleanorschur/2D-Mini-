using UnityEngine;

public class IntroScene : MonoBehaviour
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
        SceneManager.Instance.ChangeScene("5_Stage");
    }
}
