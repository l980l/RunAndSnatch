using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

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
    [SerializeField] private StaminaHUD StaminaHUD;
    [SerializeField] private MapGenerator mapGenerator;
    [SerializeField] private Text ItemValueText;
    [Tooltip ("Miya, Bambi, Leo, Cosmo, Chrono, Misty")]
    [SerializeField] private GameObject[] PlayerPrefab;
    [SerializeField] private GameObject ExitPrefab;
    [SerializeField] private SkillUI skillUI;
    [Tooltip("OnlySetInCatTown")] [SerializeField] private GameObject Player;

    public GameObject GetPlayer() { return Player; }
    public HeatlhHUD GetHeatlhHUD() { return heatlhHUD; }
    public StaminaHUD GetStaminaHUD() { return StaminaHUD; }
    public MapGenerator GetMapGenerator() { return mapGenerator; }
    public void SetItemValue(int Value, int TotalItemValue)
    {
        ItemValueText.text = Value.ToString() + " / " + TotalItemValue.ToString();
    }

    private void Start()
    {
        // 던전인 것임.
        if(mapGenerator)
        {
            // 랜덤한 위치로 플레이어 생성. 초기 HealthHUD 세팅.
            Player = Instantiate(PlayerPrefab[(int)AccountDataManager.Instance.SelectedCharacter], mapGenerator.RandomPos(false), Quaternion.identity);
            // SkillUI에 플레이어 세팅
            skillUI.SetPlayer(Player.GetComponent<Player>());
            // 시네머신 팔로우 플레이어 트랜스폼으로 세팅.
            CVC.m_Follow = Player.transform;
            // 아이템 생성
            ItemDB.Instance.GenerateItemsOnField();
            // 몬스터 생성
            MonsterManager.Instance.GenerateMonstersOnField();
        }

        // 마을인 것임.
        else
        {
            Player = Instantiate(PlayerPrefab[(int)AccountDataManager.Instance.SelectedCharacter], new Vector3(-10, 0.8f, 0), Quaternion.identity);
            CVC.m_Follow = Player.transform;
        }
    }
}
