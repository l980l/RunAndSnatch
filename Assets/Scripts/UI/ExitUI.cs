using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ExitUI : MonoBehaviour
{
    #region singletone
    public static ExitUI instance;
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }
    #endregion

    private PlayerItemCollect playerItemCollect;
    [SerializeField] private GameObject cantExitText;
    [SerializeField] private GameObject exitUIPanel;
    [SerializeField] private Text exitStreakText;
    [SerializeField] private Text exitTimeText;
    [SerializeField] private Text collectedItemText;
    [SerializeField] private TimerUI timerUI;

    [SerializeField] private int DungeonSceneInt;
    [SerializeField] private int CatTownSceneInt;

    private void Start()
    {
        cantExitText.SetActive(false);
        exitUIPanel.SetActive(false);
    }

    public void OpenExitUI()
    {
        if (playerItemCollect == null)
        {
            playerItemCollect = GameManager.Instance.GetPlayer().GetComponent<PlayerItemCollect>();
        }

        // 아이템 70프로 이상 획득.
        if (playerItemCollect.AcquiredItemCount >= ItemDB.Instance.GetTotalItemCount() * 0.7f)
        {
            SoundManager.Instance.PlaySFX(SFX.ExitSuccessSFX, Camera.main.transform.position);

            // 연속 탈출 횟수, 던전 여부 업데이트
            GPGS_AccountDataManager.Instance.ExitStreak++;
            GPGS_AccountDataManager.Instance.InDungeon = false;
            GPGS_AccountDataManager.Instance.SaveJsonToCloud();

            exitUIPanel.SetActive(true);
            Time.timeScale = 0f;

            exitStreakText.text = "Exit Streak: " + GPGS_AccountDataManager.Instance.ExitStreak.ToString();
            exitTimeText.text = "Time Taken: " + (timerUI.FullTime -timerUI.RemainTime).ToString();
            collectedItemText.text = "Snatched Item Count: " + playerItemCollect.AcquiredItemCount.ToString();
        }

        // 2초간 아이템 더 가져와야 된다는 텍스트 띄우기.
        else
        {
            ShowCantExitText();
        }
    }

    private void ShowCantExitText()
    {
        SoundManager.Instance.PlaySFX(SFX.ExitFailSFX, Camera.main.transform.position);
        StartCoroutine(ShowCantExitTextCoroutine());
    }

    private IEnumerator ShowCantExitTextCoroutine()
    {
        cantExitText.SetActive(true);
        yield return new WaitForSeconds(2f);
        cantExitText.SetActive(false);
    }

    public void NextDungeonButtonClick()
    {
        Time.timeScale = 1f;
        LoadingSceneController.LoadScene(DungeonSceneInt);
        SoundManager.Instance.PlaySFX(SFX.ButtonSFX, Camera.main.transform.position);
    }

    public void ReturnToTownButtonClick()
    {
        Time.timeScale = 1f;
        LoadingSceneController.LoadScene(CatTownSceneInt);
        SoundManager.Instance.PlaySFX(SFX.ButtonSFX, Camera.main.transform.position);
    }
}
