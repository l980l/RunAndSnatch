using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/ItemEftSO/StaminaRegenUpEft")]
public class StaminaRegenUpEft : ItemEffect
{
    private PlayerStamina playerStamina;

    public override bool ExecuteRole() 
    {
        if (playerStamina == null)
            playerStamina = GameManager.Instance.GetPlayer().GetComponent<PlayerStamina>();
        playerStamina.StaminaRegenSpeedUp(value1);
        return true;
    }
}
