using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

[CreateAssetMenu(menuName = "ItemEftSO/StaminaFullRecoverEft")]
public class StaminaFullRecoverEft : ItemEffect
{
    public override bool ExecuteRole()
    {
        GameManager.Instance.GetPlayer().GetComponent<Player>().AddStemina(value1);
        return true;
    }
}
