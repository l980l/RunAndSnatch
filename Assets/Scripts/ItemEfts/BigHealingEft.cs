using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

[CreateAssetMenu(menuName = "ScriptableObject/ItemEftSO/BigHealingEft")]
public class BigHealingEft : ItemEffect 
{
    public override bool ExecuteRole()
    {
        GameManager.Instance.GetPlayer().GetComponent<Player>().AddHP((int)value1);
        return true;
    }
}
