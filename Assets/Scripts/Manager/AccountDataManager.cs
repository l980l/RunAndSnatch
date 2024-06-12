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
            SaveJsonToCloud();
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
        SaveJsonToCloud();
    }


    #region UnityCloud 이용
    private string DataKey = "PlayerData";

    private async void SaveJsonToCloud()
    {
        string jsonData;
        lock (fileLock)
        {
            jsonData = JsonUtility.ToJson(accountData);
        }

        byte[] bytes = System.Text.Encoding.UTF8.GetBytes(jsonData);
        await SaveFileBytes(DataKey, bytes);
    }

    private async void LoadJsonFromCloud()
    {
        byte[] bytes = await LoadFileBytes(DataKey);
        if (bytes != null)
        {
            string jsonData = System.Text.Encoding.UTF8.GetString(bytes);
            accountData = JsonUtility.FromJson<AccountData>(jsonData);
            Debug.Log(jsonData);
        }
        else
        {
            accountData = new AccountData();
        }
    }

    private async Task SaveFileBytes(string key, byte[] bytes)
    {
        try
        {
            await CloudSaveService.Instance.Files.Player.SaveAsync(key, bytes);
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

    private async Task<byte[]> LoadFileBytes(string key)
    {
        try
        {
            var results = await CloudSaveService.Instance.Files.Player.LoadBytesAsync(key);
            Debug.Log("File loaded!");
            return results;
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
