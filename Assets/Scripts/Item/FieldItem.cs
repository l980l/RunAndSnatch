using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldItem : MonoBehaviour
{
    private ItemType itemType;

    public void SetItem(ItemType _itemType)
    {
        itemType = _itemType;

        // 스프라이트 변경
        GetComponent<SpriteRenderer>().sprite = ItemDB.Instance.itemDBSO.items[(int)itemType].ItemImage;
    }
    
    // 아이템 획득시 사용할 함수
    public ItemType GetItemType()
    {
        return itemType;
    }

    public void DestoryItem()
    {
        Destroy(gameObject);
    }
}
