using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeatlhHUD : MonoBehaviour
{
    private int MaxHealth;
    private int Health;
    private Image HPBar;
    private GameObject Player;

    private void Awake()
    {
        HPBar = GetComponent<Image>();
    }

    public void UpdateHP()  // Player에서 데미지를 받으면 호출.
    {
        MaxHealth = GameManager.Instance.GetPlayer().GetComponent<Player>().GetMaxHP();
        Health = GameManager.Instance.GetPlayer().GetComponent<Player>().GetHP();
        HPBar.fillAmount = (float)Health / (float)MaxHealth;
    }
}
