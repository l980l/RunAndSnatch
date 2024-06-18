using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public enum DifLevel
{
    ChickenHeart,
    Spooky,
    CreepyCrawler,
    Nightmare,
    Hellbound,
    Max
}

public class MonsterManager : MonoBehaviour
{
    public static MonsterManager Instance;
    private void Awake()
    {
        Instance = this;
    }

    [Tooltip("FlyingEye, Goblin, EvilWizard, Skeleton")]
    [SerializeField] private GameObject[] monsterPrefab;
    [SerializeField] private int monsterCount;
    [SerializeField] private DifLevel difficulty;
    [SerializeField] private MonsterSpawnRatio MonsterRatio;


    public void GenerateMonstersOnField()
    {
        for (int i = 0; i < monsterCount; i++)
        {
            int randomValue = UnityEngine.Random.Range(0, 100);
            int cumulativeRatio = 0;
            // 몬스터 종류별로 for문
            for (int j = 0; j < (int)MonsterType.Max; j++)
            {
                cumulativeRatio += MonsterRatio.monsterRatio[(int)difficulty][j];
                if (randomValue < cumulativeRatio)
                {
                    Instantiate(monsterPrefab[j], GameManager.Instance.GetMapGenerator().RandomPos(false), Quaternion.identity);
                    break;
                }
            }
        }
    }

}
