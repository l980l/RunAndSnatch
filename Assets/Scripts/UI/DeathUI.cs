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

    private void Start()
    {
        DeathPanel.SetActive(false);
    }

    public void Death()
    {
        AccountDataManager.Instance.DeathPenalty();
        ShowDeathPanel();
    }

    private void ShowDeathPanel()
    {
        StartCoroutine(ShowDeathPanelCoroutine());
    }

    private IEnumerator ShowDeathPanelCoroutine()
    {
        Time.timeScale = 0f;
        DeathPanel.SetActive(true);
        yield return new WaitForSecondsRealtime(2f);
        Time.timeScale = 1f;
        LoadingSceneController.LoadScene(CatTownSceneInt);
    }
}
