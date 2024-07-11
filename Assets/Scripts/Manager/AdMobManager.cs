using GoogleMobileAds.Api;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AdMobManager : MonoBehaviour
{
    #region Singleton
    public static AdMobManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    #endregion

    [SerializeField] private bool testMode;
    public Action onRewardedAdFinished = null;

    void Start()
    {
        var requestConfiguration = new RequestConfiguration
           .Builder()
           .SetTestDeviceIds(new List<string>() { "b7ffcbadb3204c26" }) // test Device ID
           .build();

        MobileAds.SetRequestConfiguration(requestConfiguration);

        LoadBannerAd();
        LoadFrontAd();
        LoadRewardAd();
    }

    private AdRequest GetAdRequest()
    {
        return new AdRequest.Builder().Build();
    }

    #region Banner Ad
    const string bannerTestID = "ca-app-pub-3940256099942544/6300978111";
    const string bannerID = "ca-app-pub-3030555614869163/3261602205";
    BannerView bannerAd;

    private void LoadBannerAd()
    {
        bannerAd = new BannerView(testMode ? bannerTestID : bannerID,
            AdSize.SmartBanner, AdPosition.Top);
        bannerAd.LoadAd(GetAdRequest());
        ToggleBannerAd(false);
    }

    public void ToggleBannerAd(bool b)
    {
        if (b) bannerAd.Show();
        else bannerAd.Hide();
    }
    #endregion

    #region Front Ad
    const string frontTestID = "ca-app-pub-3940256099942544/8691691433";
    const string frontID = "ca-app-pub-3030555614869163/5911499047";
    InterstitialAd frontAd;

    void LoadFrontAd()
    {
        frontAd = new InterstitialAd(testMode ? frontTestID : frontID);
        frontAd.LoadAd(GetAdRequest());
    }

    public void ShowFrontAd()
    {
        frontAd.Show();
        LoadFrontAd();
    }
    #endregion

    #region 리워드 광고
    const string rewardTestID = "ca-app-pub-3940256099942544/5224354917";
    const string rewardID = "ca-app-pub-3030555614869163/2195932541";
    RewardedAd rewardAd;

    private void LoadRewardAd()
    {
        rewardAd = new RewardedAd(testMode ? rewardTestID : rewardID);
        rewardAd.LoadAd(GetAdRequest());
        // 리워드 광고 시청 완료
        rewardAd.OnUserEarnedReward += (sender, e) =>
        {
            onRewardedAdFinished?.Invoke();
        };
    }

    public void RewardButtonClick()
    {
        rewardAd.Show();
        LoadRewardAd();
    }
    #endregion
}
