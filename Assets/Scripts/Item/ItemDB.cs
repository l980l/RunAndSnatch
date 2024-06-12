using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ItemDB : MonoBehaviour
{
    public static ItemDB Instance;
    private void Awake()
    {
        Instance = this;
    }

    [SerializeField] private GameObject FieldItemPrefab;
    [SerializeField] private int ItemCount;
    public int GetTotalItemCount(){ return ItemCount; }

    public void GenerateItemsOnField()
    { 
        // 아이템 생성
        for (int i = 0; i < ItemCount; i++)
        {
            // 랜덤한 벽 근처 위치에 아이템 생성
            GameObject NewFieldItem = Instantiate(FieldItemPrefab, GameManager.Instance.GetMapGenerator().RandomPos(true), Quaternion.identity);
            // 랜덤한 아이템으로 세팅
            NewFieldItem.GetComponent<FieldItem>().SetItem((ItemType)UnityEngine.Random.Range(0, (int)ItemType.Max));
        }
        // ItemValueText 세팅
        GameManager.Instance.SetItemValue(0, ItemCount);
    }

    #region ItemDBSO
    public ItemDBSO itemDBSO;

    const string URL = "https://docs.google.com/spreadsheets/d/1N7_WPB-efwyN61w5LAuNaK6scp1m3PSrvF06er_NaWk/export?format=tsv&gid=311763605&range=A2:G";
    private void Start()
    {
        StartCoroutine(DownloadItemDBSO());
    }

    IEnumerator DownloadItemDBSO()
    {
        UnityWebRequest www = UnityWebRequest.Get(URL);
        yield return www.SendWebRequest();
        SetItemDBSO(www.downloadHandler.text);
    }

    void SetItemDBSO(string tsv)
    {
        string[] row = tsv.Split('\n');
        int rowSize = row.Length;
        int columnSize = row[0].Split('\t').Length;

        for (int i = 0; i < rowSize; i++)
        {
            string[] column = row[i].Split("\t");
            itemDBSO.items[i].ItemType = Enum.Parse<ItemType>(column[0]);
            itemDBSO.items[i].ItemNameKr = column[1];
            itemDBSO.items[i].ItemNameEn = column[2];
            itemDBSO.items[i].ItemTipKr = column[3];
            itemDBSO.items[i].ItemTipEn = column[4];
            itemDBSO.items[i].Value = int.Parse(column[5]);

            // 이전 Effects는 지워줌. 안 그러면 매번 Add만 됨.
            itemDBSO.items[i].Effects.Clear();
            // Effect는 여러개라 _로 스플릿.
            string[] effects = column[6].Split("_");
            foreach(string effect in effects)
            {
                int index = (int)Enum.Parse<ItemEffectType>(effect);
                itemDBSO.items[i].Effects.Add(ItemEffectManager.Instance.ItemEffects[index]);
            }
        }
    }

    #endregion
}
