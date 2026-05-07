using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGameButton : MonoBehaviour
{
    public void OnClickStartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("5. StageSelect");
    }
}