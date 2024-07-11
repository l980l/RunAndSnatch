using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeathUI : MonoBehaviour
{
    #region Singleton
    public static DeathUI Instance;
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    #endregion

    [SerializeField] private GameObject DeathPanel;
    [SerializeField] private GameObject YesButton;
    [SerializeField] private GameObject NoButton;
    [SerializeField] private Text PenaltyText;
    [SerializeField] private int CatTownSceneInt;
    private WaitForSecondsRealtime waitForSecondsRealtime;


    private void Start()
    {
        waitForSecondsRealtime = new WaitForSecondsRealtime(2f);
        DeathPanel.SetActive(false);
    }

    public void Death()
    {
        ShowDeathPanel();
    }

    private void ShowDeathPanel()
    {
        SoundManager.Instance.StopBGM();
        Time.timeScale = 0f;
        DeathPanel.SetActive(true);

        if (GPGS_AccountDataManager.Instance.LanguageType == LanguageType.En)
            PenaltyText.text = "If you watch the ad, you will not receive the death penalty.\r\nWould you like to watch the ad?";
        else
            PenaltyText.text = "광고를 시청하면 사망 패널티를 받지 않습니다.\r\n광고를 시청하시겠습니까?";

        AdMobManager.Instance.onRewardedAdFinished += RewardFunc;
    }

    private void RewardFunc()
    {
        StartCoroutine(GoToCatTownCoroutine());
    }

    public void RewardAdButtonClick()
    {
        YesButton.SetActive(false);
        NoButton.SetActive(false);

        if (GPGS_AccountDataManager.Instance.LanguageType == LanguageType.En)
            PenaltyText.text = "You will return to the town shortly.";
        else
            PenaltyText.text = "곧 마을로 돌아가게 될 것입니다.";

        GPGS_AccountDataManager.Instance.InDungeon = false;
        GPGS_AccountDataManager.Instance.SaveJsonToCloud();
        AdMobManager.Instance.RewardButtonClick();
    }

    public void RefuseButtonClick()
    {
        YesButton.SetActive(false);
        NoButton.SetActive(false);

        if (GPGS_AccountDataManager.Instance.LanguageType == LanguageType.En)
            PenaltyText.text = "You have lost all items and 20% of gold.\r\nYou will return to the town shortly.";
        else
            PenaltyText.text = "모든 아이템을 잃었고 골드의 20%를 잃었습니다.\r\n곧 마을로 돌아가게 될 것입니다.";
        GPGS_AccountDataManager.Instance.DeathPenalty();
        StartCoroutine(GoToCatTownCoroutine());
    }

    private IEnumerator GoToCatTownCoroutine()
    {
        yield return waitForSecondsRealtime;
        Time.timeScale = 1f;
        LoadingSceneController.LoadScene(CatTownSceneInt);
    }
}
