using UnityEngine;

public class StageSelectScene : MonoBehaviour
{
    private void Awake()
    {

    }
    private void Start()
    {
        AudioManager.Instance.PlayTitleBGM(); // audioManager를 위한 코드 추가
        Invoke("NextScene", 1.5f);
    }

    public void NextScene()
    {
        GameSceneManager.Instance.ChangeScene("4_Intro");
    }
}
