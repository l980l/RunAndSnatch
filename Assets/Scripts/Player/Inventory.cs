using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    // 싱글톤으로 지정.
    #region Singleton
    public static Inventory Instance;
    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    #endregion

    // 아이템이 추가되면 슬롯 UI에도 추가되게 만들자.
    public delegate void OnChangeItem();                // 델리게이트 정의
    public OnChangeItem onChangeItem;                   // 델리게이트 인스턴스화

    // 획득한 아이템을 모아두는 공간. 즉, 진짜 인벤토리
    [HideInInspector] public List<Item> Items = new List<Item>();
    [SerializeField] private int slotCount;
    private int AcquiredItemCount;

    private void Start()
    {
        LoadInven();
    }

    private void LoadInven()
    {
        if (AccountDataManager.Instance.GetAccountInven() != null)
        {
            ItemType[] temp = AccountDataManager.Instance.GetAccountInven();
            foreach(ItemType item in temp)
            {
                Items.Add(ItemDB.Instance.itemDBSO.items[(int)item]);
            }
        }
        onChangeItem.Invoke();
    }

    public bool AddItem(Item _item)
    {
        // 최대 Slot 수를 정하고, 그 이상은 인벤토리 용량 초과인 것.
        if (Items.Count < slotCount)
        {
            Items.Add(_item);
            if(onChangeItem != null)
                onChangeItem.Invoke();

            AccountDataManager.Instance.UpdateAccountItems(Items);
            return true;
        }
        return false;
    }

    public void RemoveItem(int index)
    {
        Items.RemoveAt(index);
        onChangeItem.Invoke();
        AccountDataManager.Instance.UpdateAccountItems(Items);
    }

    public void RemoveAllItem()
    {
        Items.Clear();
        onChangeItem.Invoke();
        AccountDataManager.Instance.UpdateAccountItems(Items);
    }

    public void SortItems()
    {
        Items.Sort((item1, item2) => item1.ItemType.CompareTo(item2.ItemType));
        AccountDataManager.Instance.UpdateAccountItems(Items);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Item")
        {
            FieldItem fieldItem = collision.transform.GetComponent<FieldItem>();

            // ItemType으로 Item 가져오기.
            Item NowItem = ItemDB.Instance.itemDBSO.items[(int)fieldItem.GetItemType()];

            if (AddItem(NowItem))
            {
                fieldItem.DestoryItem();

                // 아이템 획득시 효과 사용
                NowItem.Use();

                // 획득 가치 적용
                AcquiredItemCount++;
                GameManager.Instance.SetItemValue(AcquiredItemCount, ItemDB.Instance.GetTotalItemCount());

                // 계정 정보에 추가
                AccountDataManager.Instance.UpdateAccountItems(Items);
            }
        }
    }

    public void Sell()
    {
        AccountDataManager.Instance.AccountGold += Items[ToolTip.Instance.ClickedSlotIndex].Value;
        RemoveItem(ToolTip.Instance.ClickedSlotIndex);
    }

    public void AllSell()
    {
        int amount = 0;
        foreach (var item in Items)
        {
            amount += item.Value;
        }
        AccountDataManager.Instance.AccountGold += amount;
        RemoveAllItem();
    }
}
