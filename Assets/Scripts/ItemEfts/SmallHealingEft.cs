using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ItemEftSO/SmallHealingEft")]
public class SmallHealingEft : ItemEffect
{
    public override bool ExecuteRole()
    {
        GameManager.Instance.GetPlayer().GetComponent<Player>().AddHP((int)value1);
        return true;
    }
}
