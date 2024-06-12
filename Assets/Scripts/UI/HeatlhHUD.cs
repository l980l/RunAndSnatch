using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeatlhHUD : MonoBehaviour
{
    private Image HPBar;

    private void Awake()
    {
        HPBar = GetComponent<Image>(); 
    }

    public void UpdateHP(float amount)  // Player에서 데미지를 받으면 호출.
    {
        HPBar.fillAmount = amount;
    }
}
