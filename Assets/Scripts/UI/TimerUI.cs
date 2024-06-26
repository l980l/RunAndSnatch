using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerUI : MonoBehaviour
{
    public float FullTime {  get; private set; }
    public float RemainTime {  get; private set; }
    private Image TimerImage; 
    private bool death;

    private void Awake()
    {
        FullTime = 300f;
        RemainTime = FullTime;
        TimerImage = GetComponent<Image>();
    }

    private void Update()
    {
        RemainTime -= Time.deltaTime;
        TimerImage.fillAmount = RemainTime / FullTime;

        // »ç¸Á
        if(!death && RemainTime <= 0)
        {
            DeathUI.Instance.Death();
            death = true;
        }
    }
}
