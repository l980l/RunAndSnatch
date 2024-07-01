using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class SettingUI : MonoBehaviour
{
    [SerializeField] private GameObject SettingPanel;
    private bool ActiveSettingPanel = false;

    [Header("#Sound")]
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private string BGMParamName;
    [SerializeField] private string SFXParamName;
    [SerializeField] private Slider sliderBGM;
    [SerializeField] private Slider sliderSFX;
    [SerializeField] private int CatTownSceneInt;
    [SerializeField] private GameObject CreditUI;
    [SerializeField] private GameObject EscapeUI;
    [SerializeField] private Dropdown fpsDropdown;
    [SerializeField] private GameObject QuitUI;

    // PC에서만 사용하는 변수들
    [SerializeField] private Dropdown resolutionDropdown;
    [SerializeField] private Toggle fullscreenToggle;

    private List<Resolution> resolutions;

    private void Awake()
    {
        SettingPanel.SetActive(ActiveSettingPanel);
        sliderBGM.value = AccountDataManager.Instance.VolumeBGM;
        sliderSFX.value = AccountDataManager.Instance.VolumeSFX;

#if UNITY_STANDALONE_WIN
        InitializeWindowsSettings();
#endif

#if UNITY_ANDROID
        InitializeAndroidSettings();
#endif
    }

    private void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            ShowSettingUI(!ActiveSettingPanel);
        }
    }

    public void ShowSettingUI(bool show)
    {
        // 끌때, 계정 정보 저장. 
        if (!show)
        {
            AccountDataManager.Instance.SaveJsonToCloud();
        }
        ActiveSettingPanel = show;
        SettingPanel.SetActive(ActiveSettingPanel);
        SoundManager.Instance.PlaySFX(SFX.ButtonSFX, Camera.main.transform.position);

        CreditUI.SetActive(false);
        QuitUI.SetActive(false);
        if (EscapeUI != null) 
            EscapeUI.SetActive(false);
    }

    public void SetBGMVolume(float volume)
    {
        audioMixer.SetFloat(BGMParamName, Mathf.Log10(volume) * 20);
        AccountDataManager.Instance.VolumeBGM = volume;
    }

    public void SetSFXVolume(float volume)
    {
        audioMixer.SetFloat(SFXParamName, Mathf.Log10(volume) * 20);
        AccountDataManager.Instance.VolumeSFX = volume;
    }

    public void KrButtonClick()
    {
        SoundManager.Instance.PlaySFX(SFX.ButtonSFX, Camera.main.transform.position);

        if (AccountDataManager.Instance.LanguageType != LanguageType.Kr)
        {
            AccountDataManager.Instance.LanguageType = LanguageType.Kr;
            AccountDataManager.Instance.SaveJsonToCloud();
        }
    }

    public void EnButtonClick()
    {
        SoundManager.Instance.PlaySFX(SFX.ButtonSFX, Camera.main.transform.position);

        if (AccountDataManager.Instance.LanguageType != LanguageType.En)
        {
            AccountDataManager.Instance.LanguageType = LanguageType.En;
            AccountDataManager.Instance.SaveJsonToCloud();
        }
    }
    public void CreditButtonClick()
    {
        CreditUI.SetActive(true);
        SoundManager.Instance.PlaySFX(SFX.ButtonSFX, Camera.main.transform.position);
    }
    public void BackButtonClick()
    {
        CreditUI.SetActive(false);
        SoundManager.Instance.PlaySFX(SFX.ButtonSFX, Camera.main.transform.position);
    }

    public void EscapeButtonClick()
    {
        EscapeUI.SetActive(true);
        SoundManager.Instance.PlaySFX(SFX.ButtonSFX, Camera.main.transform.position);
    }

    public void YesButtonClick()
    {
        AccountDataManager.Instance.DeathPenalty();
        LoadingSceneController.LoadScene(CatTownSceneInt);
    }

    public void NoButtonClick()
    {
        EscapeUI.SetActive(false);
        SoundManager.Instance.PlaySFX(SFX.ButtonSFX, Camera.main.transform.position);
    }

    public void QuitGameButtonClick()
    {
        QuitUI.SetActive(true);
        SoundManager.Instance.PlaySFX(SFX.ButtonSFX, Camera.main.transform.position);
    }

    public void QuitYesButtonClick()
    {
        Application.Quit();
    }

    public void QuitBackButtonClick()
    {
        QuitUI.SetActive(false);
        SoundManager.Instance.PlaySFX(SFX.ButtonSFX, Camera.main.transform.position);
    }

#if UNITY_STANDALONE_WIN
    private void InitializeWindowsSettings()
    {
        // 프레임 설정 없애기.
        fpsDropdown.transform.parent.gameObject.SetActive(false);

        InitializeResolutionOptions();

        resolutionDropdown.value = AccountDataManager.Instance.WinResolutionIndex;
        fullscreenToggle.isOn = AccountDataManager.Instance.IsFullscreen;
        ApplyResolutionAndFullscreen();
        resolutionDropdown.RefreshShownValue();
    }

    private void InitializeResolutionOptions()
    {
        resolutions = new List<Resolution>(Screen.resolutions);
        resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();
        foreach (Resolution resolution in resolutions)
        {
            options.Add(resolution.width + " x " + resolution.height + "  " + resolution.refreshRateRatio + "hz");
        }
        resolutionDropdown.AddOptions(options);
    }

    public void ResolutionDropdownClick(int index)
    {
        AccountDataManager.Instance.WinResolutionIndex = index;
        ApplyResolutionAndFullscreen();
    }

    public void FullscreenToggleClick(bool isFullscreen)
    {
        AccountDataManager.Instance.IsFullscreen = isFullscreen;
        ApplyResolutionAndFullscreen();
    }
    private void ApplyResolutionAndFullscreen()
    {
        int index = AccountDataManager.Instance.WinResolutionIndex;

        // -1이면 최대 크기로 지정. 
        if (index == -1)
            index = resolutions.Count - 1;
        // 혹시 모니터가 변경되면 인덱스가 넘어갈 수도 있으니 클램프.
        index = Mathf.Clamp(index, 0, resolutions.Count);
        AccountDataManager.Instance.WinResolutionIndex = index;

        Resolution resolution = resolutions[index];
        Screen.SetResolution(resolution.width, resolution.height, AccountDataManager.Instance.IsFullscreen);
    }
#endif

#if UNITY_ANDROID
    private void InitializeAndroidSettings()
    {
        // 해상도 설정 끄기 
        resolutionDropdown.transform.parent.gameObject.SetActive(false);

        // 프레임레이트 설정.
        fpsDropdown.value = AccountDataManager.Instance.FrameRateAnd;
        FPSDropdownClick(fpsDropdown.value);
    }
    
    public void FPSDropdownClick(Int32 _option)
    {
        switch (_option)
        {
            case 0:
                Application.targetFrameRate = 30;
                break;
            case 1:
                Application.targetFrameRate = 60;
                break;
            case 2:
                Application.targetFrameRate = 144;
                break;
            case 3:
                // 프레임 레이트 제한 없음
                Application.targetFrameRate = -1;
                break;
        }

        AccountDataManager.Instance.FrameRateAnd = _option;
    }
#endif
}
