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

    private void LateUpdate()
    {
        UpdateHP();
    }

    public void UpdateHP()
    {
        MaxHealth = GameManager.Instance.GetPlayer().GetComponent<Player>().GetMaxHP();
        Health = GameManager.Instance.GetPlayer().GetComponent<Player>().GetHP();
        HPBar.fillAmount = Health / MaxHealth;
    }
}
