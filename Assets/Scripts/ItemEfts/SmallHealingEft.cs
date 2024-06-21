using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/ItemEftSO/SmallHealingEft")] 
public class SmallHealingEft : ItemEffect
{
    public override bool ExecuteRole()
    {
        if (player == null)
            player = GameManager.Instance.GetPlayer().GetComponent<Player>();
        player.AddHP((int)value1);
        return true;
    }
}
