using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/ItemEftSO/StaminaRegenUpEft")]
public class StaminaRegenUpEft : ItemEffect
{
    public override bool ExecuteRole() 
    {
        if (player == null)
            player = GameManager.Instance.GetPlayer().GetComponent<Player>();
        player.StaminaRegenSpeedUp(value1);
        return true;
    }
}
