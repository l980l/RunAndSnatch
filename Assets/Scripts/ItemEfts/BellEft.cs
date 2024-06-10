using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScrittableObject/ItemEftSO/BellEft")]
public class BellEft : ItemEffect
{
    public override bool ExecuteRole()
    {
        //GameManager.Instance.GetPlayer().GetComponent<Player>().StaminaRegenSpeedUp(value1);
        return true;
    }
}
