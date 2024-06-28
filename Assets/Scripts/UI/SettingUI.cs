using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
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

    private void Awake()
    {
        SettingPanel.SetActive(ActiveSettingPanel);
        sliderBGM.value = AccountDataManager.Instance.VolumeBGM;
        sliderSFX.value = AccountDataManager.Instance.VolumeSFX;
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
}
