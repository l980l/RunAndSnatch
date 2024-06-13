using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class DownloadManager : MonoBehaviour
{
    [Tooltip("SmallHealingEft, StaminaFullRecoverEft, StaminaRegenUpEft, BigHealingEft, SpeedUpEft, BrightenEft, StealthEft, BellEft")]
    [SerializeField] private ItemEffect[] ItemEffects;

    [SerializeField] private ItemDBSO itemDBSO;

    [Tooltip("FlyingEye, Goblin, MushRoom, Skeleton")]
    [SerializeField] private MonsterData[] monsterDB;

    [SerializeField] private MonsterSpawnRatio MonsterRatio;
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        StartCoroutine(DownloadItemEffectSO());
        StartCoroutine(DownloadMonsterDB());
        StartCoroutine(DownloadSpawnRatio());
    }

    #region ItemEffects

    const string ItemEffectsURL = "https://docs.google.com/spreadsheets/d/1N7_WPB-efwyN61w5LAuNaK6scp1m3PSrvF06er_NaWk/export?format=tsv&range=A2:C";

    IEnumerator DownloadItemEffectSO()
    {
        UnityWebRequest www = UnityWebRequest.Get(ItemEffectsURL);
        yield return www.SendWebRequest();
        SetItemEffectsSO(www.downloadHandler.text);

        // Effect 세팅이 완료되면 아이템 데이터 세팅
        yield return StartCoroutine(DownloadItemDBSO());
    }

    void SetItemEffectsSO(string tsv)
    {
        string[] row = tsv.Split('\n');
        int rowSize = row.Length;
        int columnSize = row[0].Split('\t').Length;

        for (int i = 0; i < rowSize; i++)
        {
            string[] column = row[i].Split("\t");
            ItemEffects[i].effectName = column[0];
            ItemEffects[i].value1 = float.Parse(column[1]);
            ItemEffects[i].value2 = float.Parse(column[2]);
        }
    }
    #endregion

    #region ItemDBSO

    const string ItemDBSOURL = "https://docs.google.com/spreadsheets/d/1N7_WPB-efwyN61w5LAuNaK6scp1m3PSrvF06er_NaWk/export?format=tsv&gid=311763605&range=A2:G";
   
    IEnumerator DownloadItemDBSO()
    {
        UnityWebRequest www = UnityWebRequest.Get(ItemDBSOURL);
        yield return www.SendWebRequest();
        SetItemDBSO(www.downloadHandler.text);
    }

    private void SetItemDBSO(string tsv)
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
            foreach (string effect in effects)
            {
                int index = (int)Enum.Parse<ItemEffectType>(effect);
                itemDBSO.items[i].Effects.Add(ItemEffects[index]);
            }
        }
    }

    #endregion

    #region MonsterData

    const string MonsterDataURL = "https://docs.google.com/spreadsheets/d/1N7_WPB-efwyN61w5LAuNaK6scp1m3PSrvF06er_NaWk/export?format=tsv&gid=1598314971&range=A2:F";

    IEnumerator DownloadMonsterDB()
    {
        UnityWebRequest www = UnityWebRequest.Get(MonsterDataURL);
        yield return www.SendWebRequest();
        SetMonsterDB(www.downloadHandler.text);
    }

    private void SetMonsterDB(string tsv)
    {
        string[] row = tsv.Split('\n');
        int rowSize = row.Length;
        int columnSize = row[0].Split('\t').Length;

        for (int i = 0; i < rowSize; i++)
        {
            string[] column = row[i].Split("\t");
            monsterDB[i].Type = Enum.Parse<MonsterType>(column[0]);
            monsterDB[i].nearTraceRange = int.Parse(column[1]);
            monsterDB[i].farTraceRange = int.Parse(column[2]);
            monsterDB[i].attackRange = int.Parse(column[3]);
            monsterDB[i].speed = int.Parse(column[4]);
            monsterDB[i].damage = int.Parse(column[5]);
        }
    }
    #endregion

    #region MonsterSpawnRatio

    const string SpawnRatioURL = "https://docs.google.com/spreadsheets/d/1N7_WPB-efwyN61w5LAuNaK6scp1m3PSrvF06er_NaWk/export?format=tsv&gid=1993697643&range=A2:D";

    // 난이도별 생성 비율을 스프레드 시트로부터 가져오는 함수
    IEnumerator DownloadSpawnRatio()
    {
        UnityWebRequest www = UnityWebRequest.Get(SpawnRatioURL);
        yield return www.SendWebRequest();
        SetSpawnRatio(www.downloadHandler.text);
    }

    private void SetSpawnRatio(string tsv)
    {
        string[] row = tsv.Split('\n');
        int rowSize = row.Length;
        int columnSize = row[0].Split('\t').Length;

        for (int i = 0; i < rowSize; i++)
        {
            string[] column = row[i].Split("\t");

            List<int> temp = new List<int>();
            temp.Add(int.Parse(column[0]));
            temp.Add(int.Parse(column[1]));
            temp.Add(int.Parse(column[2]));
            temp.Add(int.Parse(column[3]));

            MonsterRatio.monsterRatio.Add(temp);
        }
    }
    #endregion
}
