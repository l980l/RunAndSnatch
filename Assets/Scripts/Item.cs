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
    // Type을 기반으로 자동 세팅할거라 숨김
    [HideInInspector] public string ItemName;   
    public Sprite ItemImage;
    public int Value;
    public List<ItemEffect> Effects;
    public string ItemTip;

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
