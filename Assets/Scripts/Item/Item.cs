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
    [HideInInspector] public ItemType ItemType;
    [HideInInspector] public string ItemNameKr;
    [HideInInspector] public string ItemNameEn;
    [HideInInspector] public string ItemTipKr;
    [HideInInspector] public string ItemTipEn;
    [HideInInspector] public int Value;
    [HideInInspector] public List<ItemEffect> Effects;
    public Sprite ItemImage;

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
