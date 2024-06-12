using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public enum ItemEffectType
{
    SmallHealingEft, 
    StaminaFullRecoverEft,
    StaminaRegenUpEft, 
    BigHealingEft, 
    SpeedUpEft,
    BrightenEft, 
    StealthEft, 
    BellEft,
    Max
}

public class ItemEffectManager : MonoBehaviour
{
    public static ItemEffectManager Instance;
    private void Awake()
    {
        Instance = this;
    }

    [Tooltip("SmallHealingEft, StaminaFullRecoverEft, StaminaRegenUpEft, BigHealingEft, SpeedUpEft, BrightenEft, StealthEft, BellEft")]
    public ItemEffect[] ItemEffects;
    private void Start()
    {
        StartCoroutine(DownloadItemEffectSO());
    }

    const string URL = "https://docs.google.com/spreadsheets/d/1N7_WPB-efwyN61w5LAuNaK6scp1m3PSrvF06er_NaWk/export?format=tsv&range=A2:C";

    IEnumerator DownloadItemEffectSO()
    {
        UnityWebRequest www = UnityWebRequest.Get(URL);
        yield return www.SendWebRequest();
        SetItemEffectsSO(www.downloadHandler.text);
    }

    void SetItemEffectsSO(string tsv)
    {
        string[] row = tsv.Split('\n');
        int rowSize = row.Length;
        int columnSize = row[0].Split('\t').Length;

        for(int i = 0; i < rowSize; i++)
        {
            string[] column = row[i].Split("\t");
            ItemEffects[i].effectName = column[0];
            ItemEffects[i].value1 = float.Parse(column[1]);
            ItemEffects[i].value2 = float.Parse(column[2]);
        }
    }
}
