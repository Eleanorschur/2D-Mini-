using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class OptionUI : MonoBehaviour
{
    [Header("Mute Toggles")]
    [SerializeField] private Toggle masterMuteToggle;
    [SerializeField] private Toggle bgmMuteToggle;
    [SerializeField] private Toggle sfxMuteToggle;

    // audioManager를 위한 코드 추가
    [Header("Volume Sliders")]
    [SerializeField] private ImageSlider masterSlider;
    [SerializeField] private ImageSlider bgmSlider;
    [SerializeField] private ImageSlider sfxSlider;

    [Header("Display Toggles")]
    [SerializeField] private Toggle fullScreenToggle;
    [SerializeField] private Toggle windowScreenToggle;

    [Header("Resolution")]
    [SerializeField] private TMP_Dropdown resolutionDropdown;

    private bool isChangingToggle = false;

    private const string ScreenModeKey = "ScreenMode";
    private const string ResolutionIndexKey = "ResolutionIndex";

    private readonly int[,] resolutionOptions =
    {
        { 1920, 1080 },
        { 1280, 720 }
    };

    private void Awake()
    {
        // audioManager를 위한 코드 추가
        // Awake에서 등록해야 ImageSlider.Start()의 초기값 이벤트보다 먼저 리스너가 준비됨
        masterSlider.onValueChanged.AddListener(AudioManager.Instance.SetMasterVolume);
        bgmSlider.onValueChanged.AddListener(AudioManager.Instance.SetBGMVolume);
        sfxSlider.onValueChanged.AddListener(AudioManager.Instance.SetSFXVolume);

        masterMuteToggle.onValueChanged.AddListener(AudioManager.Instance.SetMasterMute);
        bgmMuteToggle.onValueChanged.AddListener(AudioManager.Instance.SetBGMMute);
        sfxMuteToggle.onValueChanged.AddListener(AudioManager.Instance.SetSFXMute);
    }

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
    }

    private void InitScreenMode()
    {
        PlayerPrefs.SetInt(ScreenModeKey, 1);
        PlayerPrefs.Save();

        Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
        Screen.fullScreen = true;
    }

    private void InitDisplayToggleState()
    {
        isChangingToggle = true;

        fullScreenToggle.isOn = true;
        windowScreenToggle.isOn = false;

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
        {
            savedResolutionIndex = 0;
        }

        resolutionDropdown.value = savedResolutionIndex;
        resolutionDropdown.RefreshShownValue();

        ApplyResolution(savedResolutionIndex);
    }

    private void OnFullScreenToggleChanged(bool isOn)
    {
        if (isChangingToggle) return;
        if (!isOn) return;

        Debug.Log("Full Screen 선택됨");

        SetFullScreen();
    }

    private void OnWindowScreenToggleChanged(bool isOn)
    {
        if (isChangingToggle) return;
        if (!isOn) return;

        Debug.Log("Window Screen 선택됨");

        SetWindowScreen();
    }

    private void SetFullScreen()
    {
        Debug.Log("전체화면 적용됨");

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
        Debug.Log("창모드 적용됨");

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

        Debug.Log("해상도 선택됨");
    }

    private void ApplyCurrentResolution()
    {
        int currentIndex = resolutionDropdown.value;
        ApplyResolution(currentIndex);
    }

    private void ApplyResolution(int index)
    {
        if (index < 0 || index >= resolutionOptions.GetLength(0))
        {
            index = 0;
        }

        int width = resolutionOptions[index, 0];
        int height = resolutionOptions[index, 1];

        bool isFullScreen = fullScreenToggle.isOn;

        Screen.SetResolution(width, height, isFullScreen);

        Debug.Log($"해상도 적용: {width} x {height}, FullScreen: {isFullScreen}");
    }

    private void InitVolumeSliderState()
    {
        masterSlider.SetValue(AudioManager.Instance.GetMasterVolume());
        bgmSlider.SetValue(AudioManager.Instance.GetBGMVolume());
        sfxSlider.SetValue(AudioManager.Instance.GetSFXVolume());
    }

    private void InitMuteToggleState()
    {
        masterMuteToggle.isOn = AudioManager.Instance.GetMasterMute();
        bgmMuteToggle.isOn    = AudioManager.Instance.GetBGMMute();
        sfxMuteToggle.isOn    = AudioManager.Instance.GetSFXMute();
    }
}