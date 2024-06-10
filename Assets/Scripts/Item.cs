using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ItemType
{
    BlueBall,
    MouseToy,
    FishToy,
    FoxTail,
    Salmon,
    PaperBox,
    Necklace,
    CatBell,
    Max
}

// 원본 아이템 클래스
[System.Serializable]
public class Item
{
    public ItemType ItemType;
    public string ItemNameKr;
    public string ItemNameEn;
    public Sprite ItemImage;
    public int Value;
    public List<ItemEffect> Effects;
    public string ItemTipKr;
    public string ItemTipEn;

    public bool Use()
    {
        bool isUsed  = false;
        foreach(ItemEffect effect in Effects)
        {
            isUsed = effect.ExecuteRole();
        }
        isUsed = true;

        return isUsed;
    }
}
