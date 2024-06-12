using UnityEngine;
using System;
using Unity.Services.Core;
using Unity.Services.Authentication;
using System.Threading.Tasks;
using System.Collections.Generic;
using Unity.Services.CloudSave;
using Unity.Services.CloudSave.Models;

public class LoginManager : MonoBehaviour
{
    [SerializeField] private GameObject AccountDataManager;
    private async void Awake()
    {
        try
        {
            await UnityServices.InitializeAsync();
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }

    private async void Start()
    {
        SetupEvents();

        // 로그인 상태 확인
        if (!AuthenticationService.Instance.IsSignedIn)
        {
            // 로그인 안 한 경우에는 익명으로 로그인
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
            // Prevent throws due to Scene changes 
            if (this == null)
                return;
        }

        // 로그인 완료 후 AccountDataManager 생성
        Instantiate(AccountDataManager, Vector3.zero, Quaternion.identity);
    }

    // Setup authentication event handlers if desired
    private void SetupEvents()
    {
        AuthenticationService.Instance.SignedIn += () => {
            // Shows how to get a playerID
            Debug.Log($"PlayerID: {AuthenticationService.Instance.PlayerId}");

            // Shows how to get an access token
            Debug.Log($"Access Token: {AuthenticationService.Instance.AccessToken}");

        };

        AuthenticationService.Instance.SignInFailed += (err) => {
            Debug.LogError(err);
        };

        AuthenticationService.Instance.SignedOut += () => {
            Debug.Log("Player signed out.");
        };

        AuthenticationService.Instance.Expired += () =>
        {
            Debug.Log("Player session could not be refreshed and expired.");
        };
    }
}
