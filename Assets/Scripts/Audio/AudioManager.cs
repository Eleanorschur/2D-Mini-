using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Mixer")]
    [SerializeField] private AudioMixer audioMixer;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private AudioSource sfxSource;

    [Header("Fade")]
    [SerializeField] private float fadeDuration = 0.5f;

    private AudioClipData clipData;
    private Coroutine fadeCoroutine;

    private float masterVolume = 1f;
    private float bgmVolume = 1f;
    private float sfxVolume = 1f;

    private bool isMasterMuted = false;
    private bool isBGMMuted = false;
    private bool isSFXMuted = false;

    private const float MinVolume = 0.0001f;
    private const float MuteDb = -80f;
    private const float OffThreshold = 0.001f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            clipData = GetComponent<AudioClipData>();
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        switch (scene.name)
        {
            case "1_Logo":        PlayLogoSFX();        break;
            case "2_Title":       PlayTitleBGM();       break;
            case "3_StageSelect": PlaySelectBGM();      break;
            case "HappyEnding":   PlayHappyEndingBGM(); break;
            case "BadEnding":     PlayBadEndingBGM();   break;
        }
    }

    private void Start()
    {
        ApplyAllVolumes();
    }

    // --- BGM ---

    public void PlayLogoSFX()        => PlaySFX(clipData.logo);
    public void PlayTitleBGM()       => PlayBGM(clipData.title);
    public void PlaySelectBGM()      => PlayBGM(clipData.select);
    public void PlayHappyEndingBGM() => PlayBGM(clipData.happyEnding);
    public void PlayBadEndingBGM()   => PlayBGM(clipData.badEnding);

    public void PlayStageBGM(int stageIndex)
    {
        AudioClip clip = stageIndex switch
        {
            0 => clipData.stage1,
            1 => clipData.stage2,
            2 => clipData.stage3,
            _ => null
        };
        PlayBGM(clip);
    }

    // --- SFX ---

    public void PlayButtonSFX()      => PlaySFX(clipData.button);
    public void PlayStageSelectSFX() => PlaySFX(clipData.stageSelect);
    public void PlayJumpSFX()        => PlaySFX(clipData.jump);
    public void PlayFallSFX()        => PlaySFX(clipData.fall);
    public void PlayRespawnSFX()     => PlaySFX(clipData.respawn);
    public void PlayLeverSFX()       => PlaySFX(clipData.lever);
    public void PlayCheckPointSFX()  => PlaySFX(clipData.checkPoint);
    public void PlayRescueSFX()      => PlaySFX(clipData.rescue);
    public void PlayDoorSFX()        => PlaySFX(clipData.door);
    public void PlayCountdownSFX()   => PlaySFX(clipData.countdown);
    public void PlayFailSFX()        => PlaySFX(clipData.fail);

    // --- Core ---

    private void PlayBGM(AudioClip clip)
    {
        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);

        fadeCoroutine = StartCoroutine(FadeBGM(clip));
    }

    private void PlaySFX(AudioClip clip)
    {
        if (clip == null || IsMasterOff() || IsSFXOff()) return;

        sfxSource.PlayOneShot(clip);
    }

    private IEnumerator FadeBGM(AudioClip newClip)
    {
        if (bgmSource.isPlaying)
        {
            float startVolume = bgmSource.volume;
            float t = 0f;

            while (t < fadeDuration)
            {
                t += Time.unscaledDeltaTime;
                bgmSource.volume = Mathf.Lerp(startVolume, 0f, t / fadeDuration);
                yield return null;
            }

            bgmSource.Stop();
        }

        bgmSource.clip = newClip;
        bgmSource.volume = 0f;

        if (newClip == null)
        {
            fadeCoroutine = null;
            yield break;
        }

        bgmSource.Play();

        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            bgmSource.volume = Mathf.Lerp(0f, 1f, elapsed / fadeDuration);
            yield return null;
        }

        bgmSource.volume = 1f;
        fadeCoroutine = null;
    }

    // --- Volume / Mute ---

    public void SetMasterVolume(float value)
    {
        masterVolume = Mathf.Clamp01(value);
        ApplyAllVolumes();
    }

    public void SetBGMVolume(float value)
    {
        bgmVolume = Mathf.Clamp01(value);
        ApplyAllVolumes();
    }

    public void SetSFXVolume(float value)
    {
        sfxVolume = Mathf.Clamp01(value);
        ApplyAllVolumes();
    }

    public void SetMasterMute(bool isMute)
    {
        isMasterMuted = isMute;
        ApplyAllVolumes();
    }

    public void SetBGMMute(bool isMute)
    {
        isBGMMuted = isMute;
        ApplyAllVolumes();
    }

    public void SetSFXMute(bool isMute)
    {
        isSFXMuted = isMute;
        ApplyAllVolumes();
    }

    private void ApplyAllVolumes()
    {
        if (IsMasterOff())
            audioMixer.SetFloat("MasterVolume", MuteDb);
        else
            audioMixer.SetFloat("MasterVolume", Mathf.Log10(Mathf.Max(masterVolume, MinVolume)) * 20f);

        if (IsBGMOff())
            audioMixer.SetFloat("BGMVolume", MuteDb);
        else
            audioMixer.SetFloat("BGMVolume", Mathf.Log10(Mathf.Max(bgmVolume, MinVolume)) * 20f);

        if (IsSFXOff())
            audioMixer.SetFloat("SFXVolume", MuteDb);
        else
            audioMixer.SetFloat("SFXVolume", Mathf.Log10(Mathf.Max(sfxVolume, MinVolume)) * 20f);
    }

    private bool IsMasterOff() => isMasterMuted || masterVolume <= OffThreshold;
    private bool IsBGMOff()    => isBGMMuted    || bgmVolume    <= OffThreshold;
    private bool IsSFXOff()    => isSFXMuted    || sfxVolume    <= OffThreshold;
}
