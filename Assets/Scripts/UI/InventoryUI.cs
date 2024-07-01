using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Inventory;
using static UnityEditor.Progress;

public class InventoryUI : MonoBehaviour
{
    public static InventoryUI Instance;
    private void Awake()
    {
        Instance = this;
    }

    [SerializeField] private GameObject inventoryPanel;
    private bool ActiveInventory = false;

    [SerializeField] private Transform itemSlotHolder;
    private ItemSlot[] itemSlots;
    [SerializeField] private Text GoldText;
    [SerializeField] private Button SortButton;
    public bool IsGiftInven {  get; private set; }
    private CharacterType NPCType;
    private ItemType giftType;
    [SerializeField] private GameObject allSellButton;
    [SerializeField] private GameObject sellButton;
    [SerializeField] private GameObject giftButton;
    [SerializeField] private GameObject invenFullText;

    private Inventory inventory;
     
    private void Start()
    {
        itemSlots = itemSlotHolder.GetComponentsInChildren<ItemSlot>();
        inventoryPanel.SetActive(ActiveInventory);

        inventory = Inventory.Instance;
        inventory.onChangeItem += RedrawSlotUI;

        ToolTip.Instance.gameObject.SetActive(false);   // 초기 툴팁 비활성화

        RedrawSlotUI();
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
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if(ActiveInventory)
            {
                if(!IsGiftInven)
                    ShowInven(false);
                else
                    HideGiftInven();
            }
            else
            {
                ShowInven(true);
            }
        }
    }

    // 버튼에서 사용함.
    public void CloseInven()
    {
        if (ActiveInventory)
        {
            if (!IsGiftInven)
                ShowInven(false);
            else
                HideGiftInven();
        }
    }

    public void ShowInven(bool show)
    {
        ActiveInventory = show;
        inventoryPanel.SetActive(ActiveInventory);
        SoundManager.Instance.PlaySFX(SFX.SortButtonSFX, Camera.main.transform.position);
    }

    public void ShowGiftInven(CharacterType characterType)
    {
        NPCType = characterType;
        giftType = DownloadManager.Instance.playerDatas[(int)characterType].giftType;

        IsGiftInven = true;
        ActiveInventory = true;
        inventoryPanel.SetActive(ActiveInventory);
        
        allSellButton.SetActive(false);
        sellButton.SetActive(false);
        giftButton.SetActive(true);

        SoundManager.Instance.PlaySFX(SFX.SortButtonSFX, Camera.main.transform.position);
    }

    public void HideGiftInven()
    {
        IsGiftInven = false;
        ActiveInventory = false;
        inventoryPanel.SetActive(ActiveInventory);
        allSellButton.SetActive(true);
        sellButton.SetActive(true);
        giftButton.SetActive(false);
        ToolTip.Instance.gameObject.SetActive(false);

        SoundManager.Instance.PlaySFX(SFX.SortButtonSFX, Camera.main.transform.position);
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

    public void GoldTextUpdate()
    {
        // GoldText 업데이트
        GoldText.text = "Gold " + AccountDataManager.Instance.AccountGold.ToString();
    }

    private IEnumerator ButtonCooldownRoutine()
    {
        SortButton.interactable = false; // 버튼 비활성화
        yield return new WaitForSeconds(3); // 3초 대기
        SortButton.interactable = true; // 버튼 다시 활성화
    }

    public void SortButtonClicked()
    {
        inventory.SortItems();
        RedrawSlotUI();
        StartCoroutine(ButtonCooldownRoutine());

        SoundManager.Instance.PlaySFX(SFX.SortButtonSFX, Camera.main.transform.position);
    }
    public void AllSellButtonClicked()
    {
        inventory.AllSell();
        GoldTextUpdate();

        SoundManager.Instance.PlaySFX(SFX.SellSFX, Camera.main.transform.position);
    }
    public void SellButtonClicked()
    {
        inventory.Sell();
        GoldTextUpdate();

        SoundManager.Instance.PlaySFX(SFX.SellSFX, Camera.main.transform.position);

    }
    public void GiftButtonClicked()
    {
        // 툴팁 아이템
        if(ToolTip.Instance.ItemType == giftType)
        {
            inventory.Gift(NPCType);
            int giftCount = AccountDataManager.Instance.GetGiftCount(NPCType);
            DialogueManager.Instance.giftCountText.text = "X " + giftCount.ToString();
            DialogueManager.Instance.ShowDialogueWindow(2f);

            SoundManager.Instance.PlaySFX(SFX.GiftSFX, Camera.main.transform.position);
        }
    }
    public void ShowCantExitText()
    {
        SoundManager.Instance.PlaySFX(SFX.ExitFailSFX, Camera.main.transform.position);
        StartCoroutine(ShowInvenFullTextCoroutine());
    }

    private IEnumerator ShowInvenFullTextCoroutine()
    {
        invenFullText.SetActive(true);
        yield return new WaitForSeconds(2f);
        invenFullText.SetActive(false);
    }
}
