using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Inventory;
using static UnityEditor.Progress;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private GameObject inventoryPanel;
    private bool ActiveInventory = false;

    [SerializeField] private Transform itemSlotHolder;
    private ItemSlot[] itemSlots;
    [SerializeField] private Text GoldText;

    Inventory inventory;

    private void Start()
    {
        itemSlots = itemSlotHolder.GetComponentsInChildren<ItemSlot>();
        inventoryPanel.SetActive(ActiveInventory);

        inventory = Inventory.Instance;
        inventory.onChangeItem += RedrawSlotUI;

        ToolTip.Instance.gameObject.SetActive(false);   // 초기 툴팁 비활성화

        GoldTextUpdate();
    }
    private void OnDestroy()
    {
        if (inventory != null)
        {
            inventory.onChangeItem -= RedrawSlotUI;
        }
    }
    private void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            ActiveInventory = !ActiveInventory;
            inventoryPanel.SetActive(ActiveInventory);
        }
    }

    private void RedrawSlotUI()
    {
        for (int i = 0; i < itemSlots.Length; i++)
        {
            itemSlots[i].RemoveSlot();
        }
        for (int i = 0; i < inventory.Items.Count; i++)
        {
            itemSlots[i].slotNum = i;
            itemSlots[i].item = inventory.Items[i];
            itemSlots[i].UpdateSlotUI();
        }
    }

    private void GoldTextUpdate()
    {
        // GoldText 업데이트
        GoldText.text = "Gold " + AccountDataManager.Instance.AccountGold.ToString();
    }

    public void SortButtonClicked()
    {
        inventory.SortItems();
        RedrawSlotUI();
    }
    public void AllSellButtonClicked()
    {
        inventory.AllSell();
        GoldTextUpdate();
    }
    public void SellButtonClicked()
    {
        inventory.Sell();
        GoldTextUpdate();
    }
}
