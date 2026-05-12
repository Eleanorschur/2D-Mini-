using UnityEngine;
using UnityEngine.Audio;

public class TitleAudioManager : MonoBehaviour
{
    public static TitleAudioManager Instance;

    [Header("Audio Mixer")]
    [SerializeField] private AudioMixer audioMixer;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource bgmAudioSource;
    [SerializeField] private AudioSource sfxAudioSource;

    [Header("SFX Clips")]
    [SerializeField] private AudioClip buttonClickSFX;

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
        Instance = this;
    }

    private void Start()
    {
        ApplyAllVolumes();
    }

    public void PlayButtonClickSFX()
    {
        Debug.Log($"Button SFX Check / Master:{masterVolume}, SFX:{sfxVolume}, MasterMute:{isMasterMuted}, SFXMute:{isSFXMuted}");

        if (IsMasterOff() || IsSFXOff())
            return;

        if (sfxAudioSource == null || buttonClickSFX == null)
        {
            Debug.LogWarning("SFX AudioSource 또는 ButtonClickSFX가 연결되지 않았습니다.");
            return;
        }

        sfxAudioSource.PlayOneShot(buttonClickSFX);
    }

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
        Debug.Log("SFX 슬라이더 값 :" + value);

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
        ApplyMasterVolume();
        ApplyBGMVolume();
        ApplySFXVolume();
    }

    private void ApplyMasterVolume()
    {
        if (IsMasterOff())
            audioMixer.SetFloat("MasterVolume", MuteDb);
        else
            audioMixer.SetFloat("MasterVolume", Mathf.Log10(Mathf.Max(masterVolume, MinVolume)) * 20f);
    }

    private void ApplyBGMVolume()
    {
        if (IsBGMOFF())
            audioMixer.SetFloat("BGMVolume", MuteDb);
        else
            audioMixer.SetFloat("BGMVolume", Mathf.Log10(Mathf.Max(bgmVolume, MinVolume)) * 20f);
    }

    private void ApplySFXVolume()
    {
        if (IsSFXOff())
            audioMixer.SetFloat("SFXVolume", MuteDb);
        else
            audioMixer.SetFloat("SFXVolume", Mathf.Log10(Mathf.Max(sfxVolume, MinVolume)) * 20f);
    }

    private bool IsMasterOff()
    {
        return isMasterMuted || masterVolume <= OffThreshold;
    }

    private bool IsBGMOFF()
    {
        return isBGMMuted || bgmVolume <= OffThreshold;
    }

    private bool IsSFXOff()
    {
        return isSFXMuted || sfxVolume <= OffThreshold;
    }
}