using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/ItemEftSO/SpeedUpEft")]
public class SpeedUpEft : ItemEffect
{
    public override bool ExecuteRole() 
    {
        if (player == null)
            player = GameManager.Instance.GetPlayer().GetComponent<Player>();
        player.MoveSpeedUp(value1);
        return true;
    }
}
