using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/ItemEftSO/BigHealingEft")]
public class BigHealingEft : ItemEffect 
{
    private PlayerHealth playerHealth;
    public override bool ExecuteRole()
    {
        if (playerHealth == null)
            playerHealth = GameManager.Instance.GetPlayer().GetComponent<PlayerHealth>();
        playerHealth.AddHP((int)value1);
        playerHealth.PlayLargeHealPS();
        SoundManager.Instance.PlaySFX(SFX.HealingLargeSFX, Camera.main.transform.position);

        return true;
    }
}
