using System;
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

public class ItemDB : MonoBehaviour
{
    public static ItemDB Instance;
    private void Awake()
    {
        Instance = this;
    }

    [SerializeField] private GameObject FieldItemPrefab;
    [SerializeField] private int ItemCount;
    public ItemDBSO itemDBSO;
    public int GetTotalItemCount(){ return ItemCount; }
    public void SetTotalItemCount(int _ItemCount){ ItemCount = _ItemCount; }

    public void GenerateItemsOnField()
    { 
        // 아이템 생성
        for (int i = 0; i < ItemCount; i++)
        {
            // 랜덤한 벽 근처 위치에 아이템 생성
            GameObject NewFieldItem = Instantiate(FieldItemPrefab, GameManager.Instance.GetMapGenerator().RandomPos(true, false), Quaternion.identity);
            // 랜덤한 아이템으로 세팅
            NewFieldItem.GetComponent<FieldItem>().SetItem((ItemType)UnityEngine.Random.Range(0, (int)ItemType.Max));
        }
        // ItemCountText 세팅
        GameManager.Instance.SetItemCount(0, ItemCount);
    }
}
