using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ItemEftSO/StealthEft")]
public class StealthEft : ItemEffect
{
    public override bool ExecuteRole()
    {
        GameManager.Instance.GetPlayer().GetComponent<Player>().StaminaRegenSpeedUp(value1);
        return true;
    }
}
