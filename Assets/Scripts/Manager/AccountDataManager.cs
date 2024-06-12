using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

[System.Serializable]
public class AccountData    // 캐릭터 해금 정보, 캐릭터 별 선물 정보도 추가해야 한다.
{
    public Item[] Items;
    public int gold;
}

public class AccountDataManager : MonoBehaviour
{
    // 싱글톤으로 지정.DontDestroyOnLoad로 지정.
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
    private AccountData accountData;
    public int AccountGold { get { return accountData.gold; } set { accountData.gold = value; SaveJson(); } }
    public Item[] GetAccountInven() { return accountData.Items; }

    private void Start()
    {
        filePath = Path.Combine(Application.persistentDataPath, "AccountData.json");
        LoadJson();
    }

    public void UpdateAccountItems(List<Item> _items)
    {
        accountData.Items = _items.ToArray();
        SaveJson();
    }

    private void SaveJson()
    {
        string jsonData = JsonUtility.ToJson(accountData);
        Debug.Log(jsonData);
        File.WriteAllText(filePath, jsonData);
    }

    private void LoadJson()
    {
        // 저장된 정보가 있는 경우에만 로딩. 
        if(File.Exists(filePath))
        {
            string jsonData = File.ReadAllText(filePath);
            accountData = JsonUtility.FromJson<AccountData>(jsonData);
        }

        // 없으면 초기 값을 세팅해줘야 함.
        else
        {
            accountData = new AccountData();
        }
    }
}
