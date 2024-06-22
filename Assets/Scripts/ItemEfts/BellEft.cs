using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/ItemEftSO/BellEft")]
public class BellEft : ItemEffect 
{
    private PlayerStun playerStun;
    public override bool ExecuteRole()
    {
        if (playerStun == null)
            playerStun = GameManager.Instance.GetPlayer().GetComponent<PlayerStun>();
        playerStun.SetStunAreaForDuration(value1);

        return true;
    }
}
