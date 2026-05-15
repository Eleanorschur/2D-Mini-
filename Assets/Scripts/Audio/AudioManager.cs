using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

/// <summary>
/// 전역 오디오 매니저. DontDestroyOnLoad 싱글톤.
/// GameStart 씬의 빈 오브젝트에 단 한 번만 배치하세요.
/// </summary>
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    // ──────────────────────────────────────────────
    // Inspector
    // ──────────────────────────────────────────────

    [Header("Audio Mixer")]
    [SerializeField] private AudioMixer audioMixer;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private AudioSource sfxSource;

    [Header("BGM Clips")]
    [Tooltip("인덱스 = 스테이지 번호 (0부터 시작)")]
    [SerializeField] private AudioClip[] stageBGMs;
    [SerializeField] private AudioClip titleBGM;
    [SerializeField] private AudioClip introBGM;
    [SerializeField] private AudioClip endingBGM;

    [Header("UI SFX")]
    [SerializeField] private AudioClip buttonClickSFX;

    [Header("Player SFX")]
    [SerializeField] private AudioClip jumpSFX;
    [SerializeField] private AudioClip landSFX;
    [SerializeField] private AudioClip deathSFX;

    [Header("Interaction SFX")]
    [SerializeField] private AudioClip switchSFX;
    [SerializeField] private AudioClip leverSFX;
    [SerializeField] private AudioClip exitOpenSFX;
    [SerializeField] private AudioClip exitEnterSFX;
    [SerializeField] private AudioClip checkpointSFX;
    [SerializeField] private AudioClip companionRecruitSFX;

    [Header("Stage SFX")]
    [SerializeField] private AudioClip timerWarningSFX;
    [SerializeField] private AudioClip stageClearSFX;

    [Header("BGM Settings")]
    [SerializeField] private float bgmFadeTime = 1f;

    // ──────────────────────────────────────────────
    // Volume State
    // ──────────────────────────────────────────────

    private float masterVolume = 1f;
    private float bgmVolume    = 1f;
    private float sfxVolume    = 1f;

    private bool isMasterMuted = false;
    private bool isBGMMuted    = false;
    private bool isSFXMuted    = false;

    private const float MinVolume    = 0.0001f;
    private const float MuteDb       = -80f;
    private const float OffThreshold = 0.001f;

    // PlayerPrefs 키
    private const string PrefMasterVol  = "MasterVolume";
    private const string PrefBGMVol     = "BGMVolume";
    private const string PrefSFXVol     = "SFXVolume";
    private const string PrefMasterMute = "MasterMute";
    private const string PrefBGMMute    = "BGMMute";
    private const string PrefSFXMute    = "SFXMute";

    // ──────────────────────────────────────────────
    // Coroutines
    // ──────────────────────────────────────────────

    private Coroutine bgmFadeCoroutine;
    private Coroutine timerTrackingCoroutine;

    // ──────────────────────────────────────────────
    // Lifecycle
    // ──────────────────────────────────────────────

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        LoadVolumeSettings();
    }

    private void Start()
    {
        ApplyAllVolumes();
    }

    // ──────────────────────────────────────────────
    // BGM
    // ──────────────────────────────────────────────

    public void PlayTitleBGM()  => PlayBGM(titleBGM);
    public void PlayIntroBGM()  => PlayBGM(introBGM);
    public void PlayEndingBGM() => PlayBGM(endingBGM);

    public void PlayStageBGM(int stageIndex)
    {
        if (stageBGMs == null || stageIndex < 0 || stageIndex >= stageBGMs.Length)
        {
            Debug.LogWarning($"[AudioManager] stageBGMs 배열에 인덱스 {stageIndex}가 없습니다.");
            return;
        }
        PlayBGM(stageBGMs[stageIndex]);
    }

    public void PlayBGM(AudioClip clip)
    {
        if (clip == null || bgmSource == null) return;
        if (bgmSource.clip == clip && bgmSource.isPlaying) return;

        if (bgmFadeCoroutine != null)
            StopCoroutine(bgmFadeCoroutine);

        bgmFadeCoroutine = StartCoroutine(CrossFadeBGM(clip, bgmFadeTime));
    }

    public void StopBGM()
    {
        if (bgmFadeCoroutine != null)
            StopCoroutine(bgmFadeCoroutine);

        bgmFadeCoroutine = StartCoroutine(FadeOutAndStop(bgmFadeTime));
    }

    private IEnumerator CrossFadeBGM(AudioClip newClip, float fadeTime)
    {
        float halfTime   = fadeTime * 0.5f;
        float targetVol  = GetLinearBGMVolume(); // 실제 BGM 볼륨 반영

        // Fade out
        if (bgmSource.isPlaying)
        {
            float startVol = bgmSource.volume;
            float elapsed  = 0f;
            while (elapsed < halfTime)
            {
                elapsed          += Time.deltaTime;
                bgmSource.volume  = Mathf.Lerp(startVol, 0f, elapsed / halfTime);
                yield return null;
            }
            bgmSource.Stop();
        }

        // Fade in
        bgmSource.clip   = newClip;
        bgmSource.loop   = true;
        bgmSource.volume = 0f;
        bgmSource.Play();

        float elapsed2 = 0f;
        while (elapsed2 < halfTime)
        {
            elapsed2         += Time.deltaTime;
            bgmSource.volume  = Mathf.Lerp(0f, targetVol, elapsed2 / halfTime);
            yield return null;
        }
        bgmSource.volume = targetVol;
        bgmFadeCoroutine = null;
    }

    private IEnumerator FadeOutAndStop(float fadeTime)
    {
        float startVol = bgmSource.volume;
        float elapsed  = 0f;
        while (elapsed < fadeTime)
        {
            elapsed          += Time.deltaTime;
            bgmSource.volume  = Mathf.Lerp(startVol, 0f, elapsed / fadeTime);
            yield return null;
        }
        bgmSource.Stop();
        bgmSource.volume = GetLinearBGMVolume();
        bgmFadeCoroutine = null;
    }

    // ──────────────────────────────────────────────
    // SFX — UI
    // ──────────────────────────────────────────────

    /// <summary>버튼 클릭 효과음</summary>
    public void PlayButtonClick() => PlaySFX(buttonClickSFX);

    // ──────────────────────────────────────────────
    // SFX — Player
    // ──────────────────────────────────────────────

    public void PlayJump()  => PlaySFX(jumpSFX);
    public void PlayLand()  => PlaySFX(landSFX);
    public void PlayDeath() => PlaySFX(deathSFX);

    // ──────────────────────────────────────────────
    // SFX — Interaction
    // ──────────────────────────────────────────────

    public void PlaySwitch()           => PlaySFX(switchSFX);
    public void PlayLever()            => PlaySFX(leverSFX);
    public void PlayExitOpen()         => PlaySFX(exitOpenSFX);
    public void PlayExitEnter()        => PlaySFX(exitEnterSFX);
    public void PlayCheckpoint()       => PlaySFX(checkpointSFX);
    public void PlayCompanionRecruit() => PlaySFX(companionRecruitSFX);

    // ──────────────────────────────────────────────
    // SFX — Stage
    // ──────────────────────────────────────────────

    public void PlayStageClear() => PlaySFX(stageClearSFX);

    // ──────────────────────────────────────────────
    // Timer Tracking
    // ──────────────────────────────────────────────

    /// <summary>
    /// 타이머 추적 시작. (totalTime - warningTime)초 후 경고음 자동 재생.
    /// </summary>
    public void StartTimerTracking(int totalTime, int warningTime)
    {
        if (timerTrackingCoroutine != null)
            StopCoroutine(timerTrackingCoroutine);

        float waitSeconds = Mathf.Max(0f, totalTime - warningTime);
        timerTrackingCoroutine = StartCoroutine(TimerWarningRoutine(waitSeconds));
    }

    /// <summary>타이머 추적 중단.</summary>
    public void StopTimerTracking()
    {
        if (timerTrackingCoroutine != null)
        {
            StopCoroutine(timerTrackingCoroutine);
            timerTrackingCoroutine = null;
        }
    }

    private IEnumerator TimerWarningRoutine(float waitSeconds)
    {
        yield return new WaitForSeconds(waitSeconds);
        PlaySFX(timerWarningSFX);
        timerTrackingCoroutine = null;
    }

    // ──────────────────────────────────────────────
    // Volume Control — 옵션 UI에서 직접 연결
    // ──────────────────────────────────────────────

    /// <summary>마스터 볼륨 (0~1)</summary>
    public void SetMasterVolume(float value)
    {
        masterVolume = Mathf.Clamp01(value);
        PlayerPrefs.SetFloat(PrefMasterVol, masterVolume);
        ApplyAllVolumes();
    }

    /// <summary>BGM 볼륨 (0~1)</summary>
    public void SetBGMVolume(float value)
    {
        bgmVolume = Mathf.Clamp01(value);
        PlayerPrefs.SetFloat(PrefBGMVol, bgmVolume);
        ApplyAllVolumes();
        // 현재 재생 중인 BGM 볼륨도 즉시 반영
        if (bgmFadeCoroutine == null && bgmSource != null && bgmSource.isPlaying)
            bgmSource.volume = GetLinearBGMVolume();
    }

    /// <summary>SFX 볼륨 (0~1)</summary>
    public void SetSFXVolume(float value)
    {
        sfxVolume = Mathf.Clamp01(value);
        PlayerPrefs.SetFloat(PrefSFXVol, sfxVolume);
        ApplyAllVolumes();
    }

    /// <summary>마스터 뮤트</summary>
    public void SetMasterMute(bool isMute)
    {
        isMasterMuted = isMute;
        PlayerPrefs.SetInt(PrefMasterMute, isMute ? 1 : 0);
        ApplyAllVolumes();
    }

    /// <summary>BGM 뮤트</summary>
    public void SetBGMMute(bool isMute)
    {
        isBGMMuted = isMute;
        PlayerPrefs.SetInt(PrefBGMMute, isMute ? 1 : 0);
        ApplyAllVolumes();
    }

    /// <summary>SFX 뮤트</summary>
    public void SetSFXMute(bool isMute)
    {
        isSFXMuted = isMute;
        PlayerPrefs.SetInt(PrefSFXMute, isMute ? 1 : 0);
        ApplyAllVolumes();
    }

    // ──────────────────────────────────────────────
    // Volume Getters (옵션 UI 초기값 복원용)
    // ──────────────────────────────────────────────

    public float GetMasterVolume() => masterVolume;
    public float GetBGMVolume()    => bgmVolume;
    public float GetSFXVolume()    => sfxVolume;
    public bool  GetMasterMute()   => isMasterMuted;
    public bool  GetBGMMute()      => isBGMMuted;
    public bool  GetSFXMute()      => isSFXMuted;

    // ──────────────────────────────────────────────
    // Internal
    // ──────────────────────────────────────────────

    private void LoadVolumeSettings()
    {
        masterVolume  = PlayerPrefs.GetFloat(PrefMasterVol,  1f);
        bgmVolume     = PlayerPrefs.GetFloat(PrefBGMVol,     1f);
        sfxVolume     = PlayerPrefs.GetFloat(PrefSFXVol,     1f);
        isMasterMuted = PlayerPrefs.GetInt(PrefMasterMute,   0) == 1;
        isBGMMuted    = PlayerPrefs.GetInt(PrefBGMMute,      0) == 1;
        isSFXMuted    = PlayerPrefs.GetInt(PrefSFXMute,      0) == 1;
    }

    private void ApplyAllVolumes()
    {
        ApplyVolume("Master",    masterVolume, isMasterMuted);
        ApplyVolume("BGMVolume", bgmVolume,    isBGMMuted);
        ApplyVolume("SFXVolume", sfxVolume,    isSFXMuted);
    }

    private void ApplyVolume(string parameter, float volume, bool isMuted)
    {
        if (audioMixer == null) return;
        if (isMuted || volume <= OffThreshold)
            audioMixer.SetFloat(parameter, MuteDb);
        else
            audioMixer.SetFloat(parameter, Mathf.Log10(Mathf.Max(volume, MinVolume)) * 20f);
    }

    /// <summary>크로스페이드에서 사용할 BGM 선형 볼륨값 (뮤트/마스터 뮤트 반영)</summary>
    private float GetLinearBGMVolume()
    {
        if (isMasterMuted || isBGMMuted) return 0f;
        return Mathf.Clamp01(masterVolume * bgmVolume);
    }

    private void PlaySFX(AudioClip clip)
    {
        if (clip == null || sfxSource == null) return;
        sfxSource.PlayOneShot(clip);
    }
}
