using UnityEngine;

public class TitleScene : MonoBehaviour
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
        SceneManager.Instance.ChangeScene("3_StageSelect");
    }
}
