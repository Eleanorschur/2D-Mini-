using UnityEngine;
using System.Collections;
using TMPro;

public class Timer : MonoBehaviour
{
    private TextMeshProUGUI timerText;
    private PlayerManager playerManager;
    private StageReset stageReset;
    private Movement playerMovement;
    private Coroutine countdownCoroutine;

    public int defaultTime = 60;
    private int currentTime = 0;
    private int warningTime = 10;

    void Awake()
    {

    }

    void OnEnable()
    {
        playerManager = FindAnyObjectByType<PlayerManager>();

        if (playerManager != null)
            playerManager.PlayerLoadComplete += PlayerLoadComplete;
    }

    void OnDisable()
    {
        if (playerManager != null)
            playerManager.PlayerLoadComplete -= PlayerLoadComplete;
    }

    void Start()
    {
        stageReset = FindAnyObjectByType<StageReset>();
        timerText = GetComponentInChildren<TextMeshProUGUI>();

        currentTime = defaultTime;
        UpdateText(currentTime);
        countdownCoroutine = StartCoroutine(CountDown());
    }

    private void PlayerLoadComplete()
    {
        playerMovement = playerManager.GetPlayerObj().GetComponent<Movement>();
    }

    private IEnumerator CountDown()
    {
        while (currentTime > 0)
        {
            yield return new WaitForSeconds(1.0f);
            currentTime--;
            UpdateText(currentTime);

            yield return null;
        }

        if (currentTime <= 0)
            TimeOver();
    }

    private void UpdateText(int currentTime)
    {
        timerText.text = currentTime.ToString();

        if (currentTime <= warningTime)
            timerText.color = Color.red;
        else
            timerText.color = Color.white;
    }

    public void PauseTimer()
    {
        if (countdownCoroutine != null)
        {
            StopCoroutine(countdownCoroutine);
            countdownCoroutine = null;
        }
    }

    public void ResumeTimer()
    {
        if (countdownCoroutine == null && currentTime > 0)
        {
            countdownCoroutine = StartCoroutine(CountDown());
        }
    }

    public void StopTimer()
    {
        if (countdownCoroutine != null)
        {
            StopCoroutine(countdownCoroutine);
            countdownCoroutine = null;
            currentTime = 0;
        }
    }

    public void ResetTimer()
    {
        currentTime = defaultTime;
        UpdateText(currentTime);
        ResumeTimer();
    }

    private void TimeOver()
    {
        playerMovement.MoveLock(true);
        stageReset.ResetLock(true);
        timerText.text = "OVER";
        Debug.Log("타임 오버");
    }
}
