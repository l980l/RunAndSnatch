using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class NetworkManager : MonoBehaviour
{
    #region Singleton
    public static NetworkManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    #endregion

    [SerializeField] private GameObject NetworkUI;
    public bool IsNetworkConnected {  get; private set; }
    public float LastDisconnectTime { get; private set; }
    public float DisconnectTime { get; private set; }

    void Start()
    {
        IsNetworkConnected = false;
        NetworkUI.SetActive(false);
        StartCoroutine(CheckNetworkConnection());
    }

    IEnumerator CheckNetworkConnection()
    {
        while (true)
        {
            if (Application.internetReachability == NetworkReachability.NotReachable)
            {
                if (IsNetworkConnected)
                {
                    // 게임을 멈춤
                    NetworkUI.SetActive(true);
                    Time.timeScale = 0f;
                    IsNetworkConnected = false;
                    LastDisconnectTime = Time.realtimeSinceStartup; // 네트워크가 끊기기 시작한 시간 기록.
                }
            }
            else
            {
                if (!IsNetworkConnected)
                {
                    // 게임 재개
                    NetworkUI.SetActive(false);
                    Time.timeScale = 1f;
                    IsNetworkConnected = true;
                    DisconnectTime = Time.realtimeSinceStartup - LastDisconnectTime; // 네트워크 접속이 끊긴 시간 계산
                }
            }

            yield return new WaitForSecondsRealtime(2f); // 2초마다 네트워크 상태 확인
        }
    }
}
