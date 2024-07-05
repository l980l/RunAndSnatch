using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    #region Singleton
    public static GameManager Instance { get; private set; }
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
    [SerializeField] private Text itemCountText;
    [Tooltip ("Miya, Bambi, Leo, Cosmo, Chrono, Misty")]
    [SerializeField] private GameObject[] PlayerPrefab;
    [SerializeField] private SkillUI skillUI;
    [SerializeField] private PortraitUI portraitUI;
    [SerializeField] private Vector2Int OriginalMapSize;
    [SerializeField] private int OriginalItemCount;
    [SerializeField] private int OriginalMonsterCount;
    [SerializeField] private PolygonCollider2D polygonCollider;
    private GameObject Player;

    public GameObject GetPlayer() { return Player; }
    public HeatlhHUD GetHeatlhHUD() { return heatlhHUD; }
    public StaminaHUD GetStaminaHUD() { return staminaHUD; }
    public MapGenerator GetMapGenerator() { return mapGenerator; }
    public void SetItemCount(int Value, int TotalItemCount)
    {
        itemCountText.text = Value.ToString() + " / " + TotalItemCount.ToString();
    }

    private void Start()
    {
        // 던전인 것임. 마을인 경우 CatTownManager에서 플레이어 및 NPC 생성.
        if (mapGenerator)
        {
            SetDungeonData();
            mapGenerator.GenerateMap();

            // 랜덤한 위치로 플레이어 생성. 시네머신, 초상화UI 세팅
            ChangePlayer(Instantiate(PlayerPrefab[(int)GPGS_AccountDataManager.Instance.SelectedCharacter], mapGenerator.RandomPos(false), Quaternion.identity));

            // SkillUI에 플레이어 세팅
            skillUI.SetPlayer(Player.GetComponent<PlayerMovement>());
            // 아이템 생성
            ItemDB.Instance.GenerateItemsOnField();
            // 몬스터 생성
            MonsterManager.Instance.GenerateMonstersOnField();

            // 던전에 위치함을 저장
            GPGS_AccountDataManager.Instance.InDungeon = true;
            GPGS_AccountDataManager.Instance.SaveJsonToCloud();
        }
    }

    // 마을에서 캐릭터 바꾸면 호출될 함수.
    public void ChangePlayer(GameObject _nextPlayer)
    {
        Player = _nextPlayer;
        // 시네머신 팔로우 플레이어 트랜스폼으로 세팅.
        CVC.m_Follow = Player.transform;
        // 초상화UI 세팅
        portraitUI.SetPortrait(Player.GetComponent<PlayerMovement>().PlayerData.portraitImage);
    }

    private void SetDungeonData()
    {
        var accountDataManager = GPGS_AccountDataManager.Instance;
        var itemDB = ItemDB.Instance;
        var monsterManager = MonsterManager.Instance;

        float multiplier = 1;
        for(int i = 0; i < accountDataManager.ExitStreak; ++i)
        {
            multiplier *= 1.2f;
        }

        int newSizeX = Mathf.RoundToInt(OriginalMapSize.x * multiplier);
        int newSizeY = Mathf.RoundToInt(OriginalMapSize.y * multiplier);
        int newItemCount = Mathf.RoundToInt(OriginalItemCount * multiplier);
        int newMonsterCount = Mathf.RoundToInt(OriginalMonsterCount * multiplier);

        mapGenerator.SetSize(newSizeX, newSizeY);
        itemDB.SetTotalItemCount(newItemCount);
        monsterManager.SetMonsterCount(newMonsterCount);

        float halfWidth = newSizeX / 2f;
        float halfHeight = newSizeY / 2f;
        Vector2[] scaledPoints = new Vector2[4];
        scaledPoints[0] = new Vector2(halfWidth, halfHeight);
        scaledPoints[1] = new Vector2(-halfWidth, halfHeight);
        scaledPoints[2] = new Vector2(-halfWidth, -halfHeight);
        scaledPoints[3] = new Vector2(halfWidth, -halfHeight);
        polygonCollider.points = scaledPoints;
    }
}
