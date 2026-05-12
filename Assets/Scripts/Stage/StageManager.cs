using UnityEngine;

public class StageManager : MonoBehaviour
{
    private Timer timer;
    private StageReset stageReset;

    private bool pauseGame = false;

    void Awake()
    {
        stageReset = GetComponent<StageReset>();
    }

    void Start()
    {
        timer = FindAnyObjectByType<Timer>();

        pauseGame = false;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if ( ! Input.GetKeyDown(KeyCode.Escape))
            return;

        if (pauseGame == false)
        {
            pauseGame = true;
            stageReset.ResetLock(true);
            timer.PauseTimer();
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            Debug.Log("ESC 팝업창 열기");
        }
        else
        {
            pauseGame = false;
            stageReset.ResetLock(false);
            timer.ResumeTimer();
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            Debug.Log("ESC 팝업창 닫기");
        }
    }
}
