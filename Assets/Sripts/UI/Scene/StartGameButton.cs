using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGameButton : MonoBehaviour
{
    public void OnClickStartGame()
    {
        SceneManager.LoadScene("5. StageSelect");
    }
}