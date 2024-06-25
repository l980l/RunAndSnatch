using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    #region Singleton
    public static DialogueManager Instance;
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    #endregion

    public Text nameText;
    public Text dialogueText;
    public Animator animator;
    public LanguageType currentLanguage = LanguageType.Kr; // 기본 언어 설정
    public Button closeButton;
    public GameObject nextButton;
    public Button converseButton;
    public Button giftButton;
    public Button changeButton;
    public Text converseButtonText;
    public Text giftButtonText;
    public Text changeButtonText;
    public Image giftImage;
    public Text giftCountText;

    private bool OnConversation;    // 대화중인지

    private Queue<string> sentences;

    private static readonly int isShowHash = Animator.StringToHash("isShow");

    void Start()
    {
        sentences = new Queue<string>();
    }

    private void Update()
    {
        // PC로 플레이 한다면, space바로 대화 continue 가능.
        if (OnConversation && Input.GetButtonDown("Jump"))
        {
            DisplayNextSentence();
        }
    }

    public void StartDialogue(Dialogue dialogue, CharacterType characterType, GameObject player, GameObject npc)
    {
        animator.SetBool(isShowHash, true);
        closeButton.gameObject.SetActive(true);
        nextButton.gameObject.SetActive(false);
        OnConversation = false;
        SetLanguage(AccountDataManager.Instance.LanguageType);
       
        if (currentLanguage == LanguageType.En)
            nameText.text = dialogue.characterNameEN;
        else
            nameText.text = dialogue.characterNameKR;

        SetGiftUI(characterType);
        dialogueText.text = "";
        SetActiveButtons(true);

        SetConverseButton(dialogue);
        SetGiftButton(characterType, player, npc);
        SetChangeButton(characterType, player, npc);
    }

    private void SetConverseButton(Dialogue dialogue)
    {
        converseButton.enabled = true;

        // 버튼 OnClick 재할당.
        converseButton.onClick.RemoveAllListeners();
        converseButton.onClick.AddListener(() => StartConversation(dialogue));

        // 대화 및 선물하기 텍스트 세팅
        if (currentLanguage == LanguageType.En)
        {
            converseButtonText.text = "Start a Conversation.";
        }
        else
        {
            converseButtonText.text = "대화하기.";
        }
    }

    private void SetGiftButton(CharacterType characterType, GameObject player, GameObject npc)
    {
        giftButton.enabled = true;

        // 버튼 OnClick 재할당.
        giftButton.onClick.RemoveAllListeners();

        // 대화 및 선물하기 텍스트 세팅
        if (currentLanguage == LanguageType.En)
        {
            giftButtonText.text = "Give a Present";
        }
        else
        {
            giftButtonText.text = "선물하기.";
        }
    }

    private void SetChangeButton(CharacterType characterType, GameObject player, GameObject npc)
    {
        changeButton.enabled = true;
        changeButton.interactable = true;

        bool playable = AccountDataManager.Instance.GetPlayable(characterType);
        int giftCount = AccountDataManager.Instance.GetGiftCount(characterType);

        changeButton.onClick.RemoveAllListeners();

        // 플레이어 체인지 버튼 세팅.
        // 해금된 플레이어인 경우
        if (playable)
        {
            if (currentLanguage == LanguageType.En)
                changeButtonText.text = "It's your turn.";
            else if (currentLanguage == LanguageType.Kr)
                changeButtonText.text = "네 차례야.";

            changeButton.onClick.AddListener(() => ChangePlayer(player, npc));
        }

        // 잠금된 플레이어인 경우, changeButton을 해금 버튼으로 변경
        else
        {
            // 일단 테스트를 위해 한번 선물하면 구매 가능으로.
            int giftThresholod = 1;

            if (giftCount >= giftThresholod)
            {
                if (currentLanguage == LanguageType.En)
                    changeButtonText.text = "I'll hire you.";
                else if (currentLanguage == LanguageType.Kr)
                    changeButtonText.text = "너를 고용할게.";

                changeButton.onClick.AddListener(() => PurchasePlayer(characterType, npc));

                // 잔액이 부족한 경우
                if (AccountDataManager.Instance.AccountGold < DownloadManager.Instance.playerDatas[(int)characterType].hireCost)
                {
                    if (currentLanguage == LanguageType.En)
                        changeButtonText.text = "(Need more gold)";
                    else if (currentLanguage == LanguageType.Kr)
                        changeButtonText.text = "(돈이 더 필요해)";
                    // interactable 끄기
                    changeButton.interactable = false;
                }
            }

            // 선물이 부족한 경우
            else
            {
                if (currentLanguage == LanguageType.En)
                    changeButtonText.text = "(Need more gifts)";
                else if (currentLanguage == LanguageType.Kr)
                    changeButtonText.text = "(선물이 더 필요해)";
                // interactable 끄기
                changeButton.interactable = false;
            }
        }
    }

    private void StartConversation(Dialogue dialogue)
    {
        nextButton.gameObject.SetActive(true);
        SetActiveButtons(false);
        OnConversation = true;

        sentences.Clear();

        string[] selectedSentences;
        if (currentLanguage == LanguageType.En)
        {
            selectedSentences = dialogue.sentencesEN;
        }
        else
        {
            selectedSentences = dialogue.sentencesKR;
        }

        foreach (string sentence in selectedSentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            OffDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return null;
        }
    }

    private void ChangePlayer(GameObject player, GameObject npc)
    {
        player.GetComponent<PlayerNPC>().enabled = true;
        npc.GetComponent<PlayerNPC>().BePlayer();
        OffDialogue();
    }

    private void PurchasePlayer(CharacterType characterType, GameObject npc)
    {
        AccountDataManager.Instance.AccountGold -= DownloadManager.Instance.playerDatas[(int)characterType].hireCost;
        AccountDataManager.Instance.SetPlayable(characterType);
        AccountDataManager.Instance.SaveJsonToCloud();

        npc.GetComponent<PlayerNPC>().UpdateAccountData();
        OffDialogue();
    }

    private void SetActiveButtons(bool _active)
    {
        converseButton.gameObject.SetActive(_active);
        giftButton.gameObject.SetActive(_active);
        changeButton.gameObject.SetActive(_active);
    }

    public void OffDialogue()
    {
        closeButton.gameObject.SetActive(false);
        animator.SetBool(isShowHash, false);
        OnConversation = false;
    }

    private void SetGiftUI(CharacterType characterType)
    {
        ItemType giftType = DownloadManager.Instance.playerDatas[(int)characterType].giftType;
        giftImage.sprite = ItemDB.Instance.itemDBSO.items[(int)giftType].ItemImage;
        int giftCount = AccountDataManager.Instance.GetGiftCount(characterType);
        giftCountText.text = "X " + giftCount.ToString();
    }

    public void SetLanguage(LanguageType language)
    {
        currentLanguage = language;
    }
}
