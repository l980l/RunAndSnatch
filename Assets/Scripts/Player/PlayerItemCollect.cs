using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItemCollect : MonoBehaviour
{
    private Inventory inventory;
    public int AcquiredItemCount {  get; private set; } 

    private void Start()
    {
        inventory = Inventory.Instance;
        AcquiredItemCount = 0;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Item")
        {
            FieldItem fieldItem = collision.transform.GetComponent<FieldItem>();

            // ItemType으로 Item 가져오기.
            Item NowItem = ItemDB.Instance.itemDBSO.items[(int)fieldItem.GetItemType()];

            if (inventory.AddItem(NowItem))
            {
                fieldItem.DestoryItem();

                // 아이템 획득시 효과 사용
                NowItem.Use();

                // 획득 가치 적용
                AcquiredItemCount++;
                GameManager.Instance.SetItemCount(AcquiredItemCount, ItemDB.Instance.GetTotalItemCount());
                AccountDataManager.Instance.SaveJsonToCloud();
            }
        }
    }
}
