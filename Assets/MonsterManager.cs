using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MonsterManager : MonoBehaviour
{
    [Tooltip("FlyingEye, Goblin, MushRoom, Skeleton")]
    [SerializeField] private MonsterData[] monsterDB;
    [Tooltip("FlyingEye, Goblin, MushRoom, Skeleton")]
    [SerializeField] private GameObject[] monsterPrefab;
    [SerializeField] private int monsterCount;

    const string URL = "https://docs.google.com/spreadsheets/d/1N7_WPB-efwyN61w5LAuNaK6scp1m3PSrvF06er_NaWk/export?format=tsv&gid=1598314971&range=A2:F";

    private void Start()
    {
        StartCoroutine(DownloadMonsterDB());
    }

    IEnumerator DownloadMonsterDB()
    {
        UnityWebRequest www = UnityWebRequest.Get(URL);
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
}
