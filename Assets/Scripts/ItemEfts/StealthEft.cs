using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/ItemEftSO/StealthEft")]
public class StealthEft : ItemEffect
{
    private PlayerStealth playerStealth;

    public override bool ExecuteRole()
    {
        if (playerStealth == null)
            playerStealth = GameManager.Instance.GetPlayer().GetComponent<PlayerStealth>();
        playerStealth.SetStealthForDuration(value1);

        return true;
    }
}
