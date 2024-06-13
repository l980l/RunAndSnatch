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
    [SerializeField] private GameObject PlayerPrefab;
    [SerializeField] private GameObject ExitPrefab;
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
        if(mapGenerator)
        {
            // 랜덤한 위치로 플레이어 생성. 초기 HealthHUD 세팅.
            Player = Instantiate(PlayerPrefab, mapGenerator.RandomPos(false), Quaternion.identity);
            // 시네머신 팔로우 플레이어 트랜스폼으로 세팅.
            CVC.m_Follow = Player.transform;
            // 아이템 생성
            ItemDB.Instance.GenerateItemsOnField();
            // 몬스터 생성
            MonsterManager.Instance.GenerateMonstersOnField();
        }
    }
}
