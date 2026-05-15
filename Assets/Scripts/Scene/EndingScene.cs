using UnityEngine;

public class EndingScene : MonoBehaviour
{
    void Start()
    {
        AudioManager.Instance.PlayEndingBGM(); // audioManager를 위한 코드 추가
    }
}
