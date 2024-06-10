using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScrittableObject/ItemEftSO/SpeedUpEft")]
public class SpeedUpEft : ItemEffect
{
    public override bool ExecuteRole()
    {
        GameManager.Instance.GetPlayer().GetComponent<Player>().MoveSpeedUp(value1);
        return true;
    }
}
