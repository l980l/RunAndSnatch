using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using Unity.Services.CloudSave;
using System;

[System.Serializable]
public class AccountData   
{
    public CharacterType selectedCharacter;
    public ItemType[] Items; 
    public int gold;
    public LanguageType language;
    public bool[] PlayableCharacter;
    public int[] CharacterGifts;
    public int exitStreak;
    public bool inDungeon;
    public float volumeBGM;
    public float volumeSFX;

    public AccountData() 
    {
        selectedCharacter = CharacterType.Miya;
        language = LanguageType.En;
        PlayableCharacter = new bool[(int)CharacterType.Max];
        PlayableCharacter[0] = true;
        CharacterGifts = new int[(int)CharacterType.Max];
        volumeBGM = 0.5f;
        volumeSFX = 0.5f;
    }
}

public class AccountDataManager : MonoBehaviour
{
    #region Singleton
    public static AccountDataManager Instance { get; private set; }
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
    //private AccountData accountData;
    private string keyWord = "1fh3ji9-re #@sdf^&gr$w&bk`9";
    private readonly object fileLock = new object(); // 파일 쓰기 동기화를 위한 객체

    public int AccountGold { get { return accountData.gold; } set { accountData.gold = value; } }
    public CharacterType SelectedCharacter { get { return accountData.selectedCharacter; } set { accountData.selectedCharacter = value; } }
    public ItemType[] GetAccountInven() { return accountData.Items; }
    public LanguageType LanguageType { get { return accountData.language; } set { accountData.language = value; } }
    public int ExitStreak { get { return accountData.exitStreak; } set { accountData.exitStreak = value; } }
    public bool InDungeon { get { return accountData.inDungeon; } set { accountData.inDungeon = value; } }
    public bool GetPlayable(CharacterType characterType)
    {
        return accountData.PlayableCharacter[(int)characterType];
    }
    public void SetPlayable(CharacterType characterType)
    {
        accountData.PlayableCharacter[(int)characterType] = true;
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

    public void DeathPenalty()
    {
        accountData.Items = new ItemType[0];
        accountData.gold = (int)(0.8f * accountData.gold);
        ExitStreak = 0;
        InDungeon = false;
        SaveJsonToCloud();
        // 아직 안 만들어졌으면 할 필요 없음. 어차피 Start에서 함.
        if(Inventory.Instance)
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
    private string EncryptAndDecript(string data)
    {
        string result = "";

        for (int i = 0; i < data.Length; ++i)
        {
            result += (char)(data[i] ^ keyWord[i % keyWord.Length]);
        }

        return result;
    }

    #region UnityCloud로 Save & Load
    private string DataKey = "PlayerData";

    public async void SaveJsonToCloud()
    {
        string jsonData;
        lock (fileLock)
        {
            jsonData = JsonUtility.ToJson(accountData);
        }
        // XOR 암호화만 해주자
        string encryptedData = EncryptAndDecript(jsonData);
        await SavePlayerData(DataKey, encryptedData);
    }

    private async void LoadJsonFromCloud()
    {
        string encryptedData = await LoadPlayerData(DataKey);
        if (encryptedData != null)
        {
            string jsonData = EncryptAndDecript(encryptedData);
            accountData = JsonUtility.FromJson<AccountData>(jsonData);
            if (accountData.inDungeon == true)
            {
                DeathPenalty();
            }
            Debug.Log(jsonData);
        }
        else
        {
            accountData = new AccountData();
        }
    }

    private async Task SavePlayerData(string key, string Data)
    {
        try
        {
            var data = new Dictionary<string, object> { { key, Data } };
            await CloudSaveService.Instance.Data.Player.SaveAsync(data);

            Debug.Log("File saved!");
        }
        catch (CloudSaveValidationException e)
        {
            Debug.LogError(e);
        }
        catch (CloudSaveRateLimitedException e)
        {
            Debug.LogError(e);
        }
        catch (CloudSaveException e)
        {
            Debug.LogError(e);
        }
    }

    private async Task<string> LoadPlayerData(string key)
    {
        try
        {
            var playerData = await CloudSaveService.Instance.Data.Player.LoadAsync(new HashSet<string> { key });
            if (playerData.TryGetValue(key, out var keyName))
            {
                Debug.Log($"keyName: {keyName.Value.GetAs<string>()}");
                return keyName.Value.GetAs<string>();
            }
            else
            {
                Debug.LogWarning($"Key '{key}' not found in player data.");
            }
        }
        catch (CloudSaveValidationException e)
        {
            Debug.LogError(e);
        }
        catch (CloudSaveRateLimitedException e)
        {
            Debug.LogError(e);
        }
        catch (CloudSaveException e)
        {
            Debug.LogError(e);
        }

        return null;
    }
    #endregion
}
