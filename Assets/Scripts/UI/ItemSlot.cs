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
    public void Clicked()
    { 
        if (item != null)
        {
            ToolTip toolTip = ToolTip.Instance;
            // 꺼져있던 경우 클릭되면
            if (!toolTip.gameObject.activeSelf)
            {
                SoundManager.Instance.PlaySFX(SFX.ButtonSFX, Camera.main.transform.position);

                toolTip.gameObject.SetActive(true);

                toolTip.ItemType = item.ItemType;
                
                if (GPGS_AccountDataManager.Instance.LanguageType == LanguageType.En)
                {
                    toolTip.ItemName.text = item.ItemNameEn;
                    toolTip.ItemTip.text = item.ItemTipEn;
                }
                else if (GPGS_AccountDataManager.Instance.LanguageType == LanguageType.Kr)
                {
                    toolTip.ItemName.text = item.ItemNameKr;
                    toolTip.ItemTip.text = item.ItemTipKr;
                }

                toolTip.ItemImage.sprite = item.ItemImage;
                toolTip.ClickedSlotIndex = slotNum;
            }
        }
    }
}
