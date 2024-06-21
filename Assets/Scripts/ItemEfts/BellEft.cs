using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/ItemEftSO/BellEft")]
public class BellEft : ItemEffect 
{
    public override bool ExecuteRole()
    {
        if (player == null)
            player = GameManager.Instance.GetPlayer().GetComponent<Player>();
        player.SetStunAreaForDuration(value1);

        return true;
    }
}
