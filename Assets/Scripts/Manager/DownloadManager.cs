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

    [Tooltip("FlyingEye, Goblin, EvilWizard, Skeleton")]
    [SerializeField] private MonsterData[] monsterDB;

    [SerializeField] private MonsterSpawnRatio MonsterRatio;

    [Tooltip("ShadowVeilSO, FerociousHowlSO, SpatialWarpSO, ChronoTwistSO, FerociousHowlSO")]
    [SerializeField] private SkillEffect[] SkillEffects;

    [Tooltip("Miya, Bambi, Leo, Cosmo, Chrono, Misty")]
    [SerializeField] private PlayerData[] PlayerDB;
    public PlayerData[] playerDatas { get { return PlayerDB; } }

    #region Singleton
    public static DownloadManager Instance;
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

    private void Start()
    {
        StartCoroutine(DownloadItemEffectSO()); // ItemDBSO까지 다운
        StartCoroutine(DownloadSkillEffects()); // PlayerDB까지 다운
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

    const string MonsterDataURL = "https://docs.google.com/spreadsheets/d/1N7_WPB-efwyN61w5LAuNaK6scp1m3PSrvF06er_NaWk/export?format=tsv&gid=1598314971&range=A2:M";

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
            monsterDB[i].attackRange = float.Parse(column[3]);
            monsterDB[i].speed = int.Parse(column[4]);
            monsterDB[i].damage = int.Parse(column[5]);
            monsterDB[i].idleTime = float.Parse(column[6]);
            monsterDB[i].patrolTime = float.Parse(column[7]);
            monsterDB[i].nearTraceTime = float.Parse(column[8]);
            monsterDB[i].restTime = float.Parse(column[9]);
            monsterDB[i].FTSCoef = float.Parse(column[10]);
            monsterDB[i].NTSCoef = float.Parse(column[11]);
            monsterDB[i].projectileSpeed = float.Parse(column[12]);
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

            MonsterRatio.monsterRatio.Add(int.Parse(column[0]));
            MonsterRatio.monsterRatio.Add(int.Parse(column[1]));
            MonsterRatio.monsterRatio.Add(int.Parse(column[2]));
            MonsterRatio.monsterRatio.Add(int.Parse(column[3]));
        }
    }
    #endregion

    #region SkillEffects

    const string SkillEffectURL = "https://docs.google.com/spreadsheets/d/1N7_WPB-efwyN61w5LAuNaK6scp1m3PSrvF06er_NaWk/export?format=tsv&gid=1112831032&range=A2:E";

    IEnumerator DownloadSkillEffects()
    {
        UnityWebRequest www = UnityWebRequest.Get(SkillEffectURL);
        yield return www.SendWebRequest();
        SetSkillEffects(www.downloadHandler.text);

        // Skill 세팅이 완료되면 플레이어 데이터 세팅
        yield return StartCoroutine(DownloadPlayerDB());
    }

    private void SetSkillEffects(string tsv)
    {
        string[] row = tsv.Split('\n');
        int rowSize = row.Length;
        int columnSize = row[0].Split('\t').Length;

        for (int i = 0; i < rowSize; i++)
        {
            string[] column = row[i].Split("\t");
            SkillEffects[i].skillType = Enum.Parse<SkillType>(column[0]);
            SkillEffects[i].coolTime = float.Parse(column[1]);
            SkillEffects[i].skillRange = float.Parse(column[2]);
            SkillEffects[i].effectLastTime = float.Parse(column[3]);
            SkillEffects[i].lastExecutionTime = float.Parse(column[4]);
        }
    }
    #endregion

    #region PlayerData

    const string PlayerDataURL = "https://docs.google.com/spreadsheets/d/1N7_WPB-efwyN61w5LAuNaK6scp1m3PSrvF06er_NaWk/export?format=tsv&gid=1361663794&range=A2:I";

    IEnumerator DownloadPlayerDB()
    {
        UnityWebRequest www = UnityWebRequest.Get(PlayerDataURL);
        yield return www.SendWebRequest();
        SetPlayerDB(www.downloadHandler.text);
    }

    private void SetPlayerDB(string tsv)
    {
        string[] row = tsv.Split('\n');
        int rowSize = row.Length;
        int columnSize = row[0].Split('\t').Length;

        for (int i = 0; i < rowSize; i++)
        {
            string[] column = row[i].Split("\t");
            PlayerDB[i].characterType = Enum.Parse<CharacterType>(column[0]);
            PlayerDB[i].maxHP = int.Parse(column[1]);
            PlayerDB[i].maxStamina = int.Parse(column[2]);
            PlayerDB[i].staminaRegenSpeed = float.Parse(column[3]);
            PlayerDB[i].speed = int.Parse(column[4]);
            PlayerDB[i].dodgeSpeed = int.Parse(column[5]);

            int index = (int)Enum.Parse<SkillType>(column[6]);
            if (index != -1) // -1인 경우 None임.
            {
                PlayerDB[i].skill = SkillEffects[index];
            }
            PlayerDB[i].giftType = Enum.Parse<ItemType>(column[7]);
            PlayerDB[i].hireCost = int.Parse(column[8]);
        }
    }
    #endregion
}
