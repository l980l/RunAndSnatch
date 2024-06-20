using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/ItemEftSO/StealthEft")]
public class StealthEft : ItemEffect
{
    public override bool ExecuteRole()
    {
        GameManager.Instance.GetPlayer().GetComponent<Player>().SetStealthForDuration(value1);

        return true;
    }
}
