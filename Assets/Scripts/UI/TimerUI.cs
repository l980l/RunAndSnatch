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
    private int AudioSourceIndex = -1;

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

        // 남은 시간이 적다면 
        if(!death && RemainTime <= 10f)
        {
            if (AudioSourceIndex == -1)
                AudioSourceIndex = SoundManager.Instance.PlayLoopSFX(SFX.TimerSFX, Camera.main.transform.position);
        }

        // 사망
        if(!death && RemainTime <= 0)
        {
            GameManager.Instance.GetPlayer().GetComponent<PlayerHealth>().Die();

            death = true;
            if (AudioSourceIndex != -1)
                SoundManager.Instance.StopLoopSFX(AudioSourceIndex);
        }
    }
}
