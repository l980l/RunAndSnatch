using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaHUD : MonoBehaviour
{
    private Image StaminaBar;

    private void Awake()
    {
        StaminaBar = GetComponent<Image>();
    }

    public void UpdateStamina(float amount)  // Player FixedUpdate에서 호출
    {
        StaminaBar.fillAmount = amount;
    }
}
