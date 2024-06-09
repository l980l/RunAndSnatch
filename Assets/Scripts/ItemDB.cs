using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDB : MonoBehaviour
{
    public static ItemDB Instance;
    private void Awake()
    {
        Instance = this;
    }

    [SerializeField] private List<Item> itemDB = new List<Item>();
    [SerializeField] private GameObject FieldObjectPrefab;

    [SerializeField] private int ItemCount;
    private int TotalItemValue;
    public int GetTotalItemValue(){ return TotalItemValue; }

    public void GenerateItemsOnField()
    {
        // 아이템 생성
        for (int i = 0; i < ItemCount; i++)
        {
            // 랜덤한 벽 근처 위치에 아이템 생성
            GameObject NewFieldItem = Instantiate(FieldObjectPrefab, GameManager.Instance.GetMapGenerator().RandomPos(true), Quaternion.identity);
            // 랜덤한 아이템으로 세팅
            NewFieldItem.GetComponent<FieldItem>().SetItem(itemDB[Random.Range(0, (int)ItemType.Max)]);
            // Item 가치 누적
            TotalItemValue += NewFieldItem.GetComponent<FieldItem>().Item.Value;
        }
        // ItemValueText 세팅
        GameManager.Instance.SetItemValue(0, TotalItemValue);
    }
}
