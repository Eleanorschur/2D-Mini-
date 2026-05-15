using UnityEngine;

public class IntroScene : MonoBehaviour
{
    private void Awake()
    {

    }
    private void Start()
    {
        AudioManager.Instance.PlayIntroBGM(); // audioManager를 위한 코드 추가
        Invoke("NextScene", 1.5f);
    }

    public void NextScene()
    {
        GameSceneManager.Instance.ChangeScene("5_Stage");
    }
}
