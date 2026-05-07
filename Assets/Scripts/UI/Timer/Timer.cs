using UnityEngine;
using System.Collections;
using TMPro;

public class Timer : MonoBehaviour
{
    private TextMeshProUGUI timerText;
    private StageReset stageReset;
    private Movement playerMovement;
    private Coroutine countdownCoroutine;

    public int defaultTime = 60;
    private int currentTime = 0;
    private int warningTime = 10;

    void Awake()
    {
        stageReset = FindAnyObjectByType<StageReset>();
        playerMovement = FindAnyObjectByType<Movement>();

        if (timerText == null)
            timerText = GetComponentInChildren<TextMeshProUGUI>();
    }

    void Start()
    {
        currentTime = defaultTime;
        UpdateText(currentTime);
        countdownCoroutine = StartCoroutine(CountDown());
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

    private void TimeOver()
    {
        playerMovement.MoveLock(true);
        stageReset.ResetLock(true);
        timerText.text = "OVER";
        Debug.Log("타임 오버");
    }
}
