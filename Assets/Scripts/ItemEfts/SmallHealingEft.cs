using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/ItemEftSO/SmallHealingEft")] 
public class SmallHealingEft : ItemEffect
{
    private PlayerHealth playerHealth;

    public override bool ExecuteRole()
    {
        if (playerHealth == null)
            playerHealth = GameManager.Instance.GetPlayer().GetComponent<PlayerHealth>();
        playerHealth.AddHP((int)value1);
        playerHealth.PlaySmallHealPS();
        SoundManager.Instance.PlaySFX(SFX.HealingSmallSFX, Camera.main.transform.position);

        return true;
    }
}
