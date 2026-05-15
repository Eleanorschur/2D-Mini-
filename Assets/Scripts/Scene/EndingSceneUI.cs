using UnityEngine;
using UnityEngine.SceneManagement;

public class EndingSceneUI : MonoBehaviour
{
    [SerializeField] private string titleSceneName = "2_Title";

    public void GoToTitle()
    {
        Debug.Log("BACK TITLE 버튼 클릭됨");
        SceneManager.LoadScene(titleSceneName);
    }
}