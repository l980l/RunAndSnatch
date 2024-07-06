using GooglePlayGames;
using GooglePlayGames.BasicApi;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GPGS_LeaderBoardManager : MonoBehaviour
{
    #region Singleton
    public static GPGS_LeaderBoardManager Instance { get; private set; }
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

    public void ShowGoldLeaderboardUI()
    {
        PlayGamesPlatform.Instance.ShowLeaderboardUI(GPGSIds.leaderboard_gold);
    }

    public void ShowExitStreakLeaderboardUI()
    {
        PlayGamesPlatform.Instance.ShowLeaderboardUI(GPGSIds.leaderboard_dungeonexitstreak);
    }

    public void ShowDungeonExitLeaderboardUI()
    {
        PlayGamesPlatform.Instance.ShowLeaderboardUI(GPGSIds.leaderboard_dungeonexit);
    }

    public void UpdateGoldLeaderboard()
    {
        PlayGamesPlatform.Instance.ReportScore(GPGS_AccountDataManager.Instance.AccountGold, GPGSIds.leaderboard_gold, (bool success) => { });
    }

    public void UpdateExitStreakLeaderboard()
    {
        PlayGamesPlatform.Instance.ReportScore(GPGS_AccountDataManager.Instance.ExitStreak, GPGSIds.leaderboard_dungeonexitstreak, (bool success) => { });
    }

    public void UpdateDungeonExitLeaderboard()
    {
        PlayGamesPlatform.Instance.ReportScore(GPGS_AccountDataManager.Instance.TotalExitCount, GPGSIds.leaderboard_dungeonexit, (bool success) => { });
    }
}
