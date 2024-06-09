using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    public int slotNum;
    public Item item;
    public Image itemIcon;
    public void UpdateSlotUI()
    {
        itemIcon.sprite = item.ItemImage;
        itemIcon.color = Color.white;
        itemIcon.gameObject.SetActive(true);
    }
    public void RemoveSlot()
    {
        item = null;
        itemIcon.color = new Color(148, 95, 63, 255);   
        itemIcon.gameObject.SetActive(false);
    }
}
