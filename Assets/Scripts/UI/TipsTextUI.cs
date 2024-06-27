using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering.LookDev;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;

public class TipsTextUI : MonoBehaviour
{
    [SerializeField] private Tips tips;
    [SerializeField] private float holdTime;
    private LanguageType language;
    private Text tipsText;
    private int prevIndex;

    private void Awake()
    {
        tipsText = GetComponent<Text>();
    }

    private void Start()
    {
        language = AccountDataManager.Instance.LanguageType;
        prevIndex = Random.Range(0, tips.EnTips.Count);

        if (language == LanguageType.En)
            tipsText.text = "Tips: " + tips.EnTips[prevIndex];
        else
            tipsText.text = "Tips: " + tips.KrTips[prevIndex];

        StartCoroutine(ShowTipsCoroutine());
    }

    private IEnumerator ShowTipsCoroutine()
    {
        yield return new WaitForSecondsRealtime(holdTime);

        int newIndex = Random.Range(0, tips.EnTips.Count);
        while (newIndex == prevIndex)
        {
            newIndex = Random.Range(0, tips.EnTips.Count);
        }
        prevIndex = newIndex;

        if (language == LanguageType.En)
            tipsText.text = "Tips: " + tips.EnTips[prevIndex];
        else
            tipsText.text = "Tips: " + tips.KrTips[prevIndex];

        StartCoroutine(ShowTipsCoroutine());
    }
}
