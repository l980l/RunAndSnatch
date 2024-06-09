using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldItem : MonoBehaviour
{
    public Item Item;

    public void SetItem(Item _item)
    {
        // 원본 아이템 값 복사
        Item.ItemType = _item.ItemType;
        Item.ItemName = _item.ItemType.ToString();  // 이름은 ItemType을 사용
        Item.ItemImage = _item.ItemImage;
        Item.Value = _item.Value;
        Item.Effects = _item.Effects;

        // 스프라이트 변경
        GetComponent<SpriteRenderer>().sprite = _item.ItemImage;
    }
    
    // 아이템 획득시 사용할 함수
    public Item GetItem()
    {
        return Item;
    }

    public void DestoryItem()
    {
        Destroy(gameObject);
    }
}
