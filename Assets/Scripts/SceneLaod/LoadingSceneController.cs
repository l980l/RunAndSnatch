using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingSceneController : MonoBehaviour
{
    static int nextSceneIndex;  // 다음 씬 번호

    [SerializeField]
    Image progressbar; 

    public static void LoadScene(int SceneIndex)
    {
        nextSceneIndex = SceneIndex;              // 로딩씬에서 로딩할 씬.
        SceneManager.LoadScene("LoadingScene");   // 로딩씬은 작으니까 그냥 동기로 로딩.
    }
    
    private void Start()
    {
        AdMobManager.Instance.ToggleBannerAd(true);
        StartCoroutine(LoadSceneProcess());
    }

    IEnumerator LoadSceneProcess()
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(nextSceneIndex);
        op.allowSceneActivation = false;    // 씬을 로딩하면 자동으로 불러온 씬으로 이동할 것인지. false로 세팅.

        float timer = 0f;
        while (!op.isDone)
        {
            yield return null;

            if(op.progress < 0.7f)
            {
                progressbar.fillAmount = op.progress;
            }
            else
            {
                timer += Time.unscaledDeltaTime;
                progressbar.fillAmount = Mathf.Lerp(0.7f, 1f, timer / 3f);
                if (progressbar.fillAmount >= 1f)
                {
                    op.allowSceneActivation = true;
                    AdMobManager.Instance.ToggleBannerAd(false);
                    yield break;
                }
            }
        }
    }
}
