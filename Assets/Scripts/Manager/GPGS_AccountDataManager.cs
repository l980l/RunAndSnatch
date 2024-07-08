using GooglePlayGames;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.SavedGame;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class GPGS_AccountDataManager : MonoBehaviour
{
    #region Singleton
    public static GPGS_AccountDataManager Instance { get; private set; }
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
    private AccountData accountData;
    public int AccountGold { get { return accountData.gold; } set { accountData.gold = value; GPGS_LeaderBoardManager.Instance.UpdateGoldLeaderboard(); } }
    public CharacterType SelectedCharacter { get { return accountData.selectedCharacter; } set { accountData.selectedCharacter = value; } }
    public ItemType[] GetAccountInven() { return accountData.Items; }
    public LanguageType LanguageType { get { return accountData.language; } set { accountData.language = value; } }
    public int ExitStreak
    {
        get { return accountData.exitStreak; }
        set
        {
            accountData.exitStreak = value;
            GPGS_LeaderBoardManager.Instance.UpdateExitStreakLeaderboard();
            switch(accountData.exitStreak)
            {
                case 1:
                    GPGS_AchieveManager.Instance.UnlockEscapeBegginerAchievement();
                    break;
                case 3:
                    GPGS_AchieveManager.Instance.UnlockEscapeIntermediateAchievement();
                    break;
                case 5:
                    GPGS_AchieveManager.Instance.UnlockEscapeExperiencedAchievement();
                    break;
                case 7:
                    GPGS_AchieveManager.Instance.UnlockEscapeProficientAchievement();
                    break;
                case 10:
                    GPGS_AchieveManager.Instance.UnlockEscapeMasterAchievement();
                    break;
                case 13: 
                    GPGS_AchieveManager.Instance.UnlockEscapeGodAchievement();
                    break;
            }
        }
    }
    public int TotalExitCount { get { return accountData.totalExitCount; } set { accountData.totalExitCount = value; GPGS_LeaderBoardManager.Instance.UpdateDungeonExitLeaderboard(); } }

    public bool InDungeon { get { return accountData.inDungeon; } set { accountData.inDungeon = value; } }
    public bool GetPlayable(CharacterType characterType)
    {
        return accountData.PlayableCharacter[(int)characterType];
    }
    public void SetPlayable(CharacterType characterType)
    {
        accountData.PlayableCharacter[(int)characterType] = true;
        switch(characterType)
        {
            case CharacterType.Bambi:
                GPGS_AchieveManager.Instance.UnlockBambiAchievement();
                break;
            case CharacterType.Leo:
                GPGS_AchieveManager.Instance.UnlockLeoAchievement();
                break;
            case CharacterType.Cosmo:
                GPGS_AchieveManager.Instance.UnlockCosmoAchievement();
                break;
            case CharacterType.Chrono:
                GPGS_AchieveManager.Instance.UnlockChronoAchievement();
                break;
            case CharacterType.Misty:
                GPGS_AchieveManager.Instance.UnlockMistyAchievement();
                break;
        }
    }
    public int GetGiftCount(CharacterType characterType)
    {
        return accountData.CharacterGifts[(int)characterType];
    }
    public void AddGiftCount(CharacterType characterType)
    {
        accountData.CharacterGifts[(int)characterType] += 1;
    }
    public float VolumeBGM { get { return accountData.volumeBGM; } set { accountData.volumeBGM = value; } }
    public float VolumeSFX { get { return accountData.volumeSFX; } set { accountData.volumeSFX = value; } }
    public int FrameRateAnd { get { return accountData.frameRateAnd; } set { accountData.frameRateAnd = value; } }
    public int WinResolutionIndex { get { return accountData.winResolutionIndex; } set { accountData.winResolutionIndex = value; } }
    public bool IsFullscreen { get { return accountData.isFullscreen; } set { accountData.isFullscreen = value; } }

    public void DeathPenalty()
    {
        accountData.Items = new ItemType[0];
        AccountGold = Mathf.RoundToInt(AccountGold * 0.8f);
        ExitStreak = 0;
        InDungeon = false;
        SaveJsonToCloud();
        // 아직 안 만들어졌으면 할 필요 없음. 어차피 Start에서 함.
        if (Inventory.Instance)
            Inventory.Instance.LoadInven();
    }

    private void Start()
    {
        LoadJsonFromCloud();
    }

    public void UpdateAccountItems(List<Item> _items)
    {
        List<ItemType> temp = new List<ItemType>();
        foreach (Item item in _items)
        {
            temp.Add(item.ItemType);
        }
        accountData.Items = temp.ToArray();
    }

    #region Save
    private string fileName = "file.dat";

    public void SaveJsonToCloud()
    {
        OpenSaveGame();
    }

    private void OpenSaveGame()
    {
        ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;

        savedGameClient.OpenWithAutomaticConflictResolution(fileName, DataSource.ReadNetworkOnly, ConflictResolutionStrategy.UseLastKnownGood, OnSavedGameOpened);
    }

    private void OnSavedGameOpened(SavedGameRequestStatus status, ISavedGameMetadata game)
    {
        ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;

        if(status == SavedGameRequestStatus.Success)
        {
            Debug.Log("File Saved");
            var update = new SavedGameMetadataUpdate.Builder().Build();

            var json = JsonUtility.ToJson(accountData);
            byte[] bytes = Encoding.UTF8.GetBytes(json);

            Debug.Log("Saved Data: " + bytes);

            savedGameClient.CommitUpdate(game, update, bytes, OnSavedGameWritten);
        }

        else
        {
            Debug.Log("File Save Failed");
        }
    }

    private void OnSavedGameWritten(SavedGameRequestStatus status, ISavedGameMetadata data)
    {
        if (status == SavedGameRequestStatus.Success)
        {
            Debug.Log("File Saved");
        }

        else
        {
            Debug.Log("File Save Failed");
        }
    }
    #endregion

    #region Load
    private void LoadJsonFromCloud()
    {
        OpenLoadGame();
    }

    private void OpenLoadGame()
    {
        ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;

        savedGameClient.OpenWithAutomaticConflictResolution(fileName, DataSource.ReadNetworkOnly, ConflictResolutionStrategy.UseLastKnownGood, LoadGameData);
    }

    private void LoadGameData(SavedGameRequestStatus status, ISavedGameMetadata data)
    {
        ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;

        if (status == SavedGameRequestStatus.Success)
        {
            Debug.Log("File Loaded");

            savedGameClient.ReadBinaryData(data, OnSavedGameDataRead);
        }

        else
        {
            Debug.Log("File Loading Failed");
        }
    }

    private void OnSavedGameDataRead(SavedGameRequestStatus status, byte[] loadedData)
    {
        string data = System.Text.Encoding.UTF8.GetString(loadedData);

        if(data == "")
        {
            Debug.Log("No Data. Initialize a new Data");
            accountData = new AccountData();
            SaveJsonToCloud();
        }
        else
        {
            accountData = JsonUtility.FromJson<AccountData>(data);
            if (accountData.inDungeon == true)
            {
                DeathPenalty();
            }
            Debug.Log("Load Data: " + data);
        }

        // 마을로 이동
        LoadingSceneController.LoadScene(2);
    }
    #endregion

    #region Delete Data
    public void DeleteData()
    {
        DeleteGameData();
    }

    private void DeleteGameData()
    {
        ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;

        savedGameClient.OpenWithAutomaticConflictResolution(fileName, DataSource.ReadNetworkOnly,ConflictResolutionStrategy.UseLastKnownGood, DeleteSaveGame);
    }

    private void DeleteSaveGame(SavedGameRequestStatus status, ISavedGameMetadata data)
    {
        ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;

        if (status == SavedGameRequestStatus.Success)
        {
            savedGameClient.Delete(data);

            Debug.Log("File Deleted!");
        }

        else 
        {
            Debug.Log("File Delete Failed");
        }

        // 계정 정보를 제거했다면, 게임을 종료.
        Application.Quit();
    }
    #endregion
}
