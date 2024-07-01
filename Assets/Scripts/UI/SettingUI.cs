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
    [SerializeField] private int CatTownSceneInt;
    [SerializeField] private GameObject CreditUI;
    [SerializeField] private GameObject EscapeUI;

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
        SoundManager.Instance.PlaySFX(SFX.ButtonSFX, Camera.main.transform.position);

        CreditUI.SetActive(false);
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
}
