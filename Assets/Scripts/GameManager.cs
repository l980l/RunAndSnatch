using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private CinemachineVirtualCamera CVC;

    [SerializeField] private HeatlhHUD heatlhHUD;
    [SerializeField] private StaminaHUD StaminaHUD;
    [SerializeField] private Text ItemValueText;

    [SerializeField] private MapGenerator mapGenerator;
    [SerializeField] private GameObject PlayerPrefab;
    [SerializeField] private GameObject[] ItemPrefab;
    [SerializeField] private int ItemCount;
    [SerializeField] private GameObject[] MonsterPrefabs;
    [SerializeField] private GameObject ExitPrefab;

    private GameObject Player;
    private int TotalItemValue;

    public GameObject GetPlayer() { return Player; }
    public HeatlhHUD GetHeatlhHUD() { return heatlhHUD; }
    public StaminaHUD GetStaminaHUD() { return StaminaHUD; }
    public void SetItemValue(int Value)
    {
        ItemValueText.text = Value.ToString() + " / " + TotalItemValue;
    }

    private void Awake()
    {
        Instance = this;
        TotalItemValue = 0;
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
            for (int i = 0; i < ItemCount; i++)
            {
                int index = Random.Range(0, ItemPrefab.Count());
                // Item 가치 누적
                TotalItemValue += ItemPrefab[index].GetComponent<Item>().GetItemValue();
                Instantiate(ItemPrefab[index], mapGenerator.RandomPos(false), Quaternion.identity);
            }
            // ItemValueText 세팅
            SetItemValue(0);
        }
    }
}
