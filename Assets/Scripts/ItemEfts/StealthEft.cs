using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/ItemEftSO/StealthEft")]
public class StealthEft : ItemEffect
{
    public override bool ExecuteRole()
    {
        if (player == null)
            player = GameManager.Instance.GetPlayer().GetComponent<Player>();
        player.SetStealthForDuration(value1);

        return true;
    }
}
