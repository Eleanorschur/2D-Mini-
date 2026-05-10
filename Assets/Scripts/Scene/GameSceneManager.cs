using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneManager : MonoBehaviour
{
    // 외부에서 접근 가능하도록 static 인스턴스 선언
    public static GameSceneManager Instance { get; private set; }

    private void Awake()
    {
        // --- 싱글톤 로직 ---
        if (Instance == null)
        {
            Instance = this;
            // 이 오브젝트는 씬이 바뀌어도 파괴되지 않습니다.
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // 이미 인스턴스가 존재한다면 새로 생성된 오브젝트를 파괴합니다.
            Destroy(gameObject);
        }
    }

    // 씬 이동 메서드
    public void ChangeScene(string sceneName)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }

    // 씬 이동 후 특정 로직이 필요할 때 (예: 부활 위치 리셋 등)
    public void ResetAfterSceneLoad()
    {
        Debug.Log("새로운 씬에 맞춰 데이터를 정리합니다.");
    }
}