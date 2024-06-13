using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Threading.Tasks;
using Unity.Services.CloudSave;
using System.Text;
using Unity.Services.CloudSave.Models;

[System.Serializable]
public class AccountData    // 캐릭터 해금 정보, 캐릭터 별 선물 정보도 추가해야 한다.
{
    public ItemType[] Items; 
    public int gold;
}

public class AccountDataManager : MonoBehaviour
{
    #region Singleton
    public static AccountDataManager Instance;
    private string keyWord = "341#@sdf^&gr$w&bk`9";
    private readonly object fileLock = new object(); // 파일 쓰기 동기화를 위한 객체
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    #endregion
    private AccountData accountData;

    public int AccountGold
    {
        get { return accountData.gold; }
        set
        {
            accountData.gold = value;
        }
    }
    public ItemType[] GetAccountInven() { return accountData.Items; }

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
