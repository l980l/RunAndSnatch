using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private GameObject inventoryPanel;
    private bool ActiveInventory = false;

    [SerializeField] private Transform itemSlotHolder;
    private ItemSlot[] itemSlots;

    private void Start()
    {
        itemSlots = itemSlotHolder.GetComponentsInChildren<ItemSlot>();
        inventoryPanel.SetActive(ActiveInventory);
        Inventory.Instance.onChangeItem += RedrawSlotUI;
    }

    private void RedrawSlotUI()
    {
        for (int i = 0; i < itemSlots.Length; i++)
        {
            itemSlots[i].RemoveSlot();
        }
        for (int i = 0; i < Inventory.Instance.Items.Count; i++)
        {
            itemSlots[i].item = Inventory.Instance.Items[i];
            itemSlots[i].UpdateSlotUI();
        }
    }

    private void Update()
    {
        if(Input.GetButtonDown("Cancel"))
        {
            ActiveInventory = !ActiveInventory;
            inventoryPanel.SetActive(ActiveInventory);
        }
    }
}
