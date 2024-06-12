using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Threading.Tasks;

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

    private string filePath;
    private string keyWord = "341#@sdf^&gr$w&bk`9";
    private AccountData accountData;
    private readonly object fileLock = new object(); // 파일 쓰기 동기화를 위한 객체

    public int AccountGold
    {
        get { return accountData.gold; }
        set
        {
            accountData.gold = value;
            SaveJsonAsync();
        }
    }
    public ItemType[] GetAccountInven() { return accountData.Items; }

    private void Start()
    {
        filePath = Path.Combine(Application.persistentDataPath, "AccountData.json");
        LoadJson();
    }

    public void UpdateAccountItems(List<Item> _items)
    {
        List<ItemType> temp = new List<ItemType>();
        foreach (Item item in _items)
        {
            temp.Add(item.ItemType);
        }
        accountData.Items = temp.ToArray();
        SaveJsonAsync();
    }

    private async void SaveJsonAsync()
    {
        await Task.Run(() => SaveJson());
    }

    private void SaveJson()
    {
        lock (fileLock)
        {
            string jsonData = JsonUtility.ToJson(accountData);
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(jsonData);
            string code = System.Convert.ToBase64String(bytes);
            string encryptedData = EncryptAndDecript(code);
            File.WriteAllText(filePath, encryptedData);
        }
    }

    private void LoadJson()
    {
        if (File.Exists(filePath))
        {
            string code = File.ReadAllText(filePath);
            byte[] bytes = System.Convert.FromBase64String(EncryptAndDecript(code));
            string jsonData = System.Text.Encoding.UTF8.GetString(bytes);
            accountData = JsonUtility.FromJson<AccountData>(jsonData);
        }
        else
        {
            accountData = new AccountData();
        }
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
}
