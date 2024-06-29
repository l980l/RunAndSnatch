using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

[CreateAssetMenu(menuName = "ScriptableObject/ItemEftSO/StaminaFullRecoverEft")]
public class StaminaFullRecoverEft : ItemEffect
{
    private PlayerStamina playerStamina;

    public override bool ExecuteRole()
    {
        if (playerStamina == null)
            playerStamina = GameManager.Instance.GetPlayer().GetComponent<PlayerStamina>();
        playerStamina.AddStamina(value1);
        SoundManager.Instance.PlaySFX(SFX.HealingLargeSFX, Camera.main.transform.position);

        return true;
    }
}
