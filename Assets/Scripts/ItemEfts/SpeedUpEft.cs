using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/ItemEftSO/SpeedUpEft")]
public class SpeedUpEft : ItemEffect
{
    private PlayerMovement playerMovement;

    public override bool ExecuteRole() 
    {
        if (playerMovement == null)
            playerMovement = GameManager.Instance.GetPlayer().GetComponent<PlayerMovement>();
        playerMovement.MoveSpeedUp(value1);
        playerMovement.PlaySpeedUpPS();
        SoundManager.Instance.PlaySFX(SFX.SpeedUpSFX, Camera.main.transform.position);

        return true;
    }
}
