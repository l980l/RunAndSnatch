using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathUI : MonoBehaviour
{
    #region Singleton
    public static DeathUI Instance;
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

    [SerializeField] private GameObject DeathPanel;
    [SerializeField] private int CatTownSceneInt;
    private Animator animator;
    private WaitForSecondsRealtime waitForSecondsRealtime;

    private void Start()
    {
        waitForSecondsRealtime = new WaitForSecondsRealtime(2f);
        DeathPanel.SetActive(false);
        animator = DeathPanel.GetComponent<Animator>();
    }

    public void Death()
    {
        GPGS_AccountDataManager.Instance.DeathPenalty();
        ShowDeathPanel();
    }

    private void ShowDeathPanel()
    {
        StartCoroutine(ShowDeathPanelCoroutine());
    }

    private IEnumerator ShowDeathPanelCoroutine()
    {
        SoundManager.Instance.StopBGM();
        DeathPanel.SetActive(true);
        Time.timeScale = 0f;
        yield return waitForSecondsRealtime;
        Time.timeScale = 1f;
        LoadingSceneController.LoadScene(CatTownSceneInt);
    }
}
