using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class OptionUI : MonoBehaviour
{
    [Header("Volume Sliders")]
    [SerializeField] private ImageSlider masterVolumeSlider;
    [SerializeField] private ImageSlider bgmVolumeSlider;
    [SerializeField] private ImageSlider sfxVolumeSlider;

    [Header("Mute Toggles")]
    [SerializeField] private Toggle masterMuteToggle;
    [SerializeField] private Toggle bgmMuteToggle;
    [SerializeField] private Toggle sfxMuteToggle;

    [Header("Display Toggles")]
    [SerializeField] private Toggle fullScreenToggle;
    [SerializeField] private Toggle windowScreenToggle;

    [Header("Resolution")]
    [SerializeField] private TMP_Dropdown resolutionDropdown;

    private bool isChangingToggle = false;

    private const string ScreenModeKey = "ScreenMode";
    private const string ResolutionIndexKey = "ResolutionIndex";

    private const string MasterVolumeKey = "MasterVolume";
    private const string BGMVolumeKey = "BGMVolume";
    private const string SFXVolumeKey = "SFXVolume";

    private const string MasterMuteKey = "MasterMute";
    private const string BGMMuteKey = "BGMMute";
    private const string SFXMuteKey = "SFXMute";

    private readonly int[,] resolutionOptions =
    {
        { 1920, 1080 },
        { 1280, 720 }
    };

    private void Start()
    {
        InitScreenMode();
        InitDisplayToggleState();
        InitResolutionDropDown();

        InitVolumeSliderState();
        InitMuteToggleState();

        fullScreenToggle.onValueChanged.AddListener(OnFullScreenToggleChanged);
        windowScreenToggle.onValueChanged.AddListener(OnWindowScreenToggleChanged);
        resolutionDropdown.onValueChanged.AddListener(OnResolutionChanged);

        masterVolumeSlider.onValueChanged.AddListener(v =>
        {
            AudioManager.Instance?.SetMasterVolume(v);
            PlayerPrefs.SetFloat(MasterVolumeKey, v);
            PlayerPrefs.Save();
        });

        bgmVolumeSlider.onValueChanged.AddListener(v =>
        {
            AudioManager.Instance?.SetBGMVolume(v);
            PlayerPrefs.SetFloat(BGMVolumeKey, v);
            PlayerPrefs.Save();
        });

        sfxVolumeSlider.onValueChanged.AddListener(v =>
        {
            AudioManager.Instance?.SetSFXVolume(v);
            PlayerPrefs.SetFloat(SFXVolumeKey, v);
            PlayerPrefs.Save();
        });

        masterMuteToggle.onValueChanged.AddListener(v =>
        {
            AudioManager.Instance?.SetMasterMute(v);
            PlayerPrefs.SetInt(MasterMuteKey, v ? 1 : 0);
            PlayerPrefs.Save();
        });

        bgmMuteToggle.onValueChanged.AddListener(v =>
        {
            AudioManager.Instance?.SetBGMMute(v);
            PlayerPrefs.SetInt(BGMMuteKey, v ? 1 : 0);
            PlayerPrefs.Save();
        });

        sfxMuteToggle.onValueChanged.AddListener(v =>
        {
            AudioManager.Instance?.SetSFXMute(v);
            PlayerPrefs.SetInt(SFXMuteKey, v ? 1 : 0);
            PlayerPrefs.Save();
        });
    }

    private void InitScreenMode()
    {
        int savedScreenMode = PlayerPrefs.GetInt(ScreenModeKey, 1);

        if (savedScreenMode == 1)
        {
            Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
            Screen.fullScreen = true;
        }
        else
        {
            Screen.fullScreenMode = FullScreenMode.Windowed;
            Screen.fullScreen = false;
        }
    }

    private void InitDisplayToggleState()
    {
        int savedScreenMode = PlayerPrefs.GetInt(ScreenModeKey, 1);

        isChangingToggle = true;

        fullScreenToggle.isOn = savedScreenMode == 1;
        windowScreenToggle.isOn = savedScreenMode == 0;

        isChangingToggle = false;
    }

    private void InitResolutionDropDown()
    {
        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>
        {
            "1920 x 1080 (Full HD)",
            "1280 x 720 (HD)"
        };

        resolutionDropdown.AddOptions(options);

        int savedResolutionIndex = PlayerPrefs.GetInt(ResolutionIndexKey, 0);

        if (savedResolutionIndex < 0 || savedResolutionIndex >= resolutionOptions.GetLength(0))
            savedResolutionIndex = 0;

        resolutionDropdown.value = savedResolutionIndex;
        resolutionDropdown.RefreshShownValue();

        ApplyResolution(savedResolutionIndex);
    }

    private void InitVolumeSliderState()
    {
        float master = PlayerPrefs.GetFloat(MasterVolumeKey, 1f);
        float bgm = PlayerPrefs.GetFloat(BGMVolumeKey, 1f);
        float sfx = PlayerPrefs.GetFloat(SFXVolumeKey, 1f);

        masterVolumeSlider.SetValue(master);
        bgmVolumeSlider.SetValue(bgm);
        sfxVolumeSlider.SetValue(sfx);

        AudioManager.Instance?.SetMasterVolume(master);
        AudioManager.Instance?.SetBGMVolume(bgm);
        AudioManager.Instance?.SetSFXVolume(sfx);
    }

    private void InitMuteToggleState()
    {
        bool masterMute = PlayerPrefs.GetInt(MasterMuteKey, 0) == 1;
        bool bgmMute = PlayerPrefs.GetInt(BGMMuteKey, 0) == 1;
        bool sfxMute = PlayerPrefs.GetInt(SFXMuteKey, 0) == 1;

        masterMuteToggle.isOn = masterMute;
        bgmMuteToggle.isOn = bgmMute;
        sfxMuteToggle.isOn = sfxMute;

        AudioManager.Instance?.SetMasterMute(masterMute);
        AudioManager.Instance?.SetBGMMute(bgmMute);
        AudioManager.Instance?.SetSFXMute(sfxMute);
    }

    private void OnFullScreenToggleChanged(bool isOn)
    {
        if (isChangingToggle) return;
        if (!isOn) return;

        SetFullScreen();
    }

    private void OnWindowScreenToggleChanged(bool isOn)
    {
        if (isChangingToggle) return;
        if (!isOn) return;

        SetWindowScreen();
    }

    private void SetFullScreen()
    {
        Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
        Screen.fullScreen = true;

        PlayerPrefs.SetInt(ScreenModeKey, 1);
        PlayerPrefs.Save();

        isChangingToggle = true;

        fullScreenToggle.isOn = true;
        windowScreenToggle.isOn = false;

        isChangingToggle = false;

        ApplyCurrentResolution();
    }

    private void SetWindowScreen()
    {
        Screen.fullScreenMode = FullScreenMode.Windowed;
        Screen.fullScreen = false;

        PlayerPrefs.SetInt(ScreenModeKey, 0);
        PlayerPrefs.Save();

        isChangingToggle = true;

        fullScreenToggle.isOn = false;
        windowScreenToggle.isOn = true;

        isChangingToggle = false;

        ApplyCurrentResolution();
    }

    private void OnResolutionChanged(int index)
    {
        ApplyResolution(index);

        PlayerPrefs.SetInt(ResolutionIndexKey, index);
        PlayerPrefs.Save();
    }

    private void ApplyCurrentResolution()
    {
        int currentIndex = resolutionDropdown.value;
        ApplyResolution(currentIndex);
    }

    private void ApplyResolution(int index)
    {
        if (index < 0 || index >= resolutionOptions.GetLength(0))
            index = 0;

        int width = resolutionOptions[index, 0];
        int height = resolutionOptions[index, 1];

        bool isFullScreen = fullScreenToggle.isOn;

        if (isFullScreen)
        {
            Screen.fullScreenMode = FullScreenMode.FullScreenWindow;

            Screen.SetResolution(
                Display.main.systemWidth,
                Display.main.systemHeight,
                FullScreenMode.FullScreenWindow
            );
        }
        else
        {
            Screen.fullScreenMode = FullScreenMode.Windowed;

            Screen.SetResolution(
                width,
                height,
                FullScreenMode.Windowed
            );
        }
    }
}