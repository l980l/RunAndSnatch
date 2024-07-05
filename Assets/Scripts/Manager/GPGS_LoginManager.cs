using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GooglePlayGames;
using GooglePlayGames.BasicApi;

public class GPGS_LoginManager : MonoBehaviour
{
    [SerializeField] private GameObject AccountDataManager;
    [SerializeField] private GameObject LoginButton;

    [SerializeField] private Text loginText;

    public void GPGS_LoginButtonClicked()
    {
        PlayGamesPlatform.Instance.Authenticate(ProcessAuthentication);
        LoginButton.SetActive(false);
    }

    internal void ProcessAuthentication(SignInStatus status)
    {
        if (status == SignInStatus.Success)
        {
            string displayName = PlayGamesPlatform.Instance.GetUserDisplayName();
            string userID = PlayGamesPlatform.Instance.GetUserId();

            loginText.text = "User Data Loading...";

            // 로그인 완료 후 AccountDataManager 생성
            Instantiate(AccountDataManager, Vector3.zero, Quaternion.identity);
        }
        else
        {
            loginText.text = "Login Fail";
            LoginButton.SetActive(true);
            // Disable your integration with Play Games Services or show a login button
            // to ask users to sign-in. Clicking it should call
            // PlayGamesPlatform.Instance.ManuallyAuthenticate(ProcessAuthentication).
        }
    }
}
