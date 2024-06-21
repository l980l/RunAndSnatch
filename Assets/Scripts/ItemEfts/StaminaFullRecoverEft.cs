using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

[CreateAssetMenu(menuName = "ScriptableObject/ItemEftSO/StaminaFullRecoverEft")]
public class StaminaFullRecoverEft : ItemEffect
{ 
    public override bool ExecuteRole()
    {
        if (player == null)
            player = GameManager.Instance.GetPlayer().GetComponent<Player>();
        player.AddStemina(value1);
        return true;
    }
}
