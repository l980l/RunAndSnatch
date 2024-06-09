using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ItemEft/Healing")]
public class ItemHealingEft : ItemEffect
{
    [SerializeField] private int HealingPoint = 20;
    public override bool ExecuteRole()
    {
        GameManager.Instance.GetPlayer().GetComponent<Player>().AddHP(HealingPoint);
        return true;
    }
}
