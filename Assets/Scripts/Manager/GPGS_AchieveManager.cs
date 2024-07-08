using GooglePlayGames;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GPGS_AchieveManager : MonoBehaviour
{
    #region Singleton
    public static GPGS_AchieveManager Instance { get; private set; }
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

    public void ShowAchievementUI()
    {
        PlayGamesPlatform.Instance.ShowAchievementsUI();
    }

    public void UnlockBambiAchievement()
    {
        PlayGamesPlatform.Instance.UnlockAchievement(GPGSIds.achievement_bambi_we_are_friends, (bool success) => { });
    }

    public void UnlockLeoAchievement()
    {
        PlayGamesPlatform.Instance.UnlockAchievement(GPGSIds.achievement_leo_we_are_friends, (bool success) => { });
    }

    public void UnlockCosmoAchievement()
    {
        PlayGamesPlatform.Instance.UnlockAchievement(GPGSIds.achievement_cosmo_we_are_friends, (bool success) => { });
    }

    public void UnlockChronoAchievement()
    {
        PlayGamesPlatform.Instance.UnlockAchievement(GPGSIds.achievement_chrono_we_are_friends, (bool success) => { });
    }

    public void UnlockMistyAchievement()
    {
        PlayGamesPlatform.Instance.UnlockAchievement(GPGSIds.achievement_misty_we_are_friends, (bool success) => { });
    }

    public void UnlockEscapeBegginerAchievement()
    {
        PlayGamesPlatform.Instance.UnlockAchievement(GPGSIds.achievement_escape_begginer, (bool success) => { });
    }
    public void UnlockEscapeIntermediateAchievement()
    {
        PlayGamesPlatform.Instance.UnlockAchievement(GPGSIds.achievement_escape_intermediate, (bool success) => { });
    }
    public void UnlockEscapeExperiencedAchievement()
    {
        PlayGamesPlatform.Instance.UnlockAchievement(GPGSIds.achievement_escape_experienced, (bool success) => { });
    }
    public void UnlockEscapeProficientAchievement()
    {
        PlayGamesPlatform.Instance.UnlockAchievement(GPGSIds.achievement_escape_proficient, (bool success) => { });
    }
    public void UnlockEscapeMasterAchievement()
    {
        PlayGamesPlatform.Instance.UnlockAchievement(GPGSIds.achievement_escape_master, (bool success) => { });
    }
    public void UnlockEscapeGodAchievement()
    {
        PlayGamesPlatform.Instance.UnlockAchievement(GPGSIds.achievement_escape_god, (bool success) => { });
    }
}
