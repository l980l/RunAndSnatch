using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // 싱글톤으로 지정.
    #region Singleton
    public static GameManager Instance;
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

    [SerializeField] private CinemachineVirtualCamera CVC;

    [SerializeField] private HeatlhHUD heatlhHUD;
    [SerializeField] private StaminaHUD staminaHUD;
    [SerializeField] private MapGenerator mapGenerator;
    [SerializeField] private Text itemValueText;
    [Tooltip ("Miya, Bambi, Leo, Cosmo, Chrono, Misty")]
    [SerializeField] private GameObject[] PlayerPrefab;
    [SerializeField] private GameObject ExitPrefab;
    [SerializeField] private SkillUI skillUI;
    [SerializeField] private PortraitUI portraitUI;
    [Tooltip("OnlySetInCatTown")] [SerializeField] private GameObject Player;

    public GameObject GetPlayer() { return Player; }
    public HeatlhHUD GetHeatlhHUD() { return heatlhHUD; }
    public StaminaHUD GetStaminaHUD() { return staminaHUD; }
    public MapGenerator GetMapGenerator() { return mapGenerator; }
    public void SetItemValue(int Value, int TotalItemValue)
    {
        itemValueText.text = Value.ToString() + " / " + TotalItemValue.ToString();
    }

    private void Start()
    {
        // 던전인 것임.
        if(mapGenerator)
        {
            // 랜덤한 위치로 플레이어 생성. 시네머신, 초상화UI 세팅
            ChangePlayer(Instantiate(PlayerPrefab[(int)AccountDataManager.Instance.SelectedCharacter], mapGenerator.RandomPos(false), Quaternion.identity));

            // SkillUI에 플레이어 세팅
            skillUI.SetPlayer(Player.GetComponent<PlayerMovement>());
            // 아이템 생성
            ItemDB.Instance.GenerateItemsOnField();
            // 몬스터 생성
            MonsterManager.Instance.GenerateMonstersOnField();
        }

        // 마을인 것임.
        else
        {
            ChangePlayer(Instantiate(PlayerPrefab[(int)AccountDataManager.Instance.SelectedCharacter], new Vector3(-10, 0.8f, 0), Quaternion.identity));
        }
    }

    // 마을에서 캐릭터 바꾸면 호출될 함수.
    private void ChangePlayer(GameObject _nextPlayer)
    {
        Player = _nextPlayer;
        // 시네머신 팔로우 플레이어 트랜스폼으로 세팅.
        CVC.m_Follow = Player.transform;
        // 초상화UI 세팅
        portraitUI.SetPortrait(Player.GetComponent<PlayerMovement>().PlayerData.portraitImage);
    }
}
