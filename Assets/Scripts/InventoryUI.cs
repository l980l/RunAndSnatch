using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private GameObject inventoryPanel;
    private bool ActiveInventory = false;

    private void Start()
    {
        inventoryPanel.SetActive(ActiveInventory);
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
